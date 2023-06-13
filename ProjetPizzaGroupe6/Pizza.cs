using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPizzaGroupe6
{
    public class Pizza : PizzaComponent
    {
        private List<PizzaComponent> _components;

        public Pizza(string name) : base(name)
        {
            _components = new List<PizzaComponent>();
        }

        public void AddComponent(PizzaComponent component)
        {
            _components.Add(component);
        }

        public override void Display(int depth)
        {
            if(Name!=null)
                Console.WriteLine(new string('\t', depth) + Name+ " => ");
            else
                Console.WriteLine(new string('\t', depth) + Name);

            foreach (var component in _components)
            {
                component.Display(depth + 1);
            }
        }

        public override decimal GetPrice()
        {
            decimal totalPrice = 0;

            foreach (var component in _components)
            {
                totalPrice += component.GetPrice();
            }

            return totalPrice;
        }

        public override Dictionary<string, int> GetIngredients()
        {
            Dictionary<string, int> ingredients = new Dictionary<string, int>();

            foreach (var component in _components)
            {
                var componentIngredients = component.GetIngredients();

                foreach (var ingredient in componentIngredients)
                {
                    if (ingredients.ContainsKey(ingredient.Key))
                    {
                        ingredients[ingredient.Key] += ingredient.Value;
                    }
                    else
                    {
                        ingredients.Add(ingredient.Key, ingredient.Value);
                    }
                }
            }

            return ingredients;
        }
    }

}
