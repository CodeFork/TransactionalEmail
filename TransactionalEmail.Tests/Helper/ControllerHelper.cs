using System;
using System.Net.Http;
using System.Web.Http;
using TransactionalEmail.Controllers;
using TransactionalEmail.Core.Interfaces;

namespace TransactionalEmail.Tests.Helper
{
    public static class ControllerHelper
    {
        public static EmailController GetInitialisedEmailController(IEmailService emailService)
        {
            var controller = new EmailController(emailService)
            {
                Request = new HttpRequestMessage { RequestUri = new Uri("http://localhost/api/") },
                Configuration = new HttpConfiguration()
            };

            controller.Configuration.MapHttpAttributeRoutes();
            controller.Configuration.EnsureInitialized();

            return controller;
        }
    }
}
