namespace ProjetPizzaGroupe6
{
    public class VegetarianPizza : Pizza
    {
        public VegetarianPizza() : base("Végétarienne", 7.50m)
        {
            AddIngredient("tomate", 150);
            AddIngredient("mozzarella", 100);
            AddIngredient("courgette", 5);
            AddIngredient("poivron jaune", 1);
            AddIngredient("tomates cerises", 6);
            AddIngredient("olives", 1);
        }
    }
}
