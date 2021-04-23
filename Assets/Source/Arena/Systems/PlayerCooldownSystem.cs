using Leopotam.Ecs;
using Source.Arena.Components;
using UnityEngine;

namespace Source.Arena.Systems
{
    public class PlayerCooldownSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerShootCooldown> _cooldown = null;

        public void Run()
        {
            foreach (var i in _cooldown)
            {
                var entity = _cooldown.GetEntity(i);
                ref var cooldown = ref entity.Get<PlayerShootCooldown>();
                cooldown.TimeLeft -= Time.deltaTime;
                if (cooldown.TimeLeft <= 0)
                    entity.Del<PlayerShootCooldown>();
            }
        }
    }
}