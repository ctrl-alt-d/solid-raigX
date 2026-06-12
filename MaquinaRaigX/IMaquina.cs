namespace MaquinaRaigX;

public interface IMaquina
{
    public Task AplicaRadiació(int intensitat, int milisegons, CancellationToken cancellationToken);
    public bool ComprovaTotsElsSistemesActius();
}
