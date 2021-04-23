using Leopotam.Ecs;
using Source.Arena.Components;

namespace Source.Arena.Systems
{
    public class GameStartSystem : IEcsRunSystem
    {
        private readonly EcsFilter<GameStartedEvent> _gameStart = null;
        private readonly EcsFilter<PlayerComponent> _players = null;

        public void Run()
        {
            if (_gameStart.IsEmpty())
                return;

            var entity = _gameStart.GetEntity(0);
            entity.Del<GameStartedEvent>();

            foreach (var i in _players)
            {
                ref var playerComponent = ref _players.Get1(i);
                var playerBehaviourTransform = playerComponent.PlayerBehaviour.transform;
                playerBehaviourTransform.position = playerComponent.IsBlue
                    ? GameRules.BlueSpawn
                    : GameRules.OrangeSpawn;
                playerComponent.Health = GameRules.Health;
            }
        }
    }
}