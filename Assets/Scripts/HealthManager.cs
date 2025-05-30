using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
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

    [SerializeField] ScoreManager ScoreManager;
    [SerializeField] int ScoreGain;

    [Header("Death Squid")]
    [SerializeField] Material DeadMatSquid;
    [SerializeField] MeshRenderer MeshSquid;

    [Header("Death Centaur")]
    [SerializeField] Material DeadMatBaseCentaur;
    [SerializeField] Material[] DeadMatTorsoCentaur = new Material[2];
    [SerializeField] MeshRenderer MeshTorsoCentaur;
    [SerializeField] MeshRenderer MeshBaseCentaur;

    [Header("Sound")]
    [SerializeField] AudioSource Source;
    [SerializeField] AudioClip[] DamageFull;
    [SerializeField] AudioClip[] DamageHalf;
    [SerializeField] AudioClip[] DamageBlock;
    [SerializeField] float StrengthSoundDamageFull;
    [SerializeField] AudioClip DeathSound;
    [SerializeField] GameObject DeathSoundPlayer;
    [SerializeField] float DeathDelay;

    public int GetHealth()
    { return Health; }
    public int GetMaxHealth() 
    { return MaxHealth;}

    void Start()
    {
        Health = MaxHealth;
        ScoreManager = FindAnyObjectByType<ScoreManager>();
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
                Source.PlayOneShot(DamageBlock[Random.Range(0, DamageBlock.Length)], 0.25f);
            }
            else if (BlockHalf) 
            {
                Health -= damage / 2;

                Source.PlayOneShot(DamageHalf[Random.Range(0, DamageHalf.Length)], 0.25f);

                //DamageGet.text = damage.ToString();
                if (gameObject.layer == 7)
                {
                    ScoreManager.EndCombo();
                    UpdateLifeHUD();
                }
                StartCoroutine(InvicibilityTimer());
            }
            else
            {
                Health -= damage;
                //DamageGet.text = damage.ToString();

                //Debug.Log(soundStrength);
                Source.PlayOneShot(DamageFull[Random.Range(0, DamageFull.Length)], StrengthSoundDamageFull);

                if (gameObject.layer == 7)
                {
                    ScoreManager.EndCombo();
                    UpdateLifeHUD();
                }
                StartCoroutine(InvicibilityTimer());
            }
        }
        
        if (Health <= 0)
        {
            Debug.Log("health = 0");
            if (gameObject.layer == 6)
            {
                ScoreManager.AddKillScore(ScoreGain);
                DeathEnnmi();
            }
            if (gameObject.layer == 7)
            {
                DeathPlayer();
            }
        }
    }

    void DeathPlayer()
    {
        FindAnyObjectByType<Finish>().FinishGame("YOU DIED !");
        Debug.Log("ded");
    }

    void DeathEnnmi()
    {
        GameObject go = Instantiate(DeathSoundPlayer, gameObject.transform);
        go.GetComponent<EnnemiDeathSound>().Play(DeathSound);

        if (DeadMatSquid != null)
        {
            MeshSquid.material = DeadMatSquid;
        }
        else if (DeadMatBaseCentaur != null)
        {
            MeshBaseCentaur.material = DeadMatBaseCentaur;
            MeshTorsoCentaur.materials = DeadMatTorsoCentaur;
        }

        Destroy(GetComponent<EnnemiBase>());
        Destroy(GetComponent<BoxCollider>());
        GetComponent<AutoDestroy>().Delay = DeathDelay;
        GetComponent<AutoDestroy>().enabled = true;

        Destroy(this);
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
