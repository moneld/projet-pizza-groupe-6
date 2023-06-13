using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPizzaGroupe6
{
    public class OrderProcessor
    {
        private Dictionary<string, PizzaComponent> _pizzas;

        public OrderProcessor()
        {
            _pizzas = new Dictionary<string, PizzaComponent>();

            // Création des pizzas disponibles
            var reginaPizza = new Pizza("Regina");
            reginaPizza.AddComponent(new Ingredient("Tomate", 150, new Dictionary<string, int>()));
            reginaPizza.AddComponent(new Ingredient("Mozzarella", 125, new Dictionary<string, int>()));
            reginaPizza.AddComponent(new Ingredient("Fromage râpé", 100, new Dictionary<string, int>()));
            reginaPizza.AddComponent(new Ingredient("Jambon", 2, new Dictionary<string, int>()));
            reginaPizza.AddComponent(new Ingredient("Champignons frais", 4, new Dictionary<string, int>()));
            reginaPizza.AddComponent(new Ingredient("Huile d'olive", 2, new Dictionary<string, int>()));

            var quatreSaisonsPizza = new Pizza("4 Saisons");
            quatreSaisonsPizza.AddComponent(new Ingredient("Tomate", 150, new Dictionary<string, int>()));
            quatreSaisonsPizza.AddComponent(new Ingredient("Mozzarella", 125, new Dictionary<string, int>()));
            quatreSaisonsPizza.AddComponent(new Ingredient("Jambon", 2, new Dictionary<string, int>()));
            quatreSaisonsPizza.AddComponent(new Ingredient("Champignons frais", 100, new Dictionary<string, int>()));
            quatreSaisonsPizza.AddComponent(new Ingredient("Poivron", 0.5m, new Dictionary<string, int>()));
            quatreSaisonsPizza.AddComponent(new Ingredient("Olives", 1, new Dictionary<string, int>()));

            var vegetariennePizza = new Pizza("Végétarienne");
            vegetariennePizza.AddComponent(new Ingredient("Tomate", 150, new Dictionary<string, int>()));
            vegetariennePizza.AddComponent(new Ingredient("Mozzarella", 100, new Dictionary<string, int>()));
            vegetariennePizza.AddComponent(new Ingredient("Courgette", 0.5m, new Dictionary<string, int>()));
            vegetariennePizza.AddComponent(new Ingredient("Poivron jaune", 1, new Dictionary<string, int>()));
            vegetariennePizza.AddComponent(new Ingredient("Tomates cerises", 6, new Dictionary<string, int>()));
            vegetariennePizza.AddComponent(new Ingredient("Olives", 1, new Dictionary<string, int>()));

            _pizzas.Add("Regina", reginaPizza);
            _pizzas.Add("4 Saisons", quatreSaisonsPizza);
            _pizzas.Add("Végétarienne", vegetariennePizza);
        }

        public void ProcessOrder(string order)
        {
            // Analyser la commande
            var pizzaQuantities = new Dictionary<string, int>();
            var pizzaDetails = order.Split(',');

            foreach (var pizzaDetail in pizzaDetails)
            {
                var pizzaInfo = pizzaDetail.Trim().Split(' ');

                if (pizzaInfo.Length < 2)
                {
                    Console.WriteLine($"Commande incorrecte : '{pizzaDetail}'.\n");
                    continue;
                }

                var quantityStr = pizzaInfo[0];
                var pizzaName = string.Join(" ", pizzaInfo.Skip(1));

                if (!int.TryParse(quantityStr, out var quantity))
                {
                    Console.WriteLine($"Quantité incorrecte : '{quantityStr}'.\n");
                    continue;
                }

                if (_pizzas.ContainsKey(pizzaName))
                {
                    if (pizzaQuantities.ContainsKey(pizzaName))
                    {
                        pizzaQuantities[pizzaName] += quantity;
                    }
                    else
                    {
                        pizzaQuantities.Add(pizzaName, quantity);
                    }
                }
                else
                {
                    Console.WriteLine($"La pizza '{pizzaName}' n'est pas disponible.\n");
                }
            }

            // Créer la commande composite
            var orderComposite = new Pizza("Commande :");

            foreach (var pizzaQuantity in pizzaQuantities)
            {
                var pizzaName = pizzaQuantity.Key;
                var quantity = pizzaQuantity.Value;
                if (_pizzas.ContainsKey(pizzaName))
                {
                    var pizza = _pizzas[pizzaName];

                    for (int i = 0; i < quantity; i++)
                    {
                        orderComposite.AddComponent(pizza);
                    }
                }
                else
                {
                    Console.WriteLine($"La pizza '{pizzaName}' n'est pas disponible.");
                }
            }


            // Afficher la facture
            Console.WriteLine("\n##################################### ");
            Console.WriteLine("Facture :");
            Console.WriteLine("#####################################");
            orderComposite.Display(0);
            decimal totalPrice = orderComposite.GetPrice();
            Console.WriteLine("\n-------------------------------------");
            Console.WriteLine($"Prix total : {totalPrice} €");
            Console.WriteLine("-------------------------------------\n");
            // Afficher les instructions de préparation
            DisplayPreparationInstructions(orderComposite);

            // Afficher les ingrédients utilisés
            
            DisplayIngredientUsage(orderComposite);

        }

        private void DisplayPreparationInstructions(PizzaComponent component)
        {
            if (component is Pizza pizza)
            {
                Console.WriteLine("\n#####################################");
                pizza.Display(1);
                Console.WriteLine("Instructions de préparation :");
                Console.WriteLine("#####################################");
                Console.WriteLine("Préparer la pâte");

                foreach (var subComponent in pizza.GetIngredients())
                {
                    Console.WriteLine($"Ajouter {subComponent.Key}");
                }

                Console.WriteLine("Cuire la pizza");
            }
        }

        private void DisplayIngredientUsage(PizzaComponent component)
        {
            if (component is Pizza pizza)
            {
                Console.WriteLine("\n#####################################");
                Console.WriteLine(pizza.Name);
                pizza.Display(1);
                Console.WriteLine("Ingrédients utilisés :");//1 4 Saisons, 1 Végétarienne
                Console.WriteLine("\n#####################################");
                var ingredients = pizza.GetIngredients();

                foreach (var ingredient in ingredients)
                {
                    Console.WriteLine($"{ingredient.Key} : {ingredient.Value}");

                   /* foreach (var subComponent in pizza.GetIngredients())
                    {
                        Console.WriteLine($"\t {subComponent.Key} : {subComponent.Value}");
                    }*/
                }
            }
        }
    }

}
