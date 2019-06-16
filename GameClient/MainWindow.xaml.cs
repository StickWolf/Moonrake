using GameClient.Commands;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TxtUserInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            var typedText = txtUserInput.Text;
            txtUserInput.Text = string.Empty;
            ProcessUserInput(typedText);
        }

        private void ProcessUserInput(string input)
        {
            if (input.StartsWith("/"))
            {
                var extraWords = new List<string>(input.Split(' '));
                var word = extraWords[0];
                word = word.Substring(1);
                extraWords.RemoveAt(0);
                CommandHelper.TryRunCommand(word, extraWords);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.Main = this;
            Windows.WriteMainWindowDescriptiveText(
                "TODO: Write UI elements to represent these commands.\r\n" +
                "For now follow this CLI syntax to start.\r\n" +
                "Keep in mind each new account and player will need a different name.\r\n\r\n" +
                "/connect\r\n" +
                "/createnewaccount testuser abc123\r\n" +
                "/login testuser abc123\r\n" +
                "/createnewplayer Jack-o-Lantern\r\n" +
                "/useplayer Jack-o-Lantern\r\n\r\n"
                );
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ServerConnection.Disconnect("Client shutdown");
        }
    }
}
