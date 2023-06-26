using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ProjetPizzaGroupe6
{
    public class Invoice
    {
        public List<InvoiceItem> Items { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
