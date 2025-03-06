using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Color Color = Color.white;
    public float radius = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
