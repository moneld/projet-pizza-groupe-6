using ProjetPizzaGroupe6;

public class Program
{
    public static void Main(string[] args)
    {
        var orderProcessor = new OrderProcessor();

        Console.WriteLine("Veuillez entrer votre commande :");
        var order = Console.ReadLine();

        orderProcessor.ProcessOrder(order);

        Console.ReadLine();
    }
}