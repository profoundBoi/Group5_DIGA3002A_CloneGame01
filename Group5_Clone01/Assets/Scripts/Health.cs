using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth = 100f;

    public float[] damageAmount;
    [SerializeField] Image healthBar;
    [SerializeField] Image heart;

    public Color fullHealthColour;
    public Color lowHealthColour;

    private float lerpSpeed;


    private void Start()
    {
        health = maxHealth;
        
    }

    private void Update()
    {
        lerpSpeed = 3 * Time.deltaTime;

        if(health <= 0)
        {
            GameRespawnManager.Instance.RespawnAllPlayers();
        }
        HealthBarFill();
        HealthColour();
    }

    
    private void HealthBarFill()
    {
        //healthBar.fillAmount = health / maxHealth;
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health/maxHealth, lerpSpeed);
    }

    private void HealthColour()
    {
        //Color healthColur = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        Color healthColur = Color.Lerp(lowHealthColour, fullHealthColour, (health / maxHealth));
        healthBar.color = healthColur;
        heart.color = healthColur;
    }

    private void OnCollisionEnter(Collision coli)
    {
        if (coli.gameObject.CompareTag("Dragonfly"))
        {
            //damageAmount[0] will be the health that the dragon fly deals
            health -= damageAmount[0];
        }

        if (coli.gameObject.CompareTag("Rock"))
        {
            //damageAmount[1] will be the health that the rock deals
            health -= damageAmount[1];
        }

        if (coli.gameObject.CompareTag("Snake"))
        {
            //damageAmount[2] will be the health that the snake deals
            health -= damageAmount[1];
            
        }
    }

    
  
}
