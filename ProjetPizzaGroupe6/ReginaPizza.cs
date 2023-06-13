using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPizzaGroupe6
{
    public class ReginaPizza : Pizza
    {
        public ReginaPizza() : base("Regina", 8m)
        {
            AddIngredient("tomate", 150);
            AddIngredient("mozzarella", 125);
            AddIngredient("fromage râpé", 100);
            AddIngredient("jambon", 2);
            AddIngredient("champignons frais", 4);
            AddIngredient("huile d'olive", 2);
        }
    }
}
