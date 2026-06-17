using ControladoraRaigX;
using MaquinaRaigX;
using Microsoft.Extensions.DependencyInjection;

namespace PanellControlUI;

class Program
{
    static async Task Main(string[] args)
    {
        _ = args;
        
        var services = new ServiceCollection();

        services.AddSingleton<IMaquina, Maquina>();
        services.AddSingleton<App>();
        services.AddSingleton<Controladora>();

        var serviceProvider = services.BuildServiceProvider();

        var app = serviceProvider.GetRequiredService<App>();

        await app.Main();
        
    }
    
}
