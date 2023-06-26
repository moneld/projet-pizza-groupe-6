using ProjetPizzaGroupe6;

public class Program
{
    public static void Main(string[] args)
    {
        var orderProcessor = new OrderProcessor();
        Console.WriteLine("\n#####################################");
        Console.WriteLine("Pizzas disponibles :");
        Console.WriteLine("#####################################");
        orderProcessor.DisplayAvailablePizzas();

        while (true)
        {
            Console.WriteLine("\n#####################################\n");
            Console.WriteLine("Comment souhaitez-vous renseigner votre commande (ou 'exit' pour quitter) :");
            Console.WriteLine("Saisissez '1' pour saisir la commande");
            Console.WriteLine("Saisissez '2' pour charger le fichier des commandes");

            var input = Console.ReadLine().ToLower();

            if (input == "exit")
                break;

            switch (input)
            {
                case "1":
                    Console.WriteLine("Entrez une commande (ou 'exit' pour quitter) :");
                    input = Console.ReadLine();
                    orderProcessor.ProcessOrder(input);
                    break;

                case "2":
                    Console.WriteLine("Entrez le chemin du fichier contenant les commandes :");
                    var filePath = Console.ReadLine();
                    orderProcessor.VerifyFilePath(filePath);
                    break;

                default:
                    Console.WriteLine("Choix invalide. Veuillez réessayer.");
                    break;
            }
        }
    }

}