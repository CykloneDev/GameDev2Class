using UnityEngine;
using System.Collections.Generic;

public class MiniMap : MonoBehaviour
{
    public Transform player;

    Vector3 position;

    void Update()
    {
        miniMap();
    }

    void miniMap()
    {
        position = player.position;
        position.y = transform.position.y;
        transform.position = position;
    }
}
