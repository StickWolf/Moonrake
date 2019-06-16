using ServerEngine;
using Newtonsoft.Json;
using ServerEngine.GrainSiloAndClient;

namespace ExampleGameServer.Items
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ColoredLight : Item
    {
        [JsonProperty]
        private string GameVariableColor { get; set; }

        public ColoredLight(string gameVariableColor) : base("Colored light")
        {
            GameVariableColor = gameVariableColor;
            IsUnique = false;
            IsBound = true;
        }

        public override string GetDescription(int count)
        {
            string lightColor = GrainClusterClient.Universe.GetGameVarValue(GameVariableColor).Result ?? "blue";
            return $"a light neatly fastened to the wall that is covered in metal mesh which is glowing {lightColor}";
        }
    }
}
