using Leopotam.Ecs;
using Source.Arena.Components;
using Source.Arena.Systems;
using Source.Arena.UnityComponents;
using UnityEngine;

namespace Source.Arena
{
    sealed class ArenaStartup : MonoBehaviour
    {
        EcsWorld _world;
        EcsSystems _systems;

        private void Start()
        {
            var mainCamera = Camera.main;
            var prefabProvider = FindObjectOfType<PrefabProvider>();
            var remotePlayerInstantiationProvider = FindObjectOfType<RemotePlayerInstantiationProvider>();
            var nicknameDisplay = FindObjectOfType<NicknameDisplay>();

            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif
            _systems
                .Add(new PlayerInstanceControlSystem())
                .Add(new CameraControlSystem())
                .Add(new UserInputSystem())
                .Add(new PlayerMovementSystem())
                .Add(new PlayerCooldownSystem())
                .Add(new PlayerShootSystem())
                .Add(new NicknameDisplaySystem())
                .Add(new GameStartSystem())
                .Add(new ScoreboardSystem())
                
                .OneFrame<PlayerInput>()
                .OneFrame<PlayerAddedEvent>()
                
                .Inject(mainCamera)
                .Inject(prefabProvider)
                .Inject(remotePlayerInstantiationProvider)
                .Inject(nicknameDisplay)
                
                .Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        public void OnDestroy()
        {
            if (_systems == null) return;
            _systems.Destroy();
            _systems = null;
            _world.Destroy();
            _world = null;
        }
    }
}