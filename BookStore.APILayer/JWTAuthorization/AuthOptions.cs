using BookStore.API.Layer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookStore.APILayer.JWTAuthorization
{
    public class AuthOptions
    {
        public static string _issuer; // издатель токена
        public static string _audience; // потребитель токена
        private static string _key;   // ключ для шифрации
        public static int _lifetime; // время жизни токена - 1 минута
        private readonly MyOptions _options;

        public AuthOptions(IOptionsMonitor<MyOptions> optionsAccessor)
        {
            _options = optionsAccessor.CurrentValue;

            _issuer = _options.Issuer;
            _audience = _options.Audience;
            _key = _options.Key;
            _lifetime = _options.LifeTime;
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
        }
    }
}
