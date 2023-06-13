using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPizzaGroupe6
{
    public class Ingredient : PizzaComponent
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public Ingredient(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public override decimal GetPrice()
        {
            return 0; // Ingredients don't have a price
        }

        public override void Display(int depth)
        {
            Console.WriteLine($"{new string('-', depth)} {Name} : {Quantity}");
        }
    }
}
