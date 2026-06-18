using ControladoraRaigX.Exceptions;
using MaquinaRaigX;

namespace ControladoraRaigX;

public class Controladora(IMaquina maquina)
{
    public const int MAXDURADA = 5000;

    public async Task AplicaRadiació(int pes, int alcada, CancellationToken cancellationToken)
    {
        if (!maquina.ComprovaTotsElsSistemesActius())
        {
            throw new MaquinaNoOkException();
        }

        if (pes <= 0 || alcada <= 0)
        {
            throw new ParametresNoValidsException();
        }

        int intensitat = pes * 10 + alcada;
        int durada = Math.Min(MAXDURADA, alcada * 20);

        await maquina.AplicaRadiació(intensitat, durada, cancellationToken);
    }

}
