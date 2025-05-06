using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objetivo;
    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (objetivo != null)
        {
            transform.position = objetivo.position + offset;
        }
    }
}