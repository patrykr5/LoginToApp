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
            string connectionString = @"Data Source=";
            using (var conn = new SqlConnection(connectionString))
            {
                var sql = "SELECT Password FROM Loginapp_users where Login = @Login";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Login", LoginFromInput);
                    conn.Open();
                    PasswordAuth = (string)cmd.ExecuteScalar();
                }
                conn.Close();
            }
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tb_Login.Text) && !String.IsNullOrWhiteSpace(pb_Password.Password))
            {
                LoginFromInput = tb_Login.Text;
                Authentication();
                if (PasswordAuth == pb_Password.Password.ToString())
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
