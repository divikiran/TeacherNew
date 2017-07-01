using System;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using InspectionWriterWebApi.Models;
using NPA.Common;

namespace InspectionWriterWebApi.Controllers
{
    [Authorize]
    [RoutePrefix("message")]
    public class MessageController : ApiController
    {
        [HttpPost]
        [Route("email")]
        public async Task<IHttpActionResult> SendEmail([FromBody]Message message)
        {
            try
            {
                await Task.Run(() => SendEmailFromApp(message));

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [NonAction]
        private static void SendEmailFromApp(Message message)
        {
            Email.SendBasicEmail(message.To, message.Subject, message.Body, message.From, 1, 0);
        }

    }
}