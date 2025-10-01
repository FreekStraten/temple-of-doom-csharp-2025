using System.Collections.Generic;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Helpers;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic.Models.Tile
{
    /// <summary>
    /// IceTile: speler (of enemy) glijdt in dezelfde richting door zolang de volgende tile ijs en toegankelijk is.
    /// Gedrag leeft nu op de tile zelf i.p.v. in een service.
    /// </summary>
    public class IceTile : FloorTile, IAutoMoveTile
    {
        public override string Representation => "~";

        /// <summary>
        /// Lever alle extra posities na de eerste stap op deze IceTile.
        /// Stopt vóór muren, gesloten deuren, of bij niet-ijs.
        /// </summary>
        public IEnumerable<Coordinates> GetAutoMoves(Room room, Coordinates start, Direction direction, Player player)
        {
            var pos = start; // speler staat al op 'start'
            var delta = DirectionHelper.ToCoordinates(direction);

            // Safety om infinite loops te voorkomen
            for (int i = 0; i < 50; i++)
            {
                var next = pos + delta;

                // Buiten kamer? Stop (glijden stopt aan de rand)
                if (next.X < 0 || next.X >= room.Width || next.Y < 0 || next.Y >= room.Height)
                    yield break;

                var nextTile = room.GetTileAt(next);

                // Gesloten deur? Stop vóór de deur
                if (nextTile is DoorTile doorTile)
                {
                    // Deur-tegel zelf is niet walkable; laat alleen door als hij open is,
                    // maar zelfs als open: we laten de daadwerkelijke room-transition
                    // elders afhandelen; hier stoppen we vóór de deur-tegel.
                    // (Consistent met bestaande overgangslogica)
                    if (!doorTile.Door.IsOpen(player, room))
                        yield break;

                    // Deur-tegel blijft on-walkable, dus glijden stopt hier sowieso
                    yield break;
                }

                // Niet-walkable? Stop
                if (!nextTile.IsWalkable)
                    yield break;

                // Move één stap vooruit
                pos = next;
                yield return pos;

                // Als de nieuwe tegel géén ijs is, glijden we niet verder
                if (nextTile is not IceTile)
                    yield break;

                // Anders: doorlussen en nog een stap proberen
            }
        }
    }
}
