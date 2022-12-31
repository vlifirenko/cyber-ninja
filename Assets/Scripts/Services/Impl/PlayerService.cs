using System;
using CyberNinja.Ecs.Components.Unit;
using Leopotam.EcsLite;

namespace CyberNinja.Services.Impl
{
    public class PlayerService : IPlayerService
    {
        private readonly EcsWorld _world;
        private readonly EcsPool<PlayerComponent> _playerPool;
        private readonly EcsFilter _playerFilter;

        public PlayerService(EcsWorld world)
        {
            _world = world;
            _playerFilter = _world.Filter<PlayerComponent>().End();
            _playerPool = _world.GetPool<PlayerComponent>();
        }

        public Controls GetInput()
        {
            var player = _playerPool.Get(GetEntity());
            return player.Controls;
        }

        public int GetEntity()
        {
            foreach (var entity in _playerFilter)
                return entity;

            throw new Exception("Player entity not found");
        }
    }
}