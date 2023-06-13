

using ProjetPizzaGroupe6.Entities;

namespace ProjetPizzaGroupe6.Services;

public class PizzaMenuService
{
    public List<Pizza> pizzas = new List<Pizza>() ;

    public void addPizza(Pizza pizza)
    {
        pizzas.Add(pizza);
    }

    private List<Pizza> getPizza()
    {
        return pizzas;
    }

    public void getMenu()
    {
        Console.WriteLine("################################################################################################################");
        Console.WriteLine("############################################ Affichage du menu #################################################");
        Console.WriteLine("################################################################################################################");

        foreach (Pizza element in pizzas)
        {
           
            Console.Write(element.nom + " => " + element.prix + " € \n" );
          
            foreach (string el in element.ingrediants)
            {
                Console.Write( "\t -" +el + ",\n") ;
                
            }
            Console.WriteLine();
            
        }
        Console.WriteLine("");
        Console.WriteLine("################################################################################################################");
    }
}