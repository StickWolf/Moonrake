﻿using GameEngine;

namespace Moonrake
{
    public class MoonRakeGameVariables
    {
        public string IceCreamShopDoor { get; private set; }

        public void NewGame(MoonrakeGameData gameData)
        {
            IceCreamShopDoor = GameState.CurrentGameState.SetGameVarValue("IceCreamShopDoor", "open");
        }
    }
}
