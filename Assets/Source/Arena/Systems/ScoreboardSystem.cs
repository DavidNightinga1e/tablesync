using Leopotam.Ecs;
using Photon.Pun;
using Source.Arena.Components;

namespace Source.Arena.Systems
{
    public class ScoreboardSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerComponent> _players = null;
        
        public void Run()
        {
            
        }
    }
}