using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct SelectedVisualSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var seleted in SystemAPI.Query<RefRO<Selected>>())
        {
            RefRW<LocalTransform> localTransform =
                SystemAPI.GetComponentRW<LocalTransform>(seleted.ValueRO.visualEntity);
            localTransform.ValueRW.Scale = seleted.ValueRO.showScale;
        }
        foreach (var seleted in SystemAPI.Query<RefRO<Selected>>().WithDisabled<Selected>())
        {
            RefRW<LocalTransform> localTransform =
                SystemAPI.GetComponentRW<LocalTransform>(seleted.ValueRO.visualEntity);
            localTransform.ValueRW.Scale = 0f;
        }

    }

}
