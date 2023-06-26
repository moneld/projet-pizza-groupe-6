namespace ProjetPizzaGroupe6
{
    public class FourSeasonsPizza : Pizza
    {
        public FourSeasonsPizza() : base("4 Saisons", 9m)
        {
            AddIngredient("tomate", 150);
            AddIngredient("mozzarella", 125);
            AddIngredient("jambon", 2);
            AddIngredient("champignons frais", 100);
            AddIngredient("poivron", 5);
            AddIngredient("olives", 1);
        }
    }
}
