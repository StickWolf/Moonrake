using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonrake
{
    public class MoonRakeGameVariables
    {
        public string BanquetElevatorFloor { get; private set; }
        public string BanquetSecretHallOpen { get; private set; }

        public string RedGreenLightColor { get; private set; }

        public MoonRakeGameVariables(MoonrakeGameData gameData)
        {
            BanquetElevatorFloor = gameData.AddDefaultGameVar("BanquetElevatorFloor", "1");
            BanquetSecretHallOpen = gameData.AddDefaultGameVar("BanquetSecretHallOpen", "false");

            RedGreenLightColor = gameData.AddDefaultGameVar("RedGreenLightColor", "red");
        }
    }
}
