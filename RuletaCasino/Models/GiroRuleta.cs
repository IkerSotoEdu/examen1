namespace RuletaCasino.Models;

public class GiroRuleta
{
    public int Numero { get; }
    public string Color { get; }
    public bool? EsPar { get; }
    public DateTime Fecha { get; }

    public GiroRuleta(int numero, string color, bool? esPar)
    {
        Numero = numero;
        Color = color;
        EsPar = esPar;
        Fecha = DateTime.Now;
    }

    public override string ToString()
    {
        string paridad = EsPar == true ? "Par" : EsPar == false ? "Impar" : "N/A";
        return $"Número: {Numero,2} | Color: {Color,-5} | Paridad: {paridad,-5} | {Fecha:HH:mm:ss}";
    }
}
