using Unity.Mathematics;
using UnityEngine;

public class MouseWorldPosition : MonoBehaviour
{
    public static MouseWorldPosition Instance { get; private set; }
    private void Start()
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
    public Vector3 GetPosition()
    {
        Ray mouseCameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, float3.zero);
        if(plane.Raycast(mouseCameraRay, out float distance))
        {
            return mouseCameraRay.GetPoint(distance);
        }
        else
        {
            return Vector3.zero;
        }

    }
}
