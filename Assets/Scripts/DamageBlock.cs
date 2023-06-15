using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBlock : MonoBehaviour
{
    [SerializeField] private PlayerHealth player;
    public int dmg = 1; // Dano por segundo
    public float contactTimeThreshold = 3f; // Tempo de contato necessário para tomar dano
    private float contactTime = 0f; // Tempo atual de contato
    private bool isContacting = false; // Flag para verificar se está em contato

    private void FixedUpdate()
    {
        if (isContacting)
        {
            contactTime += Time.deltaTime; // Incrementa o tempo de contato

            if (contactTime >= contactTimeThreshold)
            {
                ApplyDamage();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerHealth>();
            isContacting = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isContacting = false;
            contactTime = 0f;
        }
    }

    private void ApplyDamage()
    {
        contactTime = 0;
        player.TakeDamage(dmg);
    }
}
