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

        private bool ProcessUserInput(string input)
        {
            bool processed = false;
            if (input.StartsWith("/"))
            {
                var extraWords = new List<string>(input.Split(' '));
                var word = extraWords[0];
                word = word.Substring(1);
                extraWords.RemoveAt(0);
                processed = ClientCommandHelper.TryRunClientCommand(word, extraWords);
            }

            return processed;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.Main = this;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ServerConnection.Disconnect("Client shutdown");
        }
    }
}
