using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3; // Quantidade máxima de corações
    public int currentHealth; // Quantidade atual de corações
    public Image[] hearts; // Array de imagens dos corações na UI Canvas
    private bool canTakeDmg = true;
    public float noDmgTime = 3f;
    private float timer = 0f;
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Update(){
        timer += Time.deltaTime;
        if(timer >= noDmgTime){
            canTakeDmg = true;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if(canTakeDmg){
            canTakeDmg = false;
            timer = 0f;
            currentHealth -= damageAmount;

            // Atualizar a exibição dos corações
            UpdateUI();
        }

        if (currentHealth <= 0)
        {
            // O personagem morreu, faça algo aqui
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        // Garantir que a quantidade de vida não exceda o máximo
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        // Atualizar a exibição dos corações
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Atualizar a exibição dos corações na UI Canvas
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].enabled = true; // Habilita o coração
            }
            else
            {
                hearts[i].enabled = false; // Desabilita o coração
            }
        }
    }
}
