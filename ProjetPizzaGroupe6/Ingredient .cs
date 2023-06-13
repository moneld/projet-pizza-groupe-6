using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPizzaGroupe6
{
    public class Ingredient : PizzaComponent
    {
        private decimal _price;
        private Dictionary<string, int> _ingredients;

        public Ingredient(string name, decimal price, Dictionary<string, int> ingredients) : base(name)
        {
            _price = price;
            _ingredients = ingredients;
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new string('\t', depth) + Name);
        }

        public override decimal GetPrice()
        {
            return _price;
        }

        public override Dictionary<string, int> GetIngredients()
        {
            return _ingredients;
        }
    }
}
