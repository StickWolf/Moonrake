using ServerEngine.Characters;
using System;
using System.Collections.Generic;

namespace ServerEngine
{
    public static class Console // TODO: these methods should be removed from the server layer
    {
        public static void Clear()
        {
            System.Console.Clear();
        }

        public static ConsoleKeyInfo ReadKey()
        {
            return System.Console.ReadKey();
        }

        public static string ReadLine()
        {
            return System.Console.ReadLine();
        }
    }
}
