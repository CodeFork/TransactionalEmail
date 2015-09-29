using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Conditions;
using Conditions.Guards;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Factories;
using TransactionalEmail.Models;

namespace TransactionalEmail.Controllers
{
    [RoutePrefix("api/mail")]
    public class EmailController : ApiController
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            Check.If(emailService).IsNotNull();

            _emailService = emailService;
        }

        [HttpGet, Route("fetch/{numberOfEmailsToRetrieve:int}")]
        public List<Email> GetEmails(int numberOfEmailsToRetrieve)
        {
            Check.If(numberOfEmailsToRetrieve).IsGreaterThan(0);

            return _emailService.RetrieveMessages(numberOfEmailsToRetrieve).Select(EmailFactory.CreateEmailModel).ToList();
        }

        [HttpGet, Route("{emailReference:length(1,10)}", Name = "GetEmail")]
        public Email GetEmail(string emailReference)
        {
            Check.If(emailReference).IsNotNull();

            return EmailFactory.CreateEmailModel(_emailService.GetEmail(emailReference));
        }

        [HttpPost, Route("")]
        public HttpResponseMessage Send(Email email)
        {
            Check.If(email).IsNotNull();

            var result = _emailService.Send(EmailFactory.CreateCoreEmail(email));

            if (result.IsNullOrEmpty())
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError };
            }

            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };

            response.Headers.Location = new Uri(Url.Link("GetEmail", new { emailReference = result }));

            return response;
        }

        [HttpPut, Route("")]
        public HttpResponseMessage NotifyRetrievalResult(RetrievalResult retrievalResult)
        {
            Check.If(retrievalResult).IsNotNull();

            var result = _emailService.NotifyRetrievalResult(retrievalResult.EmailReference, retrievalResult.RetrievedOk);

            return result
                ? new HttpResponseMessage { StatusCode = HttpStatusCode.OK}
                : new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest};
        }
    }
}
