using ProjetPizzaGroupe6;

public class Program
{
    public static void Main(string[] args)
    {
        var orderProcessor = new OrderProcessor();

        while (true)
        {
            Console.WriteLine("Entrez une commande (ou 'exit' pour quitter) :");
            var input = Console.ReadLine();

            if (input.ToLower() == "exit")
                break;

            orderProcessor.ProcessOrder(input);
        }
    }
}