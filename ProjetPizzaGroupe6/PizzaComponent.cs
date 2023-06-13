using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPizzaGroupe6
{
    public abstract class PizzaComponent
    {
        public string Name { get; }

        protected PizzaComponent(string name)
        {
            Name = name;
        }

        public abstract void Display(int depth);
        public abstract decimal GetPrice();
        public abstract Dictionary<string, int> GetIngredients();
    }

}
