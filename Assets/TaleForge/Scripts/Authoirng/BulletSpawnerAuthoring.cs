using Unity.Entities;
using UnityEngine;

namespace TaleForge
{
    public class BulletSpawnerAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private int _bulletCount;

        // public uint Seed => _seed; 
        public uint InitialIndex => (uint)new System.Random().Next(0, (System.Int32.MaxValue));
        public GameObject BulletPrefab => _bulletPrefab;
        public int BulletCount => _bulletCount;
    }
    public class BulletSpawnerBaker : Baker<BulletSpawnerAuthoring>
    {
        public override void Bake(BulletSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new BulletSpawnerComponentData
            {
                
                Instance = Unity.Mathematics.Random.CreateFromIndex(authoring.InitialIndex),
                BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic),
                BulletCount = authoring.BulletCount,
            });
        }
    }
}
