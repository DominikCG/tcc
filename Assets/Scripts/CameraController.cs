using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float camYOffset = 2f;
    [SerializeField] private float followSpeed = 5f;

    private void Awake()
    {
        GameObject pl = GameObject.FindGameObjectWithTag("Player");
        player = pl.GetComponent<Transform>();
    }
    private void Update()
    {
        Vector3 targetPosition = new Vector3(0, player.position.y + camYOffset, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
