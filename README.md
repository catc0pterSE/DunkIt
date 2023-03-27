This is a repository of a game which is currently in development process.

Current name: DUNKIT
Genre: casual, sports
Description: 3x3 basketball game with casual mechanics and UI-minigames . Gameplay cycle is simple: get the ball, get to the ring, score a goal any way would you like (throw or dunk)
Main features:
- amusing gameplay challenging player to use tactics (player can move, pass, throw, dunk and switch character he controls)
- unobtrusive minigames to dilute the dynamics (for throw, for jump-ball, for fight for ball)
- Challenging AI built using UtilityAI approach (meaning decision making based on dynamically calculated data)

Currently, game is built on primitives (capsules), because i am planning to finish core mechanics first, then get to models and animation.
I make videos with features i develop. Playlist: https://www.youtube.com/playlist?list=PLdi13vlsEuMr2YQwDRI99FKOhC-DQvvTt

I develop this game trying to keep OOP approach and SOLID principles in mind. Dependency injection type - Pure DI (zenject is too "massive" dor WebGL which is target platform)
Camera is maid by Cinemachine, cutscenes - by TimeLine, AI - with NodeCanvas Behavior Trees.
