namespace RuletaCasino.Models;

public class Apuesta
{
    public int Monto { get; }
    public TipoApuesta Tipo { get; }
    public int? NumeroApostado { get; }
    public string? ColorApostado { get; }
    public bool? ParApostado { get; }

    public Apuesta(int monto, TipoApuesta tipo, int? numeroApostado = null, string? colorApostado = null, bool? parApostado = null)
    {
        if (monto <= 0)
            throw new ArgumentOutOfRangeException(nameof(monto), "La apuesta debe ser mayor a cero.");

        if (monto % 10 != 0)
            throw new ArgumentException("La apuesta debe ser múltiplo de 10.", nameof(monto));

        if (tipo == TipoApuesta.Numero && (numeroApostado is null || numeroApostado < 0 || numeroApostado > 36))
            throw new ArgumentOutOfRangeException(nameof(numeroApostado), "El número apostado debe estar entre 0 y 36.");

        if (tipo == TipoApuesta.Color && string.IsNullOrWhiteSpace(colorApostado))
            throw new ArgumentException("Debes indicar un color válido: Rojo o Negro.", nameof(colorApostado));

        if (tipo == TipoApuesta.Paridad && parApostado is null)
            throw new ArgumentException("Debes elegir entre Par o Impar.", nameof(parApostado));

        Monto = monto;
        Tipo = tipo;
        NumeroApostado = numeroApostado;
        ColorApostado = colorApostado;
        ParApostado = parApostado;
    }

    public int Multiplicador => Tipo switch
    {
        TipoApuesta.Numero => 10,
        TipoApuesta.Color => 5,
        TipoApuesta.Paridad => 2,
        _ => 0
    };

    public bool EsGanadora(int numero, string color)
    {
        return Tipo switch
        {
            TipoApuesta.Numero => numero == NumeroApostado,
            TipoApuesta.Color => string.Equals(color, ColorApostado, StringComparison.OrdinalIgnoreCase),
            TipoApuesta.Paridad => numero != 0 && ((ParApostado == true && numero % 2 == 0) || (ParApostado == false && numero % 2 != 0)),
            _ => false
        };
    }

    public int CalcularGanancia(int numero, string color)
    {
        return EsGanadora(numero, color) ? Monto * Multiplicador : 0;
    }

    public override string ToString()
    {
        return Tipo switch
        {
            TipoApuesta.Numero => $"Apuesta a número: {NumeroApostado} | Monto: ${Monto} | Ganancia: ${Monto * Multiplicador}",
            TipoApuesta.Color => $"Apuesta a color: {ColorApostado} | Monto: ${Monto} | Ganancia: ${Monto * Multiplicador}",
            TipoApuesta.Paridad => $"Apuesta a {(ParApostado == true ? "Par" : "Impar")} | Monto: ${Monto} | Ganancia: ${Monto * Multiplicador}",
            _ => $"Apuesta: ${Monto}"
        };
    }
}
