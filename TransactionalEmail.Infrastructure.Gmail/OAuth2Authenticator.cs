using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Conditions.Guards;
using Google.Apis.Auth.OAuth2;
using Limilabs.Client.Authentication.Google;
using TransactionalEmail.Infrastructure.Gmail.Interfaces;

namespace TransactionalEmail.Infrastructure.Gmail
{
    public class OAuth2Authenticator : IOAuth2Authenticator
    {
        private readonly IGmailSettings _gmailSettings;

        public OAuth2Authenticator(IGmailSettings gmailSettings)
        {
            Check.If(gmailSettings).IsNotNull();

            _gmailSettings = gmailSettings;
        }

        public string GetOAuth2AccessToken(string emailAddress)
        {
            var certificate = new X509Certificate2(_gmailSettings.ServiceAccountCertPath,
                _gmailSettings.ServiceAccountCertPassword, X509KeyStorageFlags.Exportable);

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