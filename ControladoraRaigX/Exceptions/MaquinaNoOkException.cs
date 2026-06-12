namespace ControladoraRaigX.Exceptions;

public class MaquinaNoOkException : Exception
{
    public MaquinaNoOkException() : base("La màquina no està en condicions de funcionar.")
    {
    }
}
