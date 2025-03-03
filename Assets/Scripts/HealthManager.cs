using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] int Health;
    [SerializeField] int MaxHealth;

    [SerializeField] bool Invincibility;
    [SerializeField] float InvincibilityTime = 0.5f;
    [SerializeField] TextMeshPro DamageGet;
    
    void Start()
    {
        Health = MaxHealth;
    }

    // Permet de setup les hp de chacun
    public void SetStartHealth(int health)
    {
        MaxHealth = health;
        Health = MaxHealth;
    }

    // Permet de subir des dégats
    public void Damage(int damage)
    {
        if (!Invincibility) 
        {
            Health -= damage;
            DamageGet.text = damage.ToString();
            StartCoroutine(InvicibilityTimer());
        }
        
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator InvicibilityTimer()
    {
        Invincibility = true;
        yield return new WaitForSeconds(InvincibilityTime);
        Invincibility = false;
        //DamageGet.text = "";
    }
}
