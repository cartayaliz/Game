# 1er proyecto de Programacion
## Maze Runners
### MATCOM - 2024-2025

---------------------------------
## Qué se necesita?
- Instalar visual studio o visual studio code
- Agregar el paquete NuGet para utilizar la biblioteca Spectre Console


##### Commands

* Run Game `dotnet run --project=.\MagicMaze\ConsoleGame\`
* Run TDD `dotnet test .\MagicMaze\TDD\`

* Cleaning `dotnet clean --project=.\MagicMaze\ConsoleGame\ `
* Restore `dotnet restore --project=.\MagicMaze\ConsoleGame\ `


## Acerca del juego
### Tematica: Emojis
En este juego multijugador el usuario tiene la posiblidad de elegir una opcion de hasta 5 jugadores simultaneos, lo que permite hacer de este juego una experiencia competitiva.

Cada jugador cuenta con caracteristicas propias y tiene una habilidad asociada, la cual tiene un tiempo de actividad y un tiempo de reposo. Asi mismo el usuario cuenta con la opcion de elegir cuando activar su habilidad simepre que no este activa antes y pueda ser activada. 
El emoji "camara" representa la habilidad de recordar el camino antes transitado.
El emoji "carro" representa la habilidad de aumentar velocidad.
El emoji "ojos" representa la habilidad de aumentar visibilidad.


En caso de que un jugador desee pasar su turno solo necesita presionar la tecla space. Eso sí, al pasar de turno no disminuye el tiempo de reposo de su habilidad. 

En el juego existen 4 tipos de celdas especiales, las cuales pueden beneficiar o perjudicar al jugador. 
& (emoji bicicleta) aumenta en 1 la velocidad.
$ (emoji semaforo) disminuye en 1 la velocidad.
? (emoji de campana) aumenta la visibilidad.
= (emoji de puente) se convierte en un obstáculo después de que un jugador salga de esta celda.

Los obstaculos están representados por una x (emoji de caja negra). 






