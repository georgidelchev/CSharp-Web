using System;
using System.Globalization;

namespace Panda.ViewModels.Receipts
{
    public class GetAllReceiptsViewModel
    {
        public string Id { get; set; }

        public decimal Fee { get; set; }

        public DateTime IssuedOn { get; set; }

        public string IssuedOnAsString
            => this.IssuedOn.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

        public string Recipient { get; set; }
    }
}
