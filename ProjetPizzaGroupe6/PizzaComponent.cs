using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPizzaGroupe6
{
    public abstract class PizzaComponent
    {
        public abstract decimal GetPrice();
        public abstract void Display(int depth);
    }

}
