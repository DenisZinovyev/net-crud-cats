namespace DSR.Cats.Server.ApiDto.Responses
{
    public class TokenResponse
    {
        public string AuthToken { get; set; }

        public long ExpiresIn { get; set; }
    }
}
