namespace DSR.Cats.Server.Services.Models
{
    public class AuthConfiguration
    {
        public int TokenLifetimeMinutes { get; set; }
        public byte[] IssuerSigningKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
