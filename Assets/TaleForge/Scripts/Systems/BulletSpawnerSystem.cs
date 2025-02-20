using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TaleForge
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct BulletSpawnerSystem : ISystem
    {
        private bool _isEnabled;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _isEnabled = false;
            state.RequireForUpdate<BulletSpawnerComponentData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if(_isEnabled) return;

            _isEnabled = true;

            Entity bulletSpawnerEntity = 
                SystemAPI.GetSingletonEntity<BulletSpawnerComponentData>();

            BulletSpawnerAspect bulletSpawnerAspect = 
                SystemAPI.GetAspect<BulletSpawnerAspect>(bulletSpawnerEntity);

            for(int i = 0; i < bulletSpawnerAspect.BulletCount; i++)
            {
                float3 randomPosition = bulletSpawnerAspect.GetRandomSpawnPosition();
                Entity bulletEntity = state.EntityManager.Instantiate(bulletSpawnerAspect.BulletPrefab);
                state.EntityManager.SetComponentData(bulletEntity, LocalTransform.FromPosition(randomPosition));
            }
        }
    }
}
