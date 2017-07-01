#if USE_MOCKS
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NPAInspectionWriter.Models;
using Newtonsoft.Json;
using System;

namespace NPAInspectionWriter.Services
{
#pragma warning disable 1998
    public class PrintServiceMock : IPrintService
    {
        public Task<IEnumerable<PrinterModel>> GetPrinterModelAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Printer>> GetPrintersAsync( AmsUser amsUser )
        {
            string response = "[{\"printerId\":\"97a37d2e-bbf7-dd11-aca2-0019b9b35da2\",\"printerName\":\"SD - SATO_SANWH1\"},{\"printerId\":\"d56cad68-dd6e-4422-b603-57478802cd29\",\"printerName\":\"SD - SATO_SANWH2\"},{\"printerId\":\"cb8571d1-a14d-49a7-a99a-ac87b3aa5f76\",\"printerName\":\"SD VehLabel - QL420-1\"}]";
            return JsonConvert.DeserializeObject<IEnumerable<Printer>>( response );
        }

        public Task<IEnumerable<Printer>> GetPrintersAsync(CRWriterUser npaUser)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> PrintLabelAsync( ReportRequest request )
        {
            return new HttpResponseMessage( HttpStatusCode.OK );
        }
    }
}
#endif