using BaseClientServerDtos.ToClient;
using System;
using System.Collections.Generic;
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

namespace GameClient
{
    /// <summary>
    /// Interaction logic for CreateAccountDialog.xaml
    /// </summary>
    public partial class CreateAccountDialog : Window
    {
        public CreateAccountDialog()
        {
            InitializeComponent();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Text != txtPassword2.Text)
            {
                // TODO: show why we're returning (because the passwords don't match)
                return;
            }

            var dto = new CreateAccountDto();
            dto.UserName = txtUserName.Text;
            dto.Password = txtPassword.Text; // TODO: hash/encrypt
            // ServerConnection.SendDtoMessage(dto); // TODO:
        }
    }
}
