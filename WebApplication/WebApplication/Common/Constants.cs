using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Common
{
    public class Constants
    {
        public static string PATH_IMAGE = "~/Contents/Images";
        public static string PATH_ADMIN_LAYOUT = "~/Views/Layout/AdminLayout.cshtml";
        public static string PATH_MASTER_PAGE = "~/Views/Layout/MasterPage.cshtml";

        public static string PREFIX_PRODUCT = "PRO";
        public static string PREFIX_CUSTOMER = "CUS";
        public static string PREFIX_STAFF = "STA";
        public static string PREFIX_ORDER = "ORD";
        public static string PREFIX_MANUFACTURER = "MAN";
        public static string PREFIX_PRODUCT_TYPE = "PTY";
        public static string PREFIX_PROMOTION = "PMT";
        public static string PREFIX_QUESTION = "QUE";
        public static string PREFIX_ACCOUNT = "ACC";
        public static string PREFIX_DELIVERY = "DLV";
        public static string TEMPLATE_EMAIL_REGISTER = "TempleteEmail";
        public static string TEMPLATE_EMAIL_RESET_PASS = "ResetPassword";

        // Get random password
        public static int DEFAULT_MIN_PASSWORD_LENGTH = 8;
        public static int DEFAULT_MAX_PASSWORD_LENGTH = 10;

        public static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
        public static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
        public static string PASSWORD_CHARS_NUMERIC = "23456789";
        public static string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";
    }
}