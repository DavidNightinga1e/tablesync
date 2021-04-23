using Simba;
using Source.Lobby.Components;
using UnityEngine;

namespace Source.Lobby.Systems
{
    public class VersionDisplaySystem : IStartSystem
    {
        public void Start()
        {
            SimbaComponent.Get<VersionComponent>().Text = $"Version: {Application.version}";
        }
    }
}