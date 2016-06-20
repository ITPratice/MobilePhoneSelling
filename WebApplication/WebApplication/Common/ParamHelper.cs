using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using WebApplication.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WebApplication.Models
{
    public class ParamHelper
    {
        static readonly ParamHelper _instance = new ParamHelper();

        public static ParamHelper Instance
        {
            get { return _instance; }
        }

        public ParamHelper() { }

        /// <summary>
        /// Get ID of Table follow new form
        /// </summary>
        /// <param name="oldId"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string GetNewId(string oldId, string prefix)
        {
            if (String.IsNullOrEmpty(oldId)) return prefix + "0001";
            string suffix = oldId.Substring(prefix.Length);
            int id = Int16.Parse(suffix) + 1;
            suffix = "0000" + id;
            return prefix + suffix.Substring(suffix.Length - 4);
        }

        /// <summary>
        /// Get MD5 Text
        /// </summary>
        /// <param name="_text"></param>
        /// <returns></returns>
        public string MD5Hash(string _text)
        {
            MD5 _md5 = new MD5CryptoServiceProvider();

            //compute hash from the byte of text
            _md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(_text));

            //get hash result after compute it
            byte[] _result = _md5.Hash;

            StringBuilder _strBuilder = new StringBuilder();
            for (int i = 0; i < _result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                _strBuilder.Append(_result[i].ToString("x2"));
            }
            return _strBuilder.ToString();
        }

        public string GeneratePassword(int minLength,
                                  int maxLength)
        {
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;

            char[][] charGroups = new char[][] 
        {
            Constants.PASSWORD_CHARS_LCASE.ToCharArray(),
            Constants.PASSWORD_CHARS_UCASE.ToCharArray(),
            Constants.PASSWORD_CHARS_NUMERIC.ToCharArray(),
            Constants.PASSWORD_CHARS_SPECIAL.ToCharArray()
        };

            int[] charsLeftInGroup = new int[charGroups.Length];

            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;

            int[] leftGroupsOrder = new int[charGroups.Length];

            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;

            byte[] randomBytes = new byte[4];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            int seed = BitConverter.ToInt32(randomBytes, 0);

            Random random = new Random(seed);

            char[] password = null;

            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];

            int nextCharIdx;

            int nextGroupIdx;

            int nextLeftGroupsOrderIdx;

            int lastCharIdx;

            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            for (int i = 0; i < password.Length; i++)
            {
                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);

                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);

                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;
                else
                {
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    charsLeftInGroup[nextGroupIdx]--;
                }

                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                else
                {
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    lastLeftGroupsOrderIdx--;
                }
            }

            return new string(password);
        }

        public Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}