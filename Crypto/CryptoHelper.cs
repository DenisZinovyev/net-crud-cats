namespace DSR.CrudCats.Crypto
{
    using System.Security.Cryptography;

    public interface ICryptoHelper
    {
        (byte[], byte[]) HashPassword(string password);
        byte[] HashPassword(string password, byte[] salt);
    }

    public class CryptoHelper : ICryptoHelper
    {
        const int PWD_HASH_SALT_SIZE = 128;
        const int PWD_HASH_ITERATIONS = 10000;

        public (byte[], byte[]) HashPassword(string password)
        {
            byte[] hash;
            byte[] salt;

            using (var hasher = new Rfc2898DeriveBytes(password, PWD_HASH_SALT_SIZE, PWD_HASH_ITERATIONS))
            {

                hash = hasher.GetBytes(128);
                salt = hasher.Salt;
            }

            return (hash, salt);
        }

        public byte[] HashPassword(string password, byte[] salt)
        {
            byte[] hash;

            using (var hasher = new Rfc2898DeriveBytes(password, salt, PWD_HASH_ITERATIONS))
            {
                hash = hasher.GetBytes(128);
            }

            return hash;
        }
    }
}