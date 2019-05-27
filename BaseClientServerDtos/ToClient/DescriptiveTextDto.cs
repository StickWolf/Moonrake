namespace BaseClientServerDtos.ToClient
{
    public class DescriptiveTextDto : FiniteDto
    {
        public DescriptiveTextDto(string text)
        {
            this.Text = text;
        }

        public string Text { get; set; }
    }
}
