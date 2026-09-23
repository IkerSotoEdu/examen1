namespace RuletaCasino.Models;

public class Jugador
{
    public string Nombre { get; }
    public int DineroDisponible { get; private set; }
    public List<Apuesta> HistorialApuestas { get; } = new();

    public Jugador(string nombre, int dineroInicial = 300)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del jugador es obligatorio.", nameof(nombre));

        if (dineroInicial <= 0)
            throw new ArgumentOutOfRangeException(nameof(dineroInicial), "El dinero inicial debe ser mayor a cero.");

        Nombre = nombre;
        DineroDisponible = dineroInicial;
    }

    public bool Apostar(Apuesta apuesta)
    {
        if (DineroDisponible < apuesta.Monto)
            return false;

        DineroDisponible -= apuesta.Monto;
        HistorialApuestas.Add(apuesta);
        return true;
    }

    public void Ganar(int monto)
    {
        if (monto < 0)
            throw new ArgumentOutOfRangeException(nameof(monto), "La ganancia no puede ser negativa.");

        DineroDisponible += monto;
    }

    public bool TieneDinero => DineroDisponible > 0;

    public override string ToString()
    {
        return $"Jugador: {Nombre} | Dinero disponible: ${DineroDisponible}";
    }
}
