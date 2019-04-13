using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonrake
{
    public class MoonRakeItems
    {
        public string DullBronzeKey { get; private set; }

        public string RedGreenLight { get; private set; }

        public MoonRakeItems(MoonrakeGameData gameData)
        {
            DullBronzeKey = gameData.AddItem(new GameEngine.Item("Dull Bronze Key") { IsUnique = true });

            //RedGreenLight = gameData.AddItem(new ItemRedGreenLight());
        }
    }
}
