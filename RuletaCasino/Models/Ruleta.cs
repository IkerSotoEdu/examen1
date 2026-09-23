namespace RuletaCasino.Models;

public class Ruleta
{
    private static readonly HashSet<int> NumerosNegros = new()
    {
        2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35
    };

    public List<GiroRuleta> Historial { get; } = new();

    public int Girar()
    {
        int numero = Random.Shared.Next(0, 37);
        string color = ObtenerColor(numero);
        bool? esPar = numero == 0 ? null : numero % 2 == 0;

        var giro = new GiroRuleta(numero, color, esPar);
        Historial.Add(giro);
        return numero;
    }

    public static string ObtenerColor(int numero)
    {
        if (numero == 0)
            return "Verde";

        return NumerosNegros.Contains(numero) ? "Negro" : "Rojo";
    }

    public void MostrarHistorial()
    {
        if (Historial.Count == 0)
        {
            Console.WriteLine("Aún no se ha realizado ningún giro.");
            return;
        }

        Console.WriteLine("Historial de giros:");
        foreach (var giro in Historial)
        {
            Console.WriteLine(giro);
        }
    }
}
