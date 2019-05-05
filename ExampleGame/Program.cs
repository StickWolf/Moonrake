﻿using GameEngine;

namespace ExampleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineFactory.Start(() => new ExampleGameSourceData().NewGame());
        }
    }
}
