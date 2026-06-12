using ControladoraRaigX;
using ControladoraRaigX.Exceptions;
using MaquinaRaigX;
using Spectre.Console;

namespace PanellControlUI;

public class App(Controladora controladora)
{


    public async Task Main()
    {
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };


        while (!cts.IsCancellationRequested)
        {
            MostrarCapcalera();

            string opcio = DemanarOpcioMenuPrincipal();
            if (opcio == "🚪 Sortir") break;

            (int pes, int alcada) = DemanarDadesRadiació();

            bool cancelada = await AplicarRaigXAmbReintents(controladora, pes, alcada, cts);
            if (cancelada) return;
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule("[dim]sessió tancada[/]").RuleStyle("grey").LeftJustified());
        AnsiConsole.MarkupLine("[dim]👋  Fins aviat![/]");
        AnsiConsole.WriteLine();
    }

    private static void MostrarCapcalera()
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine();
        AnsiConsole.Write(
            new FigletText("RaigX")
                .Centered()
                .Color(Color.Cyan1));

        AnsiConsole.Write(
            new Panel("[bold white]☢️  Sistema de control de radiació[/]")
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Cyan1)
                .Padding(2, 0)
                .Expand());

        AnsiConsole.WriteLine();
    }

    private static string DemanarOpcioMenuPrincipal() =>
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold yellow]Menú principal[/]")
                .HighlightStyle(Style.Parse("bold cyan"))
                .AddChoices("☢️  Aplicar RaigX ...", "🚪 Sortir"));

    private static (int pes, int alcada) DemanarDadesRadiació()
    {
        AnsiConsole.Write(new Rule("[bold yellow]🧍 Dades del pacient[/]").RuleStyle("yellow"));
        AnsiConsole.WriteLine();

        int pes = AnsiConsole.Prompt(
            new TextPrompt<int>("⚖️  [green]Pes[/] (kg):")
                .PromptStyle("bold white")
                .ValidationErrorMessage("[red]⚠  El pes ha de ser major que 0.[/]")
                .Validate(v => v > 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error("")));

        int alcada = AnsiConsole.Prompt(
            new TextPrompt<int>("📏 [green]Alçada[/] (cm):")
                .PromptStyle("bold white")
                .ValidationErrorMessage("[red]⚠  L'alçada ha de ser major que 0.[/]")
                .Validate(v => v > 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error("")));

        AnsiConsole.WriteLine();
        var taula = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[grey]Paràmetre[/]")
            .AddColumn("[grey]Valor[/]")
            .AddRow("⚖️   Pes", $"[bold white]{pes} kg[/]")
            .AddRow("📏 Alçada", $"[bold white]{alcada} cm[/]");
        AnsiConsole.Write(taula);
        AnsiConsole.WriteLine();

        return (pes, alcada);
    }

    private static async Task<bool> AplicarRaigXAmbReintents(
        Controladora controladora, int pes, int alcada, CancellationTokenSource cts)
    {
        while (!cts.IsCancellationRequested)
        {
            try
            {
                await AnsiConsole.Status()
                    .Spinner(Spinner.Known.Aesthetic)
                    .SpinnerStyle(Style.Parse("bold yellow"))
                    .StartAsync("☢️  [yellow]Aplicant RaigX...[/]  [dim]No moguis el pacient[/]", async _ =>
                        await controladora.AplicaRadiació(pes, alcada, cts.Token));

                AnsiConsole.WriteLine();
                AnsiConsole.Write(
                    new Panel("[bold green]✅  RaigX aplicat correctament![/]")
                        .Border(BoxBorder.Rounded)
                        .BorderColor(Color.Green)
                        .Padding(1, 0));
            }
            catch (OperationCanceledException)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[bold yellow]⚠️  Operació cancel·lada.[/]");
                return true;
            }
            catch (Exception ex) when (ex is MaquinaNoOkException or ParametresNoValidsException)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.Write(
                    new Panel($"[bold red]🚨  {Markup.Escape(ex.Message)}[/]")
                        .Border(BoxBorder.Heavy)
                        .BorderColor(Color.Red)
                        .Padding(1, 0));
            }

            AnsiConsole.WriteLine();

            string seguent = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Què vols fer?[/]")
                    .HighlightStyle(Style.Parse("bold cyan"))
                    .AddChoices("🔁 Torna-hi", "⬅️  Menú anterior"));

            if (seguent == "⬅️  Menú anterior") break;
        }

        return false;
    }
}
