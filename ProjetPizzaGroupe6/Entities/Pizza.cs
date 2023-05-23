namespace ProjetPizzaGroupe6.Entities;

public class Pizza
{
    public String nom { get; private set; }
    public List<String> ingrediants { get; private set; }
    public double prix { get; private set; }

    public Pizza(string nom, List<string> ingrediants, double prix)
    {
        this.nom = nom;
        this.ingrediants = ingrediants;
        this.prix = prix;
    }

}