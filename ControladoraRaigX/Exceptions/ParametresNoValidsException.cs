namespace ControladoraRaigX.Exceptions;

public class ParametresNoValidsException : Exception
{
    public ParametresNoValidsException() : base("Els paràmetres proporcionats no són vàlids.")
    {
    }
}