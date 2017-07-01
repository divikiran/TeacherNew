using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using InspectionWriterWebApi.Utilities;

namespace InspectionWriterWebApi.Filters
{
    public class ValidateHttpAntiForgeryTokenAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var req = actionContext.ControllerContext.Request;

            try
            {
                if (IsAjaxRequest(req))
                {
                    ValidateRequestHeader(req);
                }
                else
                {
                    AntiForgery.Validate();
                }
            }
            catch (HttpAntiForgeryException e)
            {
                actionContext.Response = req.CreateErrorResponse(HttpStatusCode.Forbidden, e);
            }
        }

        private void ValidateRequestHeader(HttpRequestMessage req)
        {
            var cookieToken = string.Empty;
            var formToken = string.Empty;
            IEnumerable<string> tokenHeaders;

            if (req.Headers.TryGetValues("RequestVerificationToken", out tokenHeaders))
            {
                var tokenValue = tokenHeaders.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(tokenValue))
                {
                    var tokens = tokenValue.Split(':');

                    if (tokens.Length == 2)
                    {
                        cookieToken = tokens[0].Trim();
                        formToken = tokens[1].Trim();
                    }
                }
            }

            AntiForgery.Validate(cookieToken, formToken);
        }

        private bool IsAjaxRequest(HttpRequestMessage req)
        {
            IEnumerable<string> requestedWithHeaders;

            if (req.Headers.TryGetValues("X-Requested-With", out requestedWithHeaders))
            {
                var headerValue = requestedWithHeaders.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(headerValue))
                {
                    return string.Equals(headerValue, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
                }
            }

            return false;
        }
    }
}