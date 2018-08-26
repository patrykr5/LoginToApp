using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LoginToApp
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        public string PasswordAuth;
        public string LoginFromInput;

        private void Authentication()
        {
            string connectionString = @"Data Source="; //Data Source=%server/local_name_pc%;Initial Catalog=%DB name%;User ID=%login%;Password=%password%
            using (var conn = new SqlConnection(connectionString))
            {
                var sql = "EXEC usp_Auth @Login";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Login", LoginFromInput);
                    conn.Open();
                    PasswordAuth = (string)cmd.ExecuteScalar();
                }
                conn.Close();
            }
        }

        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tb_Login.Text) && !String.IsNullOrWhiteSpace(pb_Password.Password))
            {                
                LoginFromInput = tb_Login.Text;
                Authentication();
                string passwordInput = CreateMD5(pb_Password.Password.ToString());
                if (PasswordAuth == passwordInput)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid login/password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                MessageBox.Show("Enter your login/password on correctly box.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
