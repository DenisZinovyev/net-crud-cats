namespace DSR.CrudCats.Auth.CreateTokenResponse
{
    public class Token
    {
        public string Value { get; set; }
        public Token(string value) => Value = value;
    }
}