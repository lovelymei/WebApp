using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AuthorizationService.Certificates
{
    public class SigningIssuerCertificate : IDisposable
    {
        private readonly IConfiguration _config;
        private readonly RSA _rsa;

        public SigningIssuerCertificate(IConfiguration config)
        {
            _config = config;
            _rsa = RSA.Create();
        }

        public async Task<RsaSecurityKey> GetIssuerSigningKey()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var fullPath = Path.Combine(path, _config["Jwt:rsaPublicKeyXml"]);
            var publicKeyXml = await File.ReadAllTextAsync(Path.Combine(path, _config["Jwt:rsaPublicKeyXml"]));
            _rsa.FromXmlString(publicKeyXml);

            return new RsaSecurityKey(_rsa);
        }

        public void Dispose()
        {
            _rsa?.Dispose();
        }
    }
}
