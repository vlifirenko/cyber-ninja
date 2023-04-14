using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Room
{
    public class InitRoomsSystem : IEcsInitSystem
    {
        private EcsCustomInject<SceneView> _sceneView;

        public void Init(IEcsSystems systems)
        {
            var prefab = _sceneView.Value.RoomPrefab;
            var container = _sceneView.Value.RoomContainer;

            var instance = Object.Instantiate(prefab, container);
            var position = Vector3.zero;

            instance.Transform.position = position;
            instance.Circle = EMineCircle.Core;

            for (var i = 0; i < 8; i++)
            {
                float xOffset = 0, zOffset = 0;
                
                instance = Object.Instantiate(prefab, container);
                position = Vector3.zero;
                
                switch (i)
                {
                    case 0:
                        xOffset = -40f;
                        zOffset = 40f;
                        break;
                    case 1:
                        xOffset = 0f;
                        zOffset = 40f;
                        break;
                    case 2:
                        xOffset = 40f;
                        zOffset = 40f;
                        break;
                    case 3:
                        xOffset = -40f;
                        zOffset = 0f;
                        break;
                    case 4:
                        xOffset = 40f;
                        zOffset = 0f;
                        break;
                    case 5:
                        xOffset = -40f;
                        zOffset = -40f;
                        break;
                    case 6:
                        xOffset = 0f;
                        zOffset = -40f;
                        break;
                    case 7:
                        xOffset = 40f;
                        zOffset = -40f;
                        break;
                }

                position.x = xOffset;
                position.z = zOffset;
                instance.Transform.position = position;
                instance.Circle = EMineCircle.Inner;
                instance.Index = i;
            }
        }
    }
}