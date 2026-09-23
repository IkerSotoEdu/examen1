namespace RuletaCasino.Models;

public class JuegoRuleta
{
    private readonly Jugador _jugador;
    private readonly Ruleta _ruleta;
    private readonly int _dineroInicial;

    public JuegoRuleta()
    {
        Console.Write("Nombre del jugador: ");
        string nombre = Console.ReadLine() ?? "Jugador";

        _jugador = new Jugador(nombre, 300);
        _ruleta = new Ruleta();
        _dineroInicial = 300;
    }

    public void Iniciar()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("            BIENVENIDO A LA RULETA");
        Console.WriteLine("========================================");
        Console.WriteLine($"Saldo inicial: ${_dineroInicial}");
        Console.WriteLine();

        bool continuar = true;

        while (continuar && _jugador.TieneDinero)
        {
            MostrarMenu();
            Console.Write("Selecciona una opción: ");
            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    ApostarNumero();
                    break;
                case "2":
                    ApostarColor();
                    break;
                case "3":
                    ApostarParidad();
                    break;
                case "4":
                    _ruleta.MostrarHistorial();
                    break;
                case "5":
                    Console.WriteLine("Te retiras de la mesa.");
                    continuar = false;
                    break;
                default:
                    Console.WriteLine("Opción no válida. Intenta de nuevo.");
                    break;
            }

            if (!_jugador.TieneDinero)
            {
                Console.WriteLine();
                Console.WriteLine("Se acabó tu dinero. La partida termina automáticamente.");
                continuar = false;
            }
        }

        MostrarResultadoFinal();
    }

    private void MostrarMenu()
    {
        Console.WriteLine();
        Console.WriteLine("MENU DE APUESTAS");
        Console.WriteLine("1) Apostar a un número");
        Console.WriteLine("2) Apostar a color");
        Console.WriteLine("3) Apostar a par o impar");
        Console.WriteLine("4) Ver historial de giros");
        Console.WriteLine("5) Retirarse");
        Console.WriteLine($"Saldo actual: ${_jugador.DineroDisponible}");
    }

    private void ApostarNumero()
    {
        try
        {
            int monto = LeerMonto();
            int numero = LeerNumero();
            var apuesta = new Apuesta(monto, TipoApuesta.Numero, numeroApostado: numero);
            ProcesarApuesta(apuesta);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void ApostarColor()
    {
        try
        {
            int monto = LeerMonto();
            Console.Write("Elige color (Rojo/Negro): ");
            string color = Console.ReadLine() ?? string.Empty;
            string colorNormalizado = NormalizarColor(color);
            var apuesta = new Apuesta(monto, TipoApuesta.Color, colorApostado: colorNormalizado);
            ProcesarApuesta(apuesta);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void ApostarParidad()
    {
        try
        {
            int monto = LeerMonto();
            Console.Write("Elige par o impar: ");
            string valor = Console.ReadLine() ?? string.Empty;
            bool par = NormalizarParidad(valor);
            var apuesta = new Apuesta(monto, TipoApuesta.Paridad, parApostado: par);
            ProcesarApuesta(apuesta);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void ProcesarApuesta(Apuesta apuesta)
    {
        if (!_jugador.Apostar(apuesta))
        {
            Console.WriteLine("No tienes suficiente dinero para esa apuesta.");
            return;
        }

        int numero = _ruleta.Girar();
        string color = Ruleta.ObtenerColor(numero);

        Console.WriteLine();
        Console.WriteLine($"La ruleta giró y cayó en: {numero} ({color})");

        if (apuesta.EsGanadora(numero, color))
        {
            int premio = apuesta.CalcularGanancia(numero, color);
            _jugador.Ganar(premio);
            Console.WriteLine($"¡Felicidades! Ganaste ${premio}.");
        }
        else
        {
            Console.WriteLine($"Perdiste ${apuesta.Monto}. Intenta otra vez.");
        }

        Console.WriteLine($"Saldo actual: ${_jugador.DineroDisponible}");
        Console.WriteLine();
    }

    private int LeerMonto()
    {
        Console.Write("Cantidad a apostar (múltiplo de 10): ");
        string? entrada = Console.ReadLine();

        if (!int.TryParse(entrada, out int monto) || monto <= 0)
            throw new ArgumentException("La cantidad debe ser un número válido mayor a cero.");

        if (monto % 10 != 0)
            throw new ArgumentException("La apuesta debe ser múltiplo de 10.");

        return monto;
    }

    private int LeerNumero()
    {
        Console.Write("Número a apostar (0-36): ");
        string? entrada = Console.ReadLine();

        if (!int.TryParse(entrada, out int numero) || numero < 0 || numero > 36)
            throw new ArgumentOutOfRangeException(nameof(entrada), "El número debe estar entre 0 y 36.");

        return numero;
    }

    private static string NormalizarColor(string valor)
    {
        string color = valor.Trim();

        if (string.Equals(color, "rojo", StringComparison.OrdinalIgnoreCase))
            return "Rojo";

        if (string.Equals(color, "negro", StringComparison.OrdinalIgnoreCase))
            return "Negro";

        throw new ArgumentException("El color debe ser Rojo o Negro.");
    }

    private static bool NormalizarParidad(string valor)
    {
        string paridad = valor.Trim();

        if (string.Equals(paridad, "par", StringComparison.OrdinalIgnoreCase))
            return true;

        if (string.Equals(paridad, "impar", StringComparison.OrdinalIgnoreCase))
            return false;

        throw new ArgumentException("Debes elegir Par o Impar.");
    }

    private void MostrarResultadoFinal()
    {
        int diferencia = _jugador.DineroDisponible - _dineroInicial;

        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine("          RESUMEN FINAL");
        Console.WriteLine("========================================");
        Console.WriteLine($"Dinero inicial: ${_dineroInicial}");
        Console.WriteLine($"Dinero final: ${_jugador.DineroDisponible}");

        if (diferencia > 0)
            Console.WriteLine($"¡Ganaste ${diferencia}!");
        else if (diferencia < 0)
            Console.WriteLine($"Perdiste ${Math.Abs(diferencia)}.");
        else
            Console.WriteLine("No ganaste ni perdiste dinero.");
    }
}
