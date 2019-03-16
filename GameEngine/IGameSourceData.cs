using System.Collections.Generic;

namespace GameEngine
{
    public interface IGameSourceData
    {
        string DefaultPlayerName { get; }

        string GameIntroductionText { get; }

        List<Character> Characters { get; }
    }
}
