using System;
using System.Security.Cryptography;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latian_LKS_2
{

    class EmployeeModel
    {
        public static int ID { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string Name { get; set; }
        public static string Email { get; set; }
        public static string Address { get; set; }
        public static DateTime Birth { get; set; }
        public static int JobID { get; set; }
        public static string Photo { get; set; }
        public static byte[] PhotoImage { get; set; }
    }

    class ColourModel
    {
        public static Color primary = Color.FromArgb(130, 5, 30);
        public static Color glass = Color.FromArgb(30, Color.White);
    }

    class EncryptModel
    {
        public string encrypt(string text)
        {
            using ( SHA256Managed manage = new SHA256Managed() )
            {
                byte[] encode = Encoding.UTF8.GetBytes(text);
                var encryption = manage.ComputeHash(encode);
                string result = Convert.ToBase64String(encryption);

                return result;
            }
        }
    }
}
