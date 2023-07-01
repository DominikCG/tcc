using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private bool hasCollided = false; // Variável de controle

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasCollided && collision.collider.CompareTag("Player"))
        {
            hasCollided = true; // Marcar como colidido

            // Chamar a função EndLevel do GameManager
            GameManager.Instance.EndLevel(true);
        }
    }
}
