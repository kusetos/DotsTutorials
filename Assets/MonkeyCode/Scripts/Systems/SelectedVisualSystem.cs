using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateBefore(typeof(ResetEventsSystem))]
partial struct SelectedVisualSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var seleted in SystemAPI.Query<RefRO<Selected>>().WithPresent<Selected>())
        {

            if(seleted.ValueRO.onSelected)
            {
                UnityEngine.Debug.Log("OnSelected");
                RefRW<LocalTransform> localTransform =
                    SystemAPI.GetComponentRW<LocalTransform>(seleted.ValueRO.visualEntity);
                localTransform.ValueRW.Scale = seleted.ValueRO.showScale;
                
            }          
            if(seleted.ValueRO.onDeselected)
            {
                UnityEngine.Debug.Log("OnDeselected");
                RefRW<LocalTransform> localTransform =
                    SystemAPI.GetComponentRW<LocalTransform>(seleted.ValueRO.visualEntity);
                localTransform.ValueRW.Scale = 0f;
            }
        }
    }

}
