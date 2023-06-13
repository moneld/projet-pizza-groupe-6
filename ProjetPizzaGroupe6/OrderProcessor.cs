using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPizzaGroupe6
{
    public class OrderProcessor
    {
        private Dictionary<string, Pizza> _pizzas;

        public OrderProcessor()
        {
            _pizzas = new Dictionary<string, Pizza>();

            // Initialize available pizzas
            var reginaPizza = new Pizza("Regina", 8);
            reginaPizza.AddIngredient("tomate", 150);
            reginaPizza.AddIngredient("mozzarella", 125);
            reginaPizza.AddIngredient("fromage râpé", 100);
            reginaPizza.AddIngredient("jambon", 2);
            reginaPizza.AddIngredient("champignons frais", 4);
            reginaPizza.AddIngredient("huile d'olive", 2);
            _pizzas.Add(reginaPizza.Name, reginaPizza);

            var quatreSaisonsPizza = new Pizza("4 Saisons", 9);
            quatreSaisonsPizza.AddIngredient("tomate", 150);
            quatreSaisonsPizza.AddIngredient("mozzarella", 125);
            quatreSaisonsPizza.AddIngredient("jambon", 2);
            quatreSaisonsPizza.AddIngredient("champignons frais", 100);
            quatreSaisonsPizza.AddIngredient("poivron", 5);
            quatreSaisonsPizza.AddIngredient("olives", 1);
            _pizzas.Add(quatreSaisonsPizza.Name, quatreSaisonsPizza);

            var vegetariennePizza = new Pizza("Végétarienne", 7.50m);
            vegetariennePizza.AddIngredient("tomate", 150);
            vegetariennePizza.AddIngredient("mozzarella", 100);
            vegetariennePizza.AddIngredient("courgette", 5);
            vegetariennePizza.AddIngredient("poivron jaune", 1);
            vegetariennePizza.AddIngredient("tomates cerises", 6);
            vegetariennePizza.AddIngredient("olives", 0); //1 4 Saisons, 1 Regina
            _pizzas.Add(vegetariennePizza.Name, vegetariennePizza);
        }

        public void ProcessOrder(string input)
        {
            var order = ParseOrder(input);
            if (order != null)
            {
                Console.WriteLine("\n#####################################");
                Console.WriteLine("Facture :");
                Console.WriteLine("#####################################");
                order.Display(0);
                Console.WriteLine("\n#####################################");
                Console.WriteLine("Instructions de préparation :");
                Console.WriteLine("#####################################");
                foreach (var component in order.GetComponents())
                {
                    component.Display(0);
                    Console.WriteLine("Cuire la pizza\n");
                }

                Console.WriteLine("\n#####################################");
                Console.WriteLine("Liste des ingrédients utilisés :");
                Console.WriteLine("#####################################");
                var ingredients = GetIngredientsFromOrder(order);
                foreach (var ingredientEntry in ingredients)
                {
                    var ingredientName = ingredientEntry.Key;
                    var totalQuantity = ingredientEntry.Value.Sum(t => t.Item2);
                    Console.WriteLine($"{ingredientName} : {totalQuantity}");
                    foreach (var (pizza, quantity) in ingredientEntry.Value)
                    {
                        Console.WriteLine($"- {pizza.Name} : {quantity}");
                    }
                    Console.WriteLine();
                }
            }
        }

        private OrderComposite ParseOrder(string input)
        {
            var order = new OrderComposite();
            var pizzas = input.Split(',');

            foreach (var pizzaStr in pizzas)
            {
                var pizzaInfo = pizzaStr.Trim().Split(' ');

                if (pizzaInfo.Length < 2)
                {
                    Console.WriteLine($"Commande incorrecte : '{pizzaStr}'.\n");
                    order = null;
                    continue;
                }

                var quantityStr = pizzaInfo[0];
                var pizzaName = string.Join(" ", pizzaInfo.Skip(1));

                if (!int.TryParse(quantityStr, out var quantity))
                {
                    Console.WriteLine($"Quantité incorrecte : '{quantityStr}'.\n");
                    order = null;
                    continue;
                }

                if (int.TryParse(pizzaInfo[0], out var pizzaQt))
                {
                    if (pizzaQt < 0)
                    {
                        Console.WriteLine("Invalid quantity: Quantity cannot be negative.");
                        order = null;
                        continue;
                    }
                }

                if (_pizzas.ContainsKey(pizzaName))
                {
                    var pizza = _pizzas[pizzaName];
                    if (int.TryParse(pizzaInfo[0], out var pizzaQuantity))
                    {
                        for (var i = 0; i < pizzaQuantity; i++)
                        {
                            order.AddComponent(pizza);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"La pizza '{pizzaName}' n'est pas disponible.\n");
                    order = null;
                    continue;
                }

            }
            return order;
        }

        private Dictionary<string, List<(Pizza, int)>> GetIngredientsFromOrder(OrderComposite order)
        {
            var ingredients = new Dictionary<string, List<(Pizza, int)>>();

            foreach (var component in order.GetComponents())
            {
                if (component is Pizza pizza)
                {
                    foreach (var ingredient in pizza.GetIngredients())
                    {
                        if (ingredients.ContainsKey(ingredient.Name))
                        {
                            var pizzaIngredientList = ingredients[ingredient.Name];
                            bool foundPizza = false;

                            for (int i = 0; i < pizzaIngredientList.Count; i++)
                            {
                                var pizzaIngredientTuple = pizzaIngredientList[i];

                                if (pizzaIngredientTuple.Item1 == pizza)
                                {
                                    // Increment the ingredient quantity for the specific pizza
                                    pizzaIngredientList[i] = (pizza, pizzaIngredientTuple.Item2 + 1);
                                    foundPizza = true;
                                    break;
                                }
                            }

                            if (!foundPizza)
                            {
                                // Add a new tuple with the pizza and quantity
                                pizzaIngredientList.Add((pizza, 1));
                            }
                        }
                        else
                        {
                            // Create a new ingredient entry with the pizza and quantity
                            ingredients.Add(ingredient.Name, new List<(Pizza, int)> { (pizza, 1) });
                        }
                    }
                }
            }

            return ingredients;
        }

    }
}
