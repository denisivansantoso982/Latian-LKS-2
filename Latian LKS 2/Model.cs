using System;
using System.Security.Cryptography;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

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
        //public static Color primary = Color.FromArgb(130, 5, 30);
        //public static Color secondary = Color.FromArgb(181, 15, 30);

        // Flutter Theme
        //public static Color primary = Color.FromArgb(4, 84, 164);
        //public static Color secondary = Color.FromArgb(4, 124, 212);

        // Assassin's Creed IV Theme
        public static Color primary = Color.FromArgb(12, 44, 36);
        public static Color secondary = Color.FromArgb(37, 82, 78);

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

    class GradientModel
    {
        public static void gradientColor(Rectangle rectangle, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(rectangle, ColourModel.primary, ColourModel.secondary, 45F))
            {
                e.Graphics.FillRectangle(brush, rectangle);
            }
        }
    }
}
