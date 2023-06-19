using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null){
            BoxCollider2D playerCollider = player.GetComponent<BoxCollider2D>();
            if(this.GetComponent<BoxCollider2D>().bounds.Intersects(playerCollider.bounds)){
                player.GetComponent<PlayerHealth>().TakeDamage(1);
                player.GetComponent<PlayerMovement>().Warp(new Vector2(player.transform.position.x,this.transform.position.y+1));
            }

        }

    }

}
