namespace DSR.CrudCats.Auth
{
    class AuthConfiguration
    {
        public int JwtLifetime { get; set; }
        public string JwtIssuerSigningKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
    }
}
