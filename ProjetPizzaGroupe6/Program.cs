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
            Console.WriteLine("Comment souhaitez vous renseigner votre commande (ou 'exit' pour quitter) :");
            Console.WriteLine("Saisissez '1' pour saisir la commande");
            Console.WriteLine("Saisissez '2' pour charger le fichier des commandes");
            var input = Console.ReadLine();

            if (input.ToLower() == "exit")
                break;

            if (input.ToLower() == "1")
            {
                Console.WriteLine("Entrez une commande (ou 'exit' pour quitter) :");
                input = Console.ReadLine();
                orderProcessor.ProcessOrder(input);
                if (input.ToLower() == "exit")
                    break;
            }

            if (input.ToLower() == "2")
            {
                Console.WriteLine("Entrez le chemin du fichier contenant les commandes :");
                var filePath = Console.ReadLine();//C:\Users\winnie\Documents\ETUDES\LAB\MASTER 1\Semestre 2\Design patterns\projet\json.json
                orderProcessor.VerifyFilePath(filePath);//2 Regina, 1 4 Saisons, 3 Végétarienne

                if (filePath.ToLower() == "exit")
                    break;
            }
        }
    }
}