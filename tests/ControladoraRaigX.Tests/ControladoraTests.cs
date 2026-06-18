using MaquinaRaigX;
using NSubstitute;

namespace ControladoraRaigX.Tests;


public class ControladoraTests
{
    [Fact]
    public async Task SiTotEsCorrecteSAplicaLaRadiacio()
    {
        // arrange
        var maquina = Substitute.For<IMaquina>();
        maquina.ComprovaTotsElsSistemesActius().Returns(true);
        var controladora = new Controladora(maquina);

        // act
        await controladora.AplicaRadiació(80, 180, CancellationToken.None);

        // assert
        // La controladora ha d'haver demanat a la màquina que emeti radiació
        await maquina.Received(1).AplicaRadiació(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>());
        // La controladora ha d'haver preguntat a la màquina si té tots els sistemes actius
        maquina.Received(1).ComprovaTotsElsSistemesActius();
        // L'ordre en que la controladora crida a la màquina és:
        Received.InOrder(() => {
            maquina.ComprovaTotsElsSistemesActius();
            maquina.AplicaRadiació(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>());
        });
    }


    [Fact]
    public async Task SiLaMaquinaNoEstaOkLaControladoraLLançaUnaExcepcioInoAplicaRadiació()
    {
        // arrange


        // act

        //

    }

    [Fact]
    public async Task SiElsParametresNoSónValidsLLançaUnaExcepcio()
    {
        
    }

    [Fact]
    public async Task NoEsPotSuperarElLlindarDeDurada()
    {
    }
}

