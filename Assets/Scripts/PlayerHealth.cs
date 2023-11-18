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
    private SpriteRenderer spriteRenderer;
    public float taxaDePiscar = 0.1f; // Intervalo entre os piscar
    private bool spriteVisivel = true;

    private void Start()
    {
        // Obtém a referência do componente SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Trap"))
        {
            TakeDamage(1);
        }
    }

    public void Update(){
        timer += Time.deltaTime;
        if(timer >= noDmgTime){
            canTakeDmg = true;
            spriteRenderer.enabled = true;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if(canTakeDmg){
            AudioManager.Instance.PlayAudio(AudioClipID.Damage);
            canTakeDmg = false;
            timer = 0f;
            currentHealth -= damageAmount;
            InvokeRepeating("Piscar", 0f, taxaDePiscar);
            // Atualizar a exibição dos corações
            UpdateUI();
        }

        if (currentHealth <= 0)
        {
            // O personagem morreu, faça algo aqui
            GameManager.Instance.EndLevel(false);
        }
    }

    void Piscar()
    {
        if (!canTakeDmg)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
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
