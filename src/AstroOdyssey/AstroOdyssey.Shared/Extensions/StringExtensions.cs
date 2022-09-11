using System;
using System.Net.Mail;
using System.Text;
using System.IO;

namespace AstroOdyssey
{
    public static class StringExtensions
    {
        private static readonly string encryptionKey1 = "ASDWU&*^%JHJOOI)()^&HJ*^*^&KLJ:KLHJH";
        private static readonly string encryptionKey2 = "IYUHKJ(*&(*%^*GKHJGJHRTU%*^(*&YHOUIH";

        /// <summary>
        /// Checks if the provided string is null or empty or white space.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrBlank(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }

        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Get initials from a given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetInitials(string name)
        {
            string[] nameSplit = name.Split(new string[] { ",", ".", " " }, StringSplitOptions.RemoveEmptyEntries);

            string initials = "";

            foreach (string item in nameSplit)
            {
                initials += item.Substring(0, 1).ToUpper();
            }

            return initials.ToUpperInvariant();
        }

        public static string Encrypt(this string plainText)
        {
            if (plainText.IsNullOrBlank())
            {
                return plainText;
            }

            byte[] clearBytes = Encoding.Unicode.GetBytes(plainText);
            plainText = Convert.ToBase64String(clearBytes);
            plainText = encryptionKey1 + plainText + encryptionKey2;

            return plainText;
        }

        public static string Decrypt(this string encodedData)
        {
            if (encodedData.IsNullOrBlank())
            {
                return encodedData;
            }

            encodedData = encodedData.Replace(encryptionKey1, "").Replace(encryptionKey2, "");
            byte[] cipherBytes = Convert.FromBase64String(encodedData);
            encodedData = Encoding.Unicode.GetString(cipherBytes);

            return encodedData;
        }
    }
}
