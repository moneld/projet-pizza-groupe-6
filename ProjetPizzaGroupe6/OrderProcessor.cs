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
            vegetariennePizza.AddIngredient("olives", 1); //1+2 4 Saisons, 1 Regina, 2 Végétarienne
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
                order.DisplayInstructions();
                Console.WriteLine("#####################################");
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
                        var ingredientQuantity = pizza.GetIngredients()
                            .FirstOrDefault(i => i.Name == ingredientName)?.Quantity ?? 0;
                        Console.WriteLine($"- {pizza.Name} : {ingredientQuantity * quantity}");
                    }
                    Console.WriteLine();
                }
            }
        }

        private OrderComposite ParseOrder(string input)
        {
            var order = new OrderComposite();
            var pizzas = input.Split(',');

            var pizzaQuantities = new Dictionary<string, int>(); // Track pizza quantities

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

                if (quantityStr.Contains('+'))
                {
                    var quantityExpression = quantityStr.Split('+');
                    var totalQuantity = 0;

                    foreach (var expr in quantityExpression)
                    {
                        if (!int.TryParse(expr.Trim(), out var exprQuantity))
                        {
                            Console.WriteLine($"Quantité incorrecte : '{quantityStr}'.\n");
                            order = null;
                            break;
                        }

                        totalQuantity += exprQuantity;
                    }

                    if (order == null)
                    {
                        continue;
                    }

                    quantityStr = totalQuantity.ToString();
                }

                if (!int.TryParse(quantityStr, out var quantity))
                {
                    Console.WriteLine($"Quantité incorrecte : '{quantityStr}'.\n");
                    order = null;
                    continue;
                }
                
                if (_pizzas.ContainsKey(pizzaName))
                {
                    var pizza = _pizzas[pizzaName];
                    if (int.TryParse(quantityStr, out var pizzaQuantity))
                    {
                        if (pizzaQuantity < 0)
                        {
                            Console.WriteLine($"Quantité incorrecte : '{quantityStr}'.\n");
                            order = null;
                            break;
                        }
                        else
                        {
                            for (var i = 0; i < pizzaQuantity; i++)
                            {
                                order.AddComponent(pizza);
                            }

                            if (pizzaQuantities.ContainsKey(pizzaName))
                            {
                                pizzaQuantities[pizzaName] += pizzaQuantity; // Increment the pizza quantity
                            }
                            else
                            {
                                pizzaQuantities.Add(pizzaName, pizzaQuantity); // Add the pizza quantity to the tracker
                            }
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

            // Consolidate the pizza quantities within the order
            foreach (var pizzaEntry in pizzaQuantities)
            {
                var pizzaName = pizzaEntry.Key;
                var pizzaQuantity = pizzaEntry.Value;

                order.IncrementPizzaQuantity(pizzaName, pizzaQuantity);
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
                        var ingredientNameCopy = ingredient.Name; // Create a copy of the variable

                        if (ingredients.ContainsKey(ingredientNameCopy))
                        {
                            var pizzaIngredientList = ingredients[ingredientNameCopy];
                            bool foundPizza = false;

                            for (int i = 0; i < pizzaIngredientList.Count; i++)
                            {
                                var (existingPizza, quantity) = pizzaIngredientList[i];

                                if (existingPizza == pizza)
                                {
                                    // Increment the ingredient quantity for the specific pizza
                                    pizzaIngredientList[i] = (existingPizza, quantity + 1);
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
                            ingredients.Add(ingredientNameCopy, new List<(Pizza, int)> { (pizza, 1) });
                        }
                    }
                }
            }

            return ingredients;
        }

        public void DisplayAvailablePizzas()
        {
            Console.WriteLine("La pizzeria est capable de produire les pizzas suivantes:");

            Dictionary<string, decimal> availablePizzas = new Dictionary<string, decimal>
            {
                { "Regina", 8m },
                { "4 Saisons", 9m },
                { "Végétarienne", 7.50m }
            };

            foreach (var pizzaEntry in availablePizzas)
            {
                var pizzaName = pizzaEntry.Key;
                var pizzaPrice = pizzaEntry.Value;
                var pizza = GetPizzaByName(pizzaName);

                Console.WriteLine($"● {pizzaName} => {pizzaPrice} €");

                foreach (var ingredient in pizza.GetIngredients())
                {
                    Console.WriteLine($"○ {ingredient.Quantity} {ingredient.Name}");
                }

                Console.WriteLine();
            }
        }

        private Pizza GetPizzaByName(string name)
        {
            switch (name)
            {
                case "Regina":
                    return new ReginaPizza();
                case "4 Saisons":
                    return new FourSeasonsPizza();
                case "Végétarienne":
                    return new VegetarianPizza();
                default:
                    return null;
            }
        }
    }
}
