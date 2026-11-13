# LoupixDeck.Plugin.Chronos

LoupixDeck plugin for everything time-related: moon phase, time in any
timezone, date, day of week, week number, day of year, and countdowns.

## Commands

All commands appear under the **Chronos** group in the command picker.

| Command | Shows | Parameters |
|---|---|---|
| `Chronos.Moon` — Moon phase (image) | Photographic moon | — |
| `Chronos.MoonText` — Moon phase (text) | `Waxing Gibbous 87%` | — |
| `Chronos.Time` — Time in timezone | `NYC 17:35` | timezone, label, format |
| `Chronos.Date` — Date | `Nov 13` | format |
| `Chronos.DayOfWeek` — Day of week | `Thursday` | format (full/short) |
| `Chronos.Week` — Week number | `Week 46` | — |
| `Chronos.DayOfYear` — Day of year | `Day 317 of 365` | — |
| `Chronos.Countdown` — Days until date | `Christmas\n42 days` | month, day, label |

The timezone, format, and month parameters are dropdowns (enum-backed). The
countdown rolls to the next year automatically and clamps Feb 29 in non-leap
years.

## Moon images

The 30 frames in `images/` are NASA **Dial-A-Moon** images (Moon Phase and
Libration, 2026 — SVS ID 5587), public domain as US government work. They are
embedded in the DLL as resources.

## Build & install

```bash
dotnet build -c Release
cp bin/Release/LoupixDeck.Plugin.Chronos.dll \
   bin/Release/LoupixDeck.Plugin.Chronos.deps.json \
   bin/Release/plugin.json \
   ~/.config/LoupixDeck/plugins/chronos/
```

Then restart LoupixDeck.

## License

- **Code**: MIT — see [LICENSE](LICENSE).
- **Moon images** (`images/`): NASA **Dial-A-Moon** (Moon Phase and Libration,
  2026, SVS ID 5587), public domain as US government work. Credit to NASA's
  Scientific Visualization Studio is appreciated but not required.
