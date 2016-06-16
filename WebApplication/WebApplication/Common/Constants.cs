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

        // Email Configuration
        public static string TEMPLATE_EMAIL_REGISTER = "TempleteEmail";
        public static string TEMPLATE_EMAIL_RESET_PASS = "ResetPassword";

        // Get random password
        public static int DEFAULT_MIN_PASSWORD_LENGTH = 8;
        public static int DEFAULT_MAX_PASSWORD_LENGTH = 10;

        public static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
        public static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
        public static string PASSWORD_CHARS_NUMERIC = "23456789";
        public static string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";

        /// <summary>
        /// Paypal Configuration
        /// </summary>
        /// Account Personal: cuongdvt261@gmail.com | @duvuthacu@
        /// Account Bussiness: admin_test@localhost.com | @duvuthacu@
        /// Url login check blance: https://www.sandbox.paypal.com
        public static string BUSSINESS_EMAIL = "admin_test@localhost.com";
        public static string URL_RETURN = "http://localhost:1775/Cart/Order";
        public static string URL_SUBMIT_PAYMENT = "https://www.sandbox.paypal.com/cgi-bin/webscr";
        public static string TOKEN = "LnfpgO-wVWIYACjMOXlZQucwnLNGJOv_SWPkjsUVT1_dTeSkIWY_qNHXISy";

        public static string SESSION_ACCOUNT = "Account";
        public static string SESSION_ACCOUNT_ID = "AccId";
        public static string SESSION_ROLE = "Role";
    }
}