# Coneixements previs

Per fer els testos unitaris de la `ControladoraRaigX` no volem fer servir la `MaquinaRaigX`, perquè no podem disposar de la màquina real de raig X cada cop que fem proves. Llavors, utilitzarem un "doble de proves".

Existeixen diferents tipus de "Dobles de proves", en aquesta pràctica, com que volem saber com estem fent servir la màquina de raig X, farem servir un `Spy` (un espia)

## Llibreries dotnet per Mockejar la Màquina de Raig X

Per fer el Mock d'una dependència externa et caldran unes llibreries de `Mock`. Rt proposo usar `NSubstitute`.

Des de la carpeta del projecte de tests:

```bash
dotnet add tests/ControladoraRaigX.Tests package NSubstitute
```

## Mock, com instruir un Mock per a que els seus mètodes retornin el que nosaltres volem

Quan crees un mock amb `NSubstitute`, pots decidir què ha de retornar cada mètode amb `.Returns(...)`. Això et permet preparar l'escenari del test sense dependre d'una implementació real.

La idea és:
1. Crees el mock amb `Substitute.For<TInterficie>()`.
2. Indiques el valor de retorn del mètode que t'interessa.
3. Executes el codi que estàs testejant.

Exemple mínim (diferent del projecte):

Suposem una `IPassarelaMissatges` amb un mètode `ObteCreditsDisponibles()` i un servei `GestorCampanyes` que només pot enviar campanyes si hi ha crèdits.

```csharp
var passarela = Substitute.For<IPassarelaMissatges>();
var gestor = new GestorCampanyes(passarela);

// Instruïm el mock: quan es cridi ObteCreditsDisponibles, ha de retornar 25.
passarela.ObteCreditsDisponibles().Returns(25);

var esPotEnviar = await gestor.PotEnviarCampanya(CancellationToken.None);

Assert.True(esPotEnviar);
```

També pots canviar l'escenari molt ràpid: si poses `passarela.ObteCreditsDisponibles().Returns(0)`, el mateix test et permet validar el cas en què no es pot enviar cap campanya.



## Mock espia, com comprovar si s'ha invocat un mètode

Amb un `Spy` pots verificar si un mètode s'ha cridat. A `NSubstitute` es fa amb `Received()`.

Exemple mínim:

Suposem que tenim una classe `PassarelaMissatges` que s'encarrega d'enviar missatges a un servei extern, i un `GestorNotificacions` que utilitza aquesta passarel·la per processar i enviar notificacions.

```csharp
var passarela = Substitute.For<IPassarelaMissatges>();
var gestor = new GestorNotificacions(passarela);

passarela.EstaDisponible().Returns(true);
await gestor.ProcessaNotificacio("Recordatori", CancellationToken.None);

await passarela.Received(1).Envia(
  Arg.Any<string>(),
  Arg.Any<CancellationToken>());
```

`Received(1)` vol dir que esperes exactament una crida.


## Mock espia, com comprovar sque no s'ha invocat mètode

També pots verificar que un mètode no s'ha cridat. A `NSubstitute` es fa amb `DidNotReceive()`.

Exemple mínim:

En aquest cas, suposem que la `PassarelaMissatges` no està disponible. El `GestorNotificacions` ha de fallar abans d'intentar enviar res.

```csharp
var passarela = Substitute.For<IPassarelaMissatges>();
var gestor = new GestorNotificacions(passarela);

passarela.EstaDisponible().Returns(false);

await Assert.ThrowsAsync<InvalidOperationException>(() =>
  gestor.ProcessaNotificacio("Recordatori", CancellationToken.None));

await passarela.DidNotReceive().Envia(
  Arg.Any<string>(),
  Arg.Any<CancellationToken>());
```

`DidNotReceive()` vol dir que el mètode no s'ha d'haver executat cap vegada.


## Necessites més ajuda?

Amb la informació que has trobat fins ara hauries de poder fer els testos, però, si necessites més ajuda, aquí la tens:


### Com fer el test `SiTotEsCorrecteSAplicaLaRadiacio`

```csharp
[Fact]
public async Task SiTotEsCorrecteSAplicaLaRadiacio()
{
    // Arrange
  var passarela = Substitute.For<IPassarelaMissatges>();
  passarela.ComprovaTotsElsSistemesActius().Returns(true);

  var gestor = new GestorNotificacions(passarela);

    // Act
  await gestor.ProcessaNotificacio("Recordatori", CancellationToken.None);

    // Assert
  await passarela.Received(1).Envia(Arg.Any<string>(), Arg.Any<CancellationToken>());
}
```

### Com fer el test `SiLaMaquinaNoEstaOkLaControladoraLLançaUnaExcepcioInoAplicaRadiació`

```csharp
[Fact]
public async Task SiLaMaquinaNoEstaOkLaControladoraLLançaUnaExcepcioInoAplicaRadiació()
{
  // Arrange
  var passarela = Substitute.For<IPassarelaMissatges>();
  passarela.EstaDisponible().Returns(false);

  var gestor = new GestorNotificacions(passarela);

  // Act + Assert
  await Assert.ThrowsAsync<InvalidOperationException>(() =>
    gestor.ProcessaNotificacio("Recordatori", CancellationToken.None));

  await passarela.DidNotReceive().Envia(
    Arg.Any<string>(),
    Arg.Any<CancellationToken>());
}
```


