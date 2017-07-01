using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.iOS.Extensions;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.Extensions;

namespace NPAInspectionWriter.Services
{
    public class PrintService : IPrintService
    {
        InspectionWriterClient client { get; }
        public PrintService( InspectionWriterClient inspectionWriterClient )
        {
            client = inspectionWriterClient;
        }

        public async Task<IEnumerable<Printer>> GetPrintersAsync( CRWriterUser npaUser ) =>
            await client.GetAsync<IEnumerable<Printer>>( $"print/{npaUser.LocationId}/printersAtLocation" );

        public async Task<HttpResponseMessage> PrintLabelAsync( ReportRequest request ) =>
            await client.PostJsonObjectAsync( "print/printLabel", request );

        public async Task<IEnumerable<PrinterModel>> GetPrinterModelAsync()
        {
            var user = await AppRepository.Instance.GetCurrentUserAsync();

            var printerList = await GetPrintersAsync(user);
            var printerModels = new List<PrinterModel>();

            foreach (var item in printerList)
            {
                printerModels.Add(new PrinterModel() { PrinterId = item.PrinterId, PrinterName = item.PrinterName });
            }

            return printerModels;
        }
    }
}
