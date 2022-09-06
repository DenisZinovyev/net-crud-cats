namespace DSR.CrudCats.Users
{
    using System.ComponentModel.DataAnnotations;
    using DSR.CrudCats.Persistence;

    [Persistent]
    public class Credentials
    {
        public int Id { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        private Credentials() { }

        public Credentials(byte[] passwordHash, byte[] passwordSalt) =>
            (PasswordHash, PasswordSalt) = (passwordHash, passwordSalt);
    }
}
