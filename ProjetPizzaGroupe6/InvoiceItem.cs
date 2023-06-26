namespace ProjetPizzaGroupe6
{
    public class InvoiceItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<string> Ingredients { get; set; }
    }
}
