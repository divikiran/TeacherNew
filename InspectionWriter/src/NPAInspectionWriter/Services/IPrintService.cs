using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NPAInspectionWriter.Models;

namespace NPAInspectionWriter.Services
{
    public interface IPrintService
    {
        Task<IEnumerable<Printer>> GetPrintersAsync( CRWriterUser npaUser );

        Task<HttpResponseMessage> PrintLabelAsync( ReportRequest request );

        Task<IEnumerable<PrinterModel>> GetPrinterModelAsync();
    }
}
