using System.Collections.Generic;
using Simba;

namespace Source.Lobby.Systems
{
    public class LobbySystemRunner : SystemRunner
    {
        protected override List<ISystem> Systems { get; } = new List<ISystem>
        {
            new PhotonInitializationSystem(),
            new VersionDisplaySystem(),
            new PlayerSettingsSystem(),
            new MatchmakingSystem(),
            new ConnectionSystem()
        };
    }
}