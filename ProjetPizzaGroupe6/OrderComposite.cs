    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPizzaGroupe6
{
    public class OrderComposite : PizzaComponent
    {
        private List<PizzaComponent> _components;
        private Dictionary<string, int> _pizzaQuantities; // Track pizza quantities

        public OrderComposite()
        {
            _components = new List<PizzaComponent>();
            _pizzaQuantities = new Dictionary<string, int>(); // Initialize the pizza quantities dictionary
        }

        public void AddComponent(PizzaComponent component)
        {
            _components.Add(component);
        }

        public void IncrementPizzaQuantity(string pizzaName, int quantity)
        {
            if (_pizzaQuantities.ContainsKey(pizzaName))
            {
                _pizzaQuantities[pizzaName] += quantity;
            }
            else
            {
                _pizzaQuantities.Add(pizzaName, quantity);
            }
        }

        public override decimal GetPrice()
        {
            return _components.Sum(component => component.GetPrice());
        }

        public override void Display(int depth)
        {

            foreach (var pizzaEntry in _pizzaQuantities)
            {
                var pizzaName = pizzaEntry.Key;
                var pizzaQuantity = pizzaEntry.Value;
                var pizza = _components.OfType<Pizza>().FirstOrDefault(p => p.Name == pizzaName);

                Console.WriteLine($"{pizzaQuantity} {pizzaName} : {pizzaQuantity * pizza.GetPrice()} €");

                foreach (var ingredient in pizza.GetIngredients())
                {
                    Console.WriteLine($"\t{ingredient.Name} {ingredient.Quantity}");
                }
            }
            Console.WriteLine("---------------------------------");
            Console.WriteLine($"Prix total : {GetPrice()} €");
            Console.WriteLine("---------------------------------");
        }

        public void DisplayInstructions()
        {
            foreach (var pizzaEntry in _pizzaQuantities)
            {
                var pizzaName = pizzaEntry.Key;
                var pizzaQuantity = pizzaEntry.Value;
                var pizza = _components.OfType<Pizza>().FirstOrDefault(p => p.Name == pizzaName);

                Console.WriteLine($"- {pizza.Name}");
                Console.WriteLine("Préparer la pâte");

                foreach (var ingredient in pizza.GetIngredients())
                {
                    Console.WriteLine($"\tAjouter {ingredient.Name}");
                }

                Console.WriteLine("Cuire la pizza\n");
            }
        }

        public List<PizzaComponent> GetComponents()
        {
            return _components;
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
