using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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

        [HttpGet, Route("")]
        public List<Email> GetEmails(int numberOfEmailsToRetrieve)
        {
            Check.If(numberOfEmailsToRetrieve).IsGreaterThan(0);

            return _emailService.RetrieveMessages(numberOfEmailsToRetrieve).Select(EmailFactory.CreateEmailModel).ToList();
        }

        [HttpPost, Route("")]
        public HttpResponseMessage Send(Email email)
        {
            Check.If(email).IsNotNull();

            var result = _emailService.Send(EmailFactory.CreateCoreEmail(email));

            return result
                ? new HttpResponseMessage {StatusCode = HttpStatusCode.OK}
                : new HttpResponseMessage {StatusCode = HttpStatusCode.InternalServerError};
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
