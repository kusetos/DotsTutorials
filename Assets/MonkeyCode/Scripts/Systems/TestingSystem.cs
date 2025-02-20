using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

partial struct TestingSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState systemState)
    {
        int unitCount = 0;
        float deltaTime = SystemAPI.Time.DeltaTime;
        foreach(var(localTransform, unitMover, physicsVelocity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<UnitMover>, RefRW<PhysicsVelocity>>().WithPresent<Selected>())
        {
            /*            float3 moveDirection = unitMover.ValueRO.targetPosition - localTransform.ValueRO.Position;

                        float reachedTargetDistanceSq = 2f;
                        if (math.lengthsq(moveDirection) < reachedTargetDistanceSq)
                        {
                            physicsVelocity.ValueRW.Linear = float3.zero;
                            physicsVelocity.ValueRW.Angular = float3.zero;
                            return;
                        }
                        moveDirection = math.normalize(moveDirection);

                        localTransform.ValueRW.Rotation =
                        math.slerp(localTransform.ValueRO.Rotation, quaternion.LookRotation(moveDirection, math.up())
                        , deltaTime * unitMover.ValueRO.rotationSpeed);


                        physicsVelocity.ValueRW.Linear = moveDirection * unitMover.ValueRO.moveSpeed;
                        physicsVelocity.ValueRW.Angular = float3.zero;*/

            unitCount++;
        }
        //Debug.Log(unitCount);
            
    }
}