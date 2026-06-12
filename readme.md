# Kata RaigX

```text

| Alt-text: Paraula RaigX en ASCII Art

                          ____            _          __  __
                         |  _ \    __ _  (_)   __ _  \ \/ /
                         | |_) |  / _` | | |  / _` |  \  / 
                         |  _ <  | (_| | | | | (_| |  /  \ 
                         |_| \_\  \__,_| |_|  \__, | /_/\_\
                                              |___/       
                                         |___/        
╭────────────────────────────────────────────────────────────────────────────────────╮
│  ☢️  Sistema de control de radiació                                                │
╰────────────────────────────────────────────────────────────────────────────────────╯

```

## Introducció

L'hospital ha comprat una màquina de RaigX, la `MaquinaRaigX`. La màquina sap fer dues coses:
* Sap comprovar si tots els seus serveis estan operatius.
* Sap enviar radiació, té dos paràmetres: intensitat i durada.

L'hospital ha desenvolupat un programari per fer anar la màquina:
* `ControladoraRaigX`: a partir del pés d'una persona, calcula la intensitat i la durada i després fa servir `MaquinaRaigX` per enviar les radiacions. és important comprovar que la màquina té tots els sistemes actius abans d'enviar la radiació.
* `PanellDeControlUI`: és el programa que fan servir els metges per introduir les dades del pacient, aquest programa fa servir `ControladoraRaigX`


## Dependències

Aquest programa utilitza el patró [IoC (Inversion of Control)](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/ioc). Actualment, les classes s'inicialitzen manualment:

```c#
var maquina = new Maquina();
var controladora = new Controladora(maquina);
var app = new App(controladora);
await app.Main();
```

**Diagrama de dependències:**

```
App (UI)
  ↓ (depèn de)
Controladora
  ↓ (depèn de)
IMaquina (interfície)
  ↓ (implementada per)
Maquina (concreta)
```


## Primera part

**Refactoritzar per usar un contenidor d'injecció de dependències.** Transforma el codi de `Program.cs` per usar `Microsoft.Extensions.DependencyInjection` en lloc de crear les dependències manualment. A l'article [Dependency Injection in .NET Core Using IServiceCollection](https://www.c-sharpcorner.com/article/dependency-injection-in-net-core-using-iservicecollection/) pots trobar com fer-ho.

**Resultat esperat:** el contenidor de serveis ha d'injectar `Maquina` com `IMaquina`, `Controladora` com a Controladora, i `App` com a App.

## Segona part

Crear els testos que es demanen.

En aquesta Kata estem treballant testos "espia". Com et pots imaginar, per a fer els testos, no engegarem l'aparell de Raig X, no volem fregir a ningú. Substituirem l'aparell de Raig X per un dummy, un dummy espia.

Cemtrent-nos en el text `SiTotEsCorrecteSAplicaLaRadiacio`, per construir aquest test cal falsejar la màquina Raig X i:
* Quan ens preguntin `ComprovaTotsElsSistemesActius` hem de dir que `Sí`.
* Hem de comprovar que s'invoca `AplicaRadiació`.

Utilitzarem [`NSubstitute`](https://nsubstitute.github.io/).

### Instal·lació de NSubstitute

Per fer el Mock de la `MaquinaRaigX` et caldran unes llibreries de `Mock`. Rt proposo usar `NSubstitute`.

### Et cal ajuda?

Llegeix el fitxer [comeixementsprevis.md](./comeixementsprevis.md)
