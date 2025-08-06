using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth = 100f;

    public float damageAmount = 20;
    [SerializeField] private Image healthBar;

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

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        HealthBarFill();
        HealthColour();
    }


    private void HealthBarFill()
    {
        //healthBar.fillAmount = health / maxHealth;
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, lerpSpeed);
    }

    private void HealthColour()
    {
        //Color healthColur = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        Color healthColur = Color.Lerp(lowHealthColour, fullHealthColour, (health / maxHealth));
        healthBar.color = healthColur;
    }


    private void OnCollisionEnter(Collision coli)
    {
        if (coli.gameObject.CompareTag("Tongue"))
        {
            //damageAmount[0] will be the health that the dragon fly deals
            health -= damageAmount;
        }
    }
}
