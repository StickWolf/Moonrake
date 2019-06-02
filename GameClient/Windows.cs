namespace GameClient
{
    public static class Windows
    {
        public static MainWindow Main { get; set; }

        public static void WriteMainWindowDescriptiveText(string text)
        {
            Main.txtGameText.Dispatcher.Invoke(() => {
                var textbox = Main.txtGameText;
                textbox.AppendText(text);
                textbox.CaretIndex = textbox.Text.Length;
                textbox.ScrollToEnd();
            });
        }
    }
}
