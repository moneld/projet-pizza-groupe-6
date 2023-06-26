namespace ProjetPizzaGroupe6
{
    public class Pizza : PizzaComponent
    {
        private Dictionary<string, int> _ingredients;
        private decimal _price;

        public string Name { get; set; }

        public Pizza(string name, decimal price)
        {
            Name = name;
            _price = price;
            _ingredients = new Dictionary<string, int>();
        }

        public void AddIngredient(string ingredient, int quantity)
        {
            if (_ingredients.ContainsKey(ingredient))
                _ingredients[ingredient] += quantity;
            else
                _ingredients.Add(ingredient, quantity);
        }

        public override decimal GetPrice()
        {
            return _price;
        }

        public override void Display(int depth)
        {
            var indent = new string('\t', depth);

            Console.WriteLine($"{new string('-', depth)} {Name} : {_price} €");
            foreach (var ingredient in _ingredients)
            {
                Console.WriteLine($"{indent}{ingredient.Key} {ingredient.Value}");
            }
        }


        public List<Ingredient> GetIngredients()
        {
            var ingredientsList = new List<Ingredient>();

            foreach (var ingredient in _ingredients)
            {
                var ingredientName = ingredient.Key;
                var ingredientQuantity = ingredient.Value;
                var ingredientObj = new Ingredient(ingredientName, ingredientQuantity);
                ingredientsList.Add(ingredientObj);
            }

            return ingredientsList;
        }
    }

}
