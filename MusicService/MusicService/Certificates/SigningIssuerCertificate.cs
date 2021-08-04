using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Security.Cryptography;

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

        public RsaSecurityKey GetIssuerSigningKey()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var publicKeyXml = File.ReadAllText(Path.Combine(path, _config["Jwt:rsaPublicKeyXml"]));
            _rsa.FromXmlString(publicKeyXml);

            return new RsaSecurityKey(_rsa);
        }

        public void Dispose()
        {
            _rsa?.Dispose();
        }
    }
}
