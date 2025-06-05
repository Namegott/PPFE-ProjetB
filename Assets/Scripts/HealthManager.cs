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
    [SerializeField] Image OldHpBar;
    [SerializeField] GameObject BloodParticles;
    [SerializeField] GameObject RotationReference;

    [SerializeField] MeshRenderer[] GorillaMesh;
    [SerializeField] Material GorillaFullBlock;
    [SerializeField] Material GorillaHalfBlock;

    [Header("Damage Material Flicker")]
    float FlickerTime = 0.15f / 5;
    [SerializeField] Material BaseMaterialSquid;
    [SerializeField] Material BaseMaterialCentaurBase;
    [SerializeField] Material BaseMaterialPlayer;
    [SerializeField] Material[] BaseMaterialCentaurTorso, DamageMaterial;
    [SerializeField] MeshRenderer[] VisualsSquid, VisualCentaurTorso, VisualCentaurBase;
    [SerializeField] SkinnedMeshRenderer[] VisualsPlayers;

    [Header("Score")]
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

    void Update()
    {
        if (HpBar != null)
        {
            if (HpBar.transform.localScale.x < OldHpBar.transform.localScale.x)
            {
                OldHpBar.transform.localScale = new Vector3(OldHpBar.transform.localScale.x - 0.001f, 1, 1);
            }
            else if (HpBar.transform.localScale.x > OldHpBar.transform.localScale.x)
            {
                OldHpBar.transform.localScale = HpBar.transform.localScale;
            }
        }
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
                //ScoreManager.AddCombo();
            }
            else if (BlockHalf) 
            {
                Health -= damage / 2;

                Source.PlayOneShot(DamageHalf[Random.Range(0, DamageHalf.Length)], 0.25f);

                //DamageGet.text = damage.ToString();
                if (gameObject.layer == 7)
                {
                    StartCoroutine(MaterialFlicker());
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
                    StartCoroutine(MaterialFlicker());
                    ScoreManager.EndCombo();
                    UpdateLifeHUD();
                }
                else if (gameObject.layer == 6)
                {
                    StartCoroutine(MaterialFlicker());
                    //j'arrive pas a faire rotate la projection de particule a l'opposé de la d'ou vient le coup
                    /*Vector3 damageOrigin = new Vector3(FindFirstObjectByType<VisualManager>().gameObject.transform.position.x, transform.position.y, FindFirstObjectByType<VisualManager>().gameObject.transform.position.z);
                    Quaternion rota = Quaternion.LookRotation(damageOrigin);

                    Debug.Log("origine degats : " + damageOrigin + " / rotation particules : " + rota.eulerAngles);*/
                    
                    //Vector3 rota = Vector3.RotateTowards(transform.position, new Vector3(FindFirstObjectByType<VisualManager>().gameObject.transform.position.x, transform.position.y, FindFirstObjectByType<VisualManager>().gameObject.transform.position.z), 1,1);
                    //Debug.DrawRay(transform.position, rota, Color.red);
                    //Debug.Log(rota);

                    /*GameObject blood = */Instantiate(BloodParticles, gameObject.transform.position, RotationReference.transform.rotation);
                    /*blood.transform.rotation = Quaternion.LookRotation(FindFirstObjectByType<VisualManager>().gameObject.transform.position);
                    blood.GetComponent<ParticleSystem>().Play();*/
                    //BloodParticles.Play();
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

        FindAnyObjectByType<Finish>(FindObjectsInactive.Include).gameObject.SetActive(true);

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
        foreach (MeshRenderer mesh in GorillaMesh)
        {
            mesh.material = GorillaFullBlock;
        }

        yield return new WaitForSeconds(durationBlockAll);
        if (BlockAll)
        {
            BlockInfo.enabled = false;
            BlockAll = false;
            BlockHalf = true;

            foreach (MeshRenderer mesh in GorillaMesh)
            {
                mesh.material = GorillaHalfBlock;
            }
        }
    }

    IEnumerator MaterialFlicker()
    {
        //one
        foreach (MeshRenderer visual in VisualsSquid)
        {
            visual.material = DamageMaterial[0];
        }
        foreach (MeshRenderer visual in VisualCentaurBase)
        {
            visual.material = DamageMaterial[0];
        }
        foreach (MeshRenderer visual in VisualCentaurTorso)
        {
            visual.materials = DamageMaterial;
        }
        foreach (SkinnedMeshRenderer visual in VisualsPlayers)
        {
            visual.material = DamageMaterial[0];
        }
        yield return new WaitForSeconds(FlickerTime);

        foreach (MeshRenderer visual in VisualsSquid)
        {
            visual.material = BaseMaterialSquid;
        }
        foreach (MeshRenderer visual in VisualCentaurBase)
        {
            visual.material = BaseMaterialCentaurBase;
        }
        foreach (MeshRenderer visual in VisualCentaurTorso)
        {
            visual.materials = BaseMaterialCentaurTorso;
        }
        foreach (SkinnedMeshRenderer visual in VisualsPlayers)
        {
            visual.material = BaseMaterialPlayer;
        }
        yield return new WaitForSeconds(FlickerTime);

        //two
        foreach (MeshRenderer visual in VisualsSquid)
        {
            visual.material = DamageMaterial[0];
        }
        foreach (MeshRenderer visual in VisualCentaurBase)
        {
            visual.material = DamageMaterial[0];
        }
        foreach (MeshRenderer visual in VisualCentaurTorso)
        {
            visual.materials = DamageMaterial;
        }
        foreach (SkinnedMeshRenderer visual in VisualsPlayers)
        {
            visual.material = DamageMaterial[0];
        }
        yield return new WaitForSeconds(FlickerTime);

        foreach (MeshRenderer visual in VisualsSquid)
        {
            visual.material = BaseMaterialSquid;
        }
        foreach (MeshRenderer visual in VisualCentaurBase)
        {
            visual.material = BaseMaterialCentaurBase;
        }
        foreach (MeshRenderer visual in VisualCentaurTorso)
        {
            visual.materials = BaseMaterialCentaurTorso;
        }
        foreach (SkinnedMeshRenderer visual in VisualsPlayers)
        {
            visual.material = BaseMaterialPlayer;
        }
        yield return new WaitForSeconds(FlickerTime);

        //three
        foreach (MeshRenderer visual in VisualsSquid)
        {
            visual.material = DamageMaterial[0];
        }
        foreach (MeshRenderer visual in VisualCentaurBase)
        {
            visual.material = DamageMaterial[0];
        }
        foreach (MeshRenderer visual in VisualCentaurTorso)
        {
            visual.materials = DamageMaterial;
        }
        foreach (SkinnedMeshRenderer visual in VisualsPlayers)
        {
            visual.material = DamageMaterial[0];
        }
        yield return new WaitForSeconds(FlickerTime);

        foreach (MeshRenderer visual in VisualsSquid)
        {
            visual.material = BaseMaterialSquid;
        }
        foreach (MeshRenderer visual in VisualCentaurBase)
        {
            visual.material = BaseMaterialCentaurBase;
        }
        foreach (MeshRenderer visual in VisualCentaurTorso)
        {
            visual.materials = BaseMaterialCentaurTorso;
        }
        foreach (SkinnedMeshRenderer visual in VisualsPlayers)
        {
            visual.material = BaseMaterialPlayer;
        }
        yield return new WaitForSeconds(FlickerTime);
    }

    IEnumerator InvicibilityTimer()
    {
        Invincibility = true;
        yield return new WaitForSeconds(InvincibilityTime);
        Invincibility = false;
        //DamageGet.text = "";
    }
}
