﻿using System.Collections.Generic;

namespace GameEngine.Commands
{
    public interface ICommand
    {
        bool IsActivatedBy(string word);

        void Exceute(GameSourceData gameData, List<string> extraWords);
    }
}
