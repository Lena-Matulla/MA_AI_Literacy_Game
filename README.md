# Projekt Setup

## Unity-Version

Dieses Projekt wurde erstellt mit:

Unity 6000.3.9f1

Bitte möglichst dieselbe Unity-Version verwenden.

## Installation

1. Repository klonen oder herunterladen.
2. Projekt über Unity Hub öffnen.
3. Falls Unity fehlende Assets meldet, die folgenden externen Assets installieren.

## Externe Assets:

### A* Pathfinding Project

Dieses Asset wird benötigt damit die NPCs richtig funktionieren (pathfinder). Eigentlich sollte alles nötige hierfür über das Github mitgeliefert werden. Falls es dennoch zu problemen kommt muss es gegebenenfalls separat installiert werden über folgenden Link. Für das Projekt wurde die kostenlose Version benutzt.

https://arongranberg.com/astar/download

Danach Unity neu laden.

## Projekt richtig Starten
Es gibt zwei relevante Scenen unter dem Ordner "Scenens".
"StartScene" und "Game" sind hierbei die relevanten. Die anderen beiden werden im späteren Verlauf noch gelöscht.

Um das Spiel korrekt zu starten muss die "StartScene" Scene gestartet werden. Diese startet dann automatisch die "Game" Scene. (Also nicht einfach die "Game" Scene starten, das wird zu fehlen führen, es muss über die "StartScene" gegangen werden)

## Weitere Hinweise:

Falls beim ersten Öffnen Fehler auftreten, zuerst prüfen, ob alle externen Assets korrekt importiert wurden.