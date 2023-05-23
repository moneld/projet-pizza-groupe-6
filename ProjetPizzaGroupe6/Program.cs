using ProjetPizzaGroupe6.Entities;
using ProjetPizzaGroupe6.Services;

PizzaMenuService menu = new PizzaMenuService();

//Pizza Regina
List<String> ingredientsRegina = new List<string>();
ingredientsRegina.Add("150g de tomate");
ingredientsRegina.Add("125g de mozzarella");
ingredientsRegina.Add("100g de fromage râpé");
ingredientsRegina.Add("2 tranches de Jambon");
ingredientsRegina.Add("4 champignons frais");
ingredientsRegina.Add("2 cuillères à soupe d’huile d’olive");
Pizza pizzaRegina = new Pizza("Regina", ingredientsRegina, 8.00);

//Pizza 4 Saisons
List<String> ingredients4Saisons = new List<string>();
ingredients4Saisons.Add("150g de tomate");
ingredients4Saisons.Add("125g de mozzarella");
ingredients4Saisons.Add("2 tranches de Jambon");
ingredients4Saisons.Add("100g de champignons frais");
ingredients4Saisons.Add("0,5 poivron");
ingredients4Saisons.Add("1 poignée d’olives");
Pizza pizza4Saisons= new Pizza("4Saisons", ingredients4Saisons, 9.00);


//Pizza Vegetarienne
List<String> ingredientsVegetarienne = new List<string>();
ingredientsVegetarienne.Add("150g de tomate");
ingredientsVegetarienne.Add("100g de mozzarella");
ingredientsVegetarienne.Add("0,5 courgette");
ingredientsVegetarienne.Add("1 poivron jaune");
ingredientsVegetarienne.Add("6 tomates cerises");
ingredientsVegetarienne.Add("quelques olives");
Pizza pizzaVegetarienne= new Pizza("Végétarienne", ingredientsVegetarienne, 7.50);


menu.addPizza(pizzaRegina);
menu.addPizza(pizza4Saisons);
menu.addPizza(pizzaVegetarienne);

string continu = "O";
string commands = "";


menu.getMenu();

while ("n" != continu)
{
    Console.WriteLine("Quels pizza souhaitez vous ?");
    string commandSaisir = Console.ReadLine();
    commands += commandSaisir+ ",";
    Console.WriteLine("Souhaitez vous en ajouter encore  ? O/n");
    continu = Console.ReadLine();
}

commands = commands.Substring(0,commands.Length-1);


Console.WriteLine(commands);
Console.WriteLine("");

Console.WriteLine("################################################################################################################");
Console.WriteLine("############################################ Affichage du panier #################################################");
Console.WriteLine("################################################################################################################");

string[] items = commands.Split(',');
int nb = items.Length;


Console.WriteLine(nb);
for (int i = 0; i < nb; i++)
{
    string[] item = items[i].Split(' ');
 
    
    if(item[1].ToLower()== "regina")
    {
        Console.WriteLine("Vous avez commander " + item[0] + " pizza " + item[1]);
        
    }
    if(item[1].ToLower()== "4saisons")
    {
        Console.WriteLine("Vous avez commander " + item[0] + " pizza " + item[1]);
        
    }
    if(item[1].ToLower()== "végétarienne")
    {
        Console.WriteLine("Vous avez commander " + item[0] + " pizza " + item[1]);
        
    }
}