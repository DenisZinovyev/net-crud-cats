namespace Microsoft.Extensions.DependencyInjection
{
    using DSR.CrudCats.Crypto;

    public static class CryptoExtensions
    {
        public static void AddCrypto(this IServiceCollection services)
        {
            services.AddSingleton<ICryptoHelper, CryptoHelper>();
        }
    }
}