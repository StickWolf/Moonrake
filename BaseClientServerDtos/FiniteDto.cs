namespace BaseClientServerDtos
{
    public class FiniteDto
    {
        public string DtoName { get; set; }

        public FiniteDto()
        {
            DtoName = this.GetType().Name;
        }
    }
}
