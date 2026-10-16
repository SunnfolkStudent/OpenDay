using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;

    private void LateUpdate()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(target.position.x, target.position.y, -10);
    }
}
