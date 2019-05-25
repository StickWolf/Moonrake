namespace BaseClientServerDtos.ToClient
{
    public class CreateAccountDto : FiniteDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
