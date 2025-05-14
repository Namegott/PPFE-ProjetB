using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] int Health;
    [SerializeField] int MaxHealth;
    [SerializeField] bool BlockAll;
    [SerializeField] bool BlockHalf;
    [SerializeField] Coroutine CrBlockTime;
    [SerializeField] Image BlockInfo;

    [SerializeField] bool Invincibility;
    [SerializeField] float InvincibilityTime = 0.5f;
    [SerializeField] TextMeshPro DamageGet;
    [SerializeField] Image HpBar;
    
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
            if (BlockAll)
            {

            }
            else if (BlockHalf) 
            {
                Health -= damage / 2;
                //DamageGet.text = damage.ToString();
                if (gameObject.layer == 7)
                {
                    UpdateLifeHUD();
                }
                StartCoroutine(InvicibilityTimer());
            }
            else
            {
                Health -= damage;
                //DamageGet.text = damage.ToString();
                if (gameObject.layer == 7)
                {
                    UpdateLifeHUD();
                }
                StartCoroutine(InvicibilityTimer());
            }
        }
        
        if (Health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("ded");
        }
    }

    private void UpdateLifeHUD()
    {
        float PercentHelth = (float)Health / (float)MaxHealth;
        HpBar.transform.localScale = new Vector3(PercentHelth, 1, 1);
    }

    public void BlockStart(float durationBlockAll)
    {
        BlockAll = true;
        BlockInfo.enabled = true;
        CrBlockTime = StartCoroutine(BlockTime(durationBlockAll));
    }

    public void BlockEnd()
    {
        StopCoroutine(CrBlockTime);
        BlockInfo.enabled = false;
        BlockAll = false;
        BlockHalf = false;
    }

    public string GetBlock()
    {
        if (BlockAll)
        {
            return "All";
        }
        else if (BlockHalf)
        {
            return "Half";
        }
        else
        {
            return "none";
        }
    }

    IEnumerator BlockTime(float durationBlockAll)
    {
        yield return new WaitForSeconds(durationBlockAll);
        if (BlockAll)
        {
            BlockInfo.enabled = false;
            BlockAll = false;
            BlockHalf = true;
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
