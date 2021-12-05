namespace ToDoAPI.Uilities.Security.Token
{
    public class CustomTokenOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }

        public int AccessTokenExpiration { get; set; }

        public string SecurityKey { get; set; }

    }
}
