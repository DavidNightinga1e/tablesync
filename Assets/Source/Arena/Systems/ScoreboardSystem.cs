using Leopotam.Ecs;
using Source.Arena.Components;

namespace Source.Arena.Systems
{
    public class ScoreboardSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerComponent, PlayerHitEvent, LocalPlayerTag> _localPlayerHit = null;
        private readonly EcsFilter<PlayerComponent, PlayerHitEvent, RemotePlayerTag> _remotePlayerHit = null;

        private readonly EcsFilter<PlayerComponent, LocalPlayerTag> _localPlayer = null;
        private readonly EcsFilter<PlayerComponent, RemotePlayerTag> _remotePlayer = null;

        private readonly NicknameDisplay _nicknameDisplay = null;

        public void Run()
        {
            if (!_localPlayerHit.IsEmpty())
            {
                var localPlayerEntity = _localPlayerHit.GetEntity(0);
                localPlayerEntity.Del<PlayerHitEvent>();

                _remotePlayerHit.Get1(0).Score += 1;
            }
            
            if (!_remotePlayerHit.IsEmpty())
            {
                var remotePlayerEntity = _remotePlayerHit.GetEntity(0);
                remotePlayerEntity.Del<PlayerHitEvent>();

                _localPlayerHit.Get1(0).Score += 1;
            }

            if (!_localPlayer.IsEmpty())
                _nicknameDisplay.LocalScore = _localPlayer.Get1(0).Score.ToString();
            if (!_remotePlayer.IsEmpty())
                _nicknameDisplay.RemoteScore = _remotePlayer.Get1(0).Score.ToString();
        }
    }
}