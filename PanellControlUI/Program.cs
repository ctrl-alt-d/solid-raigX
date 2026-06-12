using ControladoraRaigX;
using MaquinaRaigX;

namespace PanellControlUI;

class Program
{
    static async Task Main(string[] args)
    {
        _ = args;
        var maquina = new Maquina();
        var controladora = new Controladora(maquina);
        var app = new App(controladora);
        await app.Main();
    }
}
