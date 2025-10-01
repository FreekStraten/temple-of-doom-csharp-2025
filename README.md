# Temple of Doom (C# Console)

Console dungeon crawler als **duo-opdracht** voor *Code Design* (Avans, 2025). De wereld bestaat uit **kamers, items en deuren** en wordt **data‑gedreven** ingeladen uit **JSON-levels**.  
**Status:** Coursework (Archived) · **Rol:** Duo · **Jaar:** 2025

## Tech stack
C# · .NET 8 · Console I/O · System.Text.Json

## Highlights
- **Kamers & beweging** – speler ziet steeds de huidige kamer en beweegt met pijltjestoetsen.
- **Items & effecten** – o.a. traps (damage/verdwijnen), keys, Sankara stones.
- **Deuren & regels** – o.a. *colored*, *toggle*, *closing gate*, *open on odd*, *open on stones in room*.
- **Data‑gedreven** – level(s) worden uit **JSON** geladen; ontwerp is uitbreidbaar (bijv. XML loader).

## Demo
![Gameplay](docs/demo.gif)

## Demo (tekst)
- **Besturing:** pijltjestoetsen / WASD
- **Doel:** verzamel 5 *Sankara stones* → win; 0 levens → game over.

## Snel starten
> Vereisten: **.NET 8 SDK**

```bash
git clone https://github.com/FreekStraten/temple-of-doom-csharp-2025.git
cd temple-of-doom-csharp-2025

# Build
dotnet build

# Run (console UI)
dotnet run --project TempleOfDoom.Presentation
```

## Projectstructuur
```
TempleOfDoom.sln
TempleOfDoom.BusinessLogic/   # Domein & game‑logica (beweging, items, deuren)
TempleOfDoom.DataAccess/      # Level loaders (JSON; extensible naar XML/…)
TempleOfDoom.Presentation/    # Console UI (enige laag die Console aanspreekt)
```

## Levelbestand (JSON – voorbeeld)
```json
{
  "rooms": [
    { "id": 1, "width": 5, "height": 5,
      "items": [ { "type": "sankara stone", "x": 2, "y": 2 } ]
    }
  ],
  "connections": [
    { "from": 1, "dir": "EAST", "to": 2,
      "door": [ { "type": "colored", "color": "red" } ]
    }
  ],
  "player": { "startRoomId": 1, "startX": 2, "startY": 2, "lives": 3 }
}
```

## Ontwerpkeuzes
- **Layered**: Presentation (Console) · BusinessLogic (domain) · DataAccess (loaders). Alleen de UI‑laag schrijft naar Console.
- **Strategy** voor data‑loaders (JSON nu, later uitbreidbaar met XML).
- **Decorator/Composite** om deur‑eigenschappen te combineren (bijv. *colored* + *closing gate*).

## Refactor‑backlog
- Services **ontkoppelen** (vermijd deep nesting en lange `switch`‑ketens).
- **Inappropriate intimacy** verminderen: logica naar domeinobjecten verplaatsen.
- **IceTile** implementeren (nu placeholder).
- Unit tests toevoegen voor movement, door‑rules en item‑interactie.

## Credits
Gemaakt in **duo/solo** voor het vak **Code Design** (Avans Hogeschool), 2025.
