using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float camYOffset = 2f;

    private void Update()
    {
        transform.position = new Vector3(0, player.position.y + camYOffset, transform.position.z);
    }
}
