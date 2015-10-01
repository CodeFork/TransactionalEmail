using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Conditions.Guards;
using Google.Apis.Auth.OAuth2;
using Limilabs.Client.Authentication.Google;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Infrastructure.Gmail.Interfaces;

namespace TransactionalEmail.Infrastructure.Gmail
{
    public class OAuth2Authenticator : IOAuth2Authenticator
    {
        private readonly IGmailSettings _gmailSettings;
        private readonly ICertificatePath _certificatePath;

        public OAuth2Authenticator(IGmailSettings gmailSettings, ICertificatePath certificatePath)
        {
            Check.If(gmailSettings).IsNotNull();
            Check.If(certificatePath).IsNotNull();

            _gmailSettings = gmailSettings;
            _certificatePath = certificatePath;
        }

        public string GetOAuth2AccessToken(string emailAddress)
        {
            var certificate = new X509Certificate2(_certificatePath.Value,
                _gmailSettings.ServiceAccountCertPassword, X509KeyStorageFlags.MachineKeySet |
                                                           X509KeyStorageFlags.PersistKeySet |
                                                           X509KeyStorageFlags.Exportable);

            var credential = new ServiceAccountCredential(

                new ServiceAccountCredential.Initializer(_gmailSettings.ServiceAccountEmailAddress)
                {
                    Scopes = new[] {GoogleScope.ImapAndSmtp.Name},
                    User = emailAddress
                }.FromCertificate(certificate));

            var success = credential.RequestAccessTokenAsync(CancellationToken.None).Result;

            return success ? credential.Token.AccessToken : string.Empty;
        }
    }
}