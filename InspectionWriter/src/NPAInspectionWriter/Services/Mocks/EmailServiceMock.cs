#if USE_MOCKS
using NPAInspectionWriter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPAInspectionWriter.Services
{
#pragma warning disable 1998
    public class EmailServiceMock : IEmailService
    {
        public async Task<bool> SendEmailAsync( Message message )
        {
            return true;
        }
    }
}
#endif