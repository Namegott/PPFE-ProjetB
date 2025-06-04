
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] int Score;
    [SerializeField] int KillScore;
    [SerializeField] int ActualCombo;
    [SerializeField] int MaxCombo;
    [SerializeField] TextMeshProUGUI Combo;
    [SerializeField] RectTransform ComboTransform;
    [SerializeField] HealthManager PlayerHealth;
    [SerializeField] float HealthMult;

    [SerializeField] bool ShakeUp;
    [SerializeField] float ShakePower;

    public int GetKillScore()
    { return KillScore; }

    public int GetMaxcombo()
    { return MaxCombo; }

    public float GetHealthMult() 
    {
        Debug.Log("1 + (" + PlayerHealth.GetHealth() + " / " + PlayerHealth.GetMaxHealth() + ")");
        float health = PlayerHealth.GetHealth();
        float maxHealth = PlayerHealth.GetMaxHealth();

        HealthMult = 1 + (health / maxHealth);
        Debug.Log("health mul : " + HealthMult);
        return HealthMult; 
    }

    private void Start()
    {
        UpdateCombo();
        StartCoroutine(DelayShakeSwitch());
    }

    private void Update()
    {
        Debug.Log("pos texte : " + ComboTransform.position);
        if (ShakeUp)
        {
            ComboTransform.position = new Vector3(1429,ComboTransform.position.y + (0.2f * ShakePower),0);
        }
        else
        {
            ComboTransform.position = new Vector3(1429, ComboTransform.position.y - (0.2f * ShakePower), 0);
        }
        if (ShakePower == 0 && ComboTransform.position.y != 0)
        {
            ComboTransform.position = new Vector3(1429, 1080, 0);
        }
    }

    public void AddKillScore(int addition)
    {
        KillScore += addition;
    }

    public void AddCombo()
    {
        ActualCombo++;
        UpdateCombo();
        //Debug.Log("combo : " + ActualCombo);
        ShakePower++;
        StartCoroutine(Shakereduction());
    }

    public void EndCombo()
    {
        if (ActualCombo > MaxCombo) 
        {
            MaxCombo = ActualCombo;
        }
        ActualCombo = 0;
        UpdateCombo();
    }

    void UpdateCombo()
    {
        Combo.fontSize = (ActualCombo / 2) + 65;
        if (ActualCombo >  MaxCombo)
        {
            Combo.color = new Vector4(0.9372549f, 0.7490196f, 0.01568628f, 1);
        }
        else if (ActualCombo == 0)
        {
            Combo.color = Color.black;
        }
        else
        {
            Combo.color = new Vector4(0, 0.7264151f, 0, 1);
        }
        Combo.text = ActualCombo.ToString();
    }

    public int ScoreCalculator()
    {
        EndCombo();
        float fScore = (KillScore + (MaxCombo * 10 * GetHealthMult()) ) * GetHealthMult();
        Debug.Log("( "+KillScore+" + ( "+MaxCombo+" * 10 *"+GetHealthMult()+" ) ) * "+GetHealthMult());
        Debug.Log("float : "+fScore);
        Score = Mathf.RoundToInt(fScore);
        Debug.Log("int : "+Score);
        return Score;
    }

    IEnumerator Shakereduction()
    {
        yield return new WaitForSeconds(1);
        ShakePower--;
    }

    IEnumerator DelayShakeSwitch()
    {
        yield return new WaitForSeconds(0.25f);
        ShakeUp = !ShakeUp;
        StartCoroutine(DelayShakeSwitch());
    }
}
