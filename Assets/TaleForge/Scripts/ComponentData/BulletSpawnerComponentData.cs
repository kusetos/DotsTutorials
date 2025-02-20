using Unity.Entities;
using Unity.Mathematics;

namespace TaleForge
{
    public struct BulletSpawnerComponentData : IComponentData
    {
        public Random Instance;
        public int BulletCount;
        public Entity BulletPrefab;
    }
    
}
