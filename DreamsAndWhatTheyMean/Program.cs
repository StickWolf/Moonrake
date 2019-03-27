using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamsAndWhatTheyMean
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineFactory.Start(CreateDragonKittyGameData);
        }

        static GameSourceDataBase CreateDragonKittyGameData()
        {
            return new TheTaleOfTheDragonKittyGameData();
        }
    }
}
