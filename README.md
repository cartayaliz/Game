# 1er proyecto de Programacion
## Maze Runners
### MATCOM - 2024-2025

---------------------------------
## Qué se necesita?
- Instalar visual studio o visual studio code
- Agregar el paquete NuGet para utilizar la biblioteca Spectre Console


##### Commands

* Run Game `dotnet run --project=.\MagicMaze\Logic\`

* Cleaning `dotnet clean --project=.\MagicMaze\Logic\ `
* Restore `dotnet restore --project=.\MagicMaze\Logic\ `


## Acerca del juego
### Tematica: Emojis
En este juego multijugador el usuario tiene la posiblidad de elegir una opcion de hasta 5 jugadores simultaneos, lo que permite hacer de este juego una experiencia competitiva.


Cada jugador cuenta con caracteristicas propias y tiene una habilidad asociada, la cual tiene un tiempo de actividad y un tiempo de reposo. Asi mismo el usuario cuenta con la opcion de elegir cuando activar su habilidad simepre que no este activa antes y pueda ser activada. 
El emoji "camara":camera: representa la habilidad de recordar el camino antes transitado.
El emoji "carro":oncoming_police_car: representa la habilidad de aumentar velocidad.
El emoji "ojos" :eyes: representa la habilidad de aumentar visibilidad.


En caso de que un jugador desee pasar su turno solo necesita presionar la tecla space. Eso sí, al pasar de turno no disminuye el tiempo de reposo de su habilidad. 

En el juego existen 4 tipos de celdas especiales, las cuales pueden beneficiar o perjudicar al jugador. 
& (emoji bicicleta):bicycle: aumenta en 1 la velocidad.
$ (emoji semaforo) :vertical_traffic_light: disminuye en 1 la velocidad.
? (emoji de campana) :bell: aumenta la visibilidad.
= (emoji de puente) :bridge_at_night: se convierte en un obstáculo después de que un jugador salga de esta celda.

Los obstaculos están representados por una x (emoji de caja negra) :black_large_square:. 

La parte logica está dividida en varios ficheros y clases de manera que las clases tienen  responsabilidades individuales.
La responsabilidad de la logica se divide en 4 elementos fundamentales. El tablero, los jugadores, lo visual y la relacion de los elementos anteriores.

La clase GameCenter se encarga de la lógica. Recibe una insterface Ivisual que contiene todos los metodos que permiten visualizar los diferentes momentos que pueden producirse en el juego. De modo que si se desea se puede cambiar la interface sin muchos problemas. Esto permitió que pudiera utilizar tanto una visualizacion en la consola, como una empleando la biblioteca Spectre Console. El GameCenter se encarga de conectar los jugadores y sus habilidades, con el tablero, y las reglas del juego.

La clase Cell se encarga unicamente de definir todos los tipos de casillas que posee tablero. Tiene dos metodos virtual que permiten crear nuevas celdas utilizando todos los metodos anteriores, sobreescribiendo  si el jugador puede entrar en una casilla del tablero, que sucede cuando entra y que sucede con este usuario o con el tablero luego de salir de ella. De este modo fue posible crear los diferentes tipos de celdas disponibles en este juego.

La clase Player define todas las caracteristicas que posen los jugadores: la velocidad, el nivel de visibilidad, asi como la capacidad de recordar o no el camino que antes ha recorrido. Esta clase controla la utilizacion y el manejo de las habilidades del jugador. Permitió crear distintos tipos de jugadores que fueron el resultado de combinar las caracteristicas que puede o no poseer el jugador y las disntintas habilidades que pueda emplear.

La clase Board tiene como responsabilidades disponer en el tablero las distintas piezas que interactuan. Saber los movimientos que puede hacer un jugador y realizarlos; y definir las casillas que un jugador puede ver.  









