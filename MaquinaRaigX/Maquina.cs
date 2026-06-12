namespace MaquinaRaigX;

public class Maquina : IMaquina
{
    readonly Random random = new();
    public async Task AplicaRadiació(int intensitat, int milisegons, CancellationToken cancellationToken)
    {
        _ = intensitat; // Fem veure que fem servir la intensitat
        _ = milisegons; // Fem veure que fem servir la durada
        await Task.Delay(milisegons, cancellationToken);
    }

    public bool ComprovaTotsElsSistemesActius()
    {
        return random.Next(0, 15) % 3 == 0;
    }
}
