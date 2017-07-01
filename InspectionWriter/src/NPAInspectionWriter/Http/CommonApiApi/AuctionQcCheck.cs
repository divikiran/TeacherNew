using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPAInspectionWriter.Logging;

namespace NPA.XF.Http.CommonApiApi
{
    public class AuctionQcCheck
    {
        private CommonApiClient _client { get; }
        private ILog _logger { get; }

        internal AuctionQcCheck( CommonApiClient client )
        {
            _client = client;
            _logger = _client._options.Logger;
        }
    }
}
