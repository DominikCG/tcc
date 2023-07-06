using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private bool hasCollided = false; // Variável de controle
    private bool isOverlaping = false;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasCollided && collision.collider.CompareTag("Player"))
        {
            hasCollided = true; // Marcar como colidido
            player.GetComponent<PlayerMovement>().Warp(new Vector2(player.transform.position.x,this.transform.position.y+1.5f));
            // Chamar a função EndLevel do GameManager
            GameManager.Instance.EndLevel(true);
        }
    }
}
