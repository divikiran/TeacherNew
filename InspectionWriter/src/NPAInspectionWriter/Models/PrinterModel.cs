using System;
using NPAInspectionWriter.DTO;

namespace NPAInspectionWriter.Models
{
    public class PrinterModel : BaseDTO
    {
        Guid _printerId;

        public Guid PrinterId
        {
            get
            {
                return _printerId;
            }

            set
            {
                _printerId = value;
                OnPropertyChanged(nameof(PrinterId));
            }
        }
        string _printerName;

        public string PrinterName
        {
            get
            {
                return _printerName;
            }

            set
            {
                _printerName = value;
                OnPropertyChanged(nameof(PrinterName));
            }
        }
    }
}
