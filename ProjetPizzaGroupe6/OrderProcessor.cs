﻿using System.Xml.Serialization;
using System.Text.Json;
using System.Xml;
using System.Text.RegularExpressions;

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
            vegetariennePizza.AddIngredient("olives", 1); 
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

                    var sumTotalQuantity = 0;

                    foreach (var (pizza, quantity) in ingredientEntry.Value)
                    {
                        var ingredientQuantity = pizza.GetIngredients()
                            .FirstOrDefault(i => i.Name == ingredientName)?.Quantity ?? 0;
                        var ingredientTotal = ingredientQuantity * quantity;
                        sumTotalQuantity += ingredientTotal;
                    }

                    Console.WriteLine($"{ingredientName} : {sumTotalQuantity}");

                    foreach (var (pizza, quantity) in ingredientEntry.Value)
                    {
                        var ingredientQuantity = pizza.GetIngredients()
                            .FirstOrDefault(i => i.Name == ingredientName)?.Quantity ?? 0;
                        var ingredientTotal = ingredientQuantity * quantity;
                        Console.WriteLine($"- {pizza.Name} : {ingredientTotal}");
                    }
                }


                Console.WriteLine("\nVoulez-vous enregistrer la facture ? (oui/non)");
                var saveInvoice = Console.ReadLine()?.ToLower();
                if (saveInvoice == "oui" || saveInvoice == "yes")
                {
                    var invoice = formatInvoice(order);

                    Console.WriteLine("Choisissez le format de fichier pour enregistrer la facture :");
                    Console.WriteLine("1. JSON");
                    Console.WriteLine("2. XML");
                    var formatChoice = Console.ReadLine();

                    switch (formatChoice)
                    {
                        case "1":
                            SaveInvoiceToJsonFile(invoice);
                            break;
                        case "2":
                            SaveInvoiceToXmlFile(invoice);
                            break;
                        default:
                            Console.WriteLine("Format de fichier non pris en charge. La facture ne sera pas enregistrée.");
                            break;
                    }
                }
            }
        }

        private static List<string> LoadOrdersFromJsonFile(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                var ordersObject = JsonSerializer.Deserialize<JsonDocument>(json);
                var orders = new Dictionary<string, int>();

                if (ordersObject.RootElement.TryGetProperty("order", out var orderArray))
                {
                    foreach (var orderElement in orderArray.EnumerateArray())
                    {
                        var pizza = orderElement.GetProperty("pizza").GetString();
                        var quantity = orderElement.GetProperty("quantite").GetInt32();

                        if (orders.ContainsKey(pizza))
                        {
                            orders[pizza] += quantity;
                        }
                        else
                        {
                            orders[pizza] = quantity;
                        }
                    }
                }

                var orderStrings = orders.Select(kvp => $"{kvp.Value} {kvp.Key}").ToList();
                return orderStrings;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading orders from the JSON file: {ex.Message}");
                return null;
            }
        }

        private static List<string> LoadOrdersFromXmlFile(string filePath)
        {
            try
            {
                var orders = new Dictionary<string, int>();

                var xmlDocument = new XmlDocument();
                xmlDocument.Load(filePath);

                var orderNodes = xmlDocument.SelectNodes("//order");
                foreach (XmlNode orderNode in orderNodes)
                {
                    var pizzaNode = orderNode.SelectSingleNode("pizza");
                    var quantityNode = orderNode.SelectSingleNode("quantity");

                    if (pizzaNode != null && quantityNode != null)
                    {
                        var pizza = pizzaNode.InnerText;
                        var quantity = int.Parse(quantityNode.InnerText);

                        if (orders.ContainsKey(pizza))
                        {
                            orders[pizza] += quantity;
                        }
                        else
                        {
                            orders[pizza] = quantity;
                        }
                    }
                }

                var orderStrings = orders.Select(kvp => $"{kvp.Value} {kvp.Key}").ToList();
                return orderStrings;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors du chargement des commandes depuis le fichier XML : {ex.Message}");
                return null;
            }
        }


        public void VerifyFilePath(string filePath)
        {
            if (File.Exists(filePath))
            {
                var fileExtension = Path.GetExtension(filePath);

                if (fileExtension.Equals(".json", StringComparison.OrdinalIgnoreCase))
                {
                    var orders = LoadOrdersFromJsonFile(filePath);
                    if (orders != null)
                    {
                        var allOrders = string.Join(", ", orders);
                        Console.WriteLine("\n#####################################");
                        Console.WriteLine($"Commande chargée : ");
                        Console.WriteLine("#####################################");
                        Console.WriteLine(allOrders);
                        ProcessOrder(allOrders);
                    }
                }
                else if (fileExtension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    var orders = LoadOrdersFromXmlFile(filePath);
                    if (orders != null)
                    {
                        var allOrders = string.Join(", ", orders);
                        Console.WriteLine("\n#####################################");
                        Console.WriteLine($"Commande chargée : ");
                        Console.WriteLine("#####################################");
                        Console.WriteLine(allOrders);
                        ProcessOrder(allOrders);
                    }
                }
                else
                {
                    Console.WriteLine("Le format de fichier n'est pas pris en charge.");
                }
            }
            else
            {
                Console.WriteLine("Le fichier spécifié n'existe pas.");
            }
        }

        private OrderComposite ParseOrder(string input)
        {
            var order = new OrderComposite();
            var pizzas = input.Split(',');

            var pizzaQuantities = new Dictionary<string, int>();

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
                    var quantityExpression = quantityStr.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
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
                                pizzaQuantities[pizzaName] += pizzaQuantity;
                            }
                            else
                            {
                                pizzaQuantities.Add(pizzaName, pizzaQuantity); 
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
                        var ingredientNameCopy = ingredient.Name; 

                        if (ingredients.ContainsKey(ingredientNameCopy))
                        {
                            var pizzaIngredientList = ingredients[ingredientNameCopy];
                            bool foundPizza = false;

                            for (int i = 0; i < pizzaIngredientList.Count; i++)
                            {
                                var (existingPizza, quantity) = pizzaIngredientList[i];

                                if (existingPizza == pizza)
                                {
                                    pizzaIngredientList[i] = (existingPizza, quantity + 1);
                                    foundPizza = true;
                                    break;
                                }
                            }

                            if (!foundPizza)
                            {
                                pizzaIngredientList.Add((pizza, 1));
                            }
                        }
                        else
                        {
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

        private Invoice formatInvoice(OrderComposite order)
        {
            var invoice = new Invoice
            {
                Items = new List<InvoiceItem>(),
                TotalPrice = order.GetPrice()
            };               

            foreach (var pizzaEntry in order.PizzaQuantities)
            {
                var pizzaName = pizzaEntry.Key;
                var pizzaQuantity = pizzaEntry.Value;
                var pizza = order.GetComponents().OfType<Pizza>().FirstOrDefault(p => p.Name == pizzaName);
                var item = new InvoiceItem
                {
                    Name = Regex.Unescape(pizzaName),
                    Quantity = pizzaQuantity,
                    Price = pizzaQuantity * pizza.GetPrice(),
                    Ingredients = pizza.GetIngredients().Select(ingredient => $"{Regex.Unescape(ingredient.Name)} {ingredient.Quantity}").ToList()
                };

                invoice.Items.Add(item);
            }
            
            return invoice;
        }
        private void SaveInvoiceToJsonFile(Invoice invoice)
        {
            try
            {                    
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                var json = JsonSerializer.Serialize(invoice, options);
                var fileName = $"facture_{DateTime.Now:yyyyMMddHHmmss}.json";
                File.WriteAllText(fileName, json);
                Console.WriteLine($"Facture enregistrée au format JSON : {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de l'enregistrement de la facture au format JSON : {ex.Message}");
            }
        }

        private void SaveInvoiceToXmlFile(Invoice invoice)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Invoice));
                var fileName = $"facture_{DateTime.Now:yyyyMMddHHmmss}.xml";
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    serializer.Serialize(fileStream, invoice);
                    Console.WriteLine($"Facture enregistrée au format XML : {fileName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de l'enregistrement de la facture au format XML : {ex.Message}");
            }
        }

    }
}
