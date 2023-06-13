﻿    using System;
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
            foreach (var component in _components)
            {
                component.Display(depth);
            }
            Console.WriteLine("---------------------------------");
            Console.WriteLine($"Prix total : {GetPrice()} £");
            Console.WriteLine("---------------------------------");
        }

        public List<PizzaComponent> GetComponents()
        {
            return _components;
        }
    }
}
