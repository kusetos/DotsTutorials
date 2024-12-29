using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public event EventHandler OnSelectinoAreaStart;
    public event EventHandler OnSelectinoAreaEnd;
    private Vector2 selectionStartMousePosition;
    //private Vector2 selectionEndMousePosition;

    public static UnitSelectionManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionStartMousePosition = Input.mousePosition;
            OnSelectinoAreaStart?.Invoke(this, EventArgs.Empty);

        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 selectionEndMousePosition = Input.mousePosition;
            OnSelectinoAreaEnd?.Invoke(this, EventArgs.Empty);

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = 
                new EntityQueryBuilder(Allocator.Temp).
                WithAll<LocalTransform, Unit>().
                WithPresent<Selected>().
                Build(entityManager);

            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            for(int i = 0; i < entityArray.Length; i++)
            {
                entityManager.SetComponentEnabled<Selected>(entityArray[i], false);
            }


            Rect selectionAreaRect = GetSelectionArea();
            float selectionAreaSize = selectionAreaRect.width + selectionAreaRect.height;
            float multipleSelectionSizeMin = 40f;
            bool isMultipleSelection = selectionAreaSize > multipleSelectionSizeMin;
            Debug.Log(isMultipleSelection + " " + selectionAreaSize);

            if (isMultipleSelection)
            {
                entityArray = entityQuery.ToEntityArray(Allocator.Temp);

                NativeArray<LocalTransform> localTransformArray =
                    entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
                for(int i = 0; i < entityArray.Length; i++)
                {
                    LocalTransform unitLocalTransform = localTransformArray[i];
                    Vector2 unitScreenPosition = Camera.main.WorldToScreenPoint(unitLocalTransform.Position);
                    if(GetSelectionArea().Contains(unitScreenPosition))
                    {
                        entityManager.SetComponentEnabled<Selected>(entityArray[i], true);
                    }
                }

            }
            else
            {
                entityQuery = entityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton));
                PhysicsWorldSingleton physicsWorldSingleton = entityQuery.GetSingleton<PhysicsWorldSingleton>();
                CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
                UnityEngine.Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                int unitLayer = 6;
                RaycastInput raycastInput = new RaycastInput
                {
                    Start = cameraRay.GetPoint(0f),
                    End = cameraRay.GetPoint(10000f),
                    Filter = new CollisionFilter
                    {
                        BelongsTo = ~0u,
                        CollidesWith = 1u << unitLayer,
                        GroupIndex = 0,
                       

                    }
                };
                if(collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit raycastHit))
                {
                    if (entityManager.HasComponent<Unit>(raycastHit.Entity))
                    {
                        entityManager.SetComponentEnabled<Selected>(raycastHit.Entity, true);
                    }
                }
            }


        }



        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMover, Selected>().Build(entityManager);

            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<UnitMover> unitMoverArray = entityQuery.ToComponentDataArray<UnitMover>(Allocator.Temp);

            for(int i = 0; i < unitMoverArray.Length; i++)
            {
                UnitMover unitMover = unitMoverArray[i];
                unitMover.targetPosition = mouseWorldPosition;
                unitMoverArray[i] = unitMover;
                //entityManager.SetComponentData(entityArray[i], unitMover);

            }
            entityQuery.CopyFromComponentDataArray(unitMoverArray);

/*            foreach (var unitMover in unitMoverArray)
            {
                UnitMover unit = unitMover;
                unit.targetPosition = mouseWorldPosition;
            }*/
        }
    }
    public Rect GetSelectionArea()
    {
        Vector2 selectionEndMousePosition = Input.mousePosition;
        Vector2 lowerLeftCorner = new Vector2(
            Mathf.Min(selectionStartMousePosition.x, selectionEndMousePosition.x),
            Mathf.Min(selectionStartMousePosition.y, selectionEndMousePosition.y)
            );  
        Vector2 upperRightCorner = new Vector2(
            Mathf.Max(selectionStartMousePosition.x, selectionEndMousePosition.x),
            Mathf.Max(selectionStartMousePosition.y, selectionEndMousePosition.y)
            );
        return new Rect(
            lowerLeftCorner.x,
            lowerLeftCorner.y,
            upperRightCorner.x - lowerLeftCorner.x,
            upperRightCorner.y - lowerLeftCorner.y
            );
    }
}

