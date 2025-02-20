using System;
using Unity.Entities;
using Unity.Mathematics;
using Random =  Unity.Mathematics.Random;

namespace TaleForge
{
    public readonly partial struct BulletSpawnerAspect : IAspect
    {
        private readonly RefRW<BulletSpawnerComponentData> _bulletSpawnerComponentData;
        public Entity BulletPrefab => _bulletSpawnerComponentData.ValueRW.BulletPrefab;
        public int BulletCount => _bulletSpawnerComponentData.ValueRW.BulletCount;
        public float3 GetRandomSpawnPosition()
        {
            uint index = _bulletSpawnerComponentData.ValueRW.Instance.NextUInt(UInt32.MaxValue);
            _bulletSpawnerComponentData.ValueRW.Instance = Random.CreateFromIndex(index);
            float randomPositionY = _bulletSpawnerComponentData.ValueRW.Instance.NextFloat(-10, 10);
            float randomPositionX = _bulletSpawnerComponentData.ValueRW.Instance.NextFloat(-10, 10);
            return new float3(randomPositionX, randomPositionY, 0); 
        }   
    }
}
