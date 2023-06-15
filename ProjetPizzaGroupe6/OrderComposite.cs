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
    }
}
