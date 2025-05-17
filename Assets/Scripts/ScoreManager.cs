
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] int Score;
    [SerializeField] int KillScore;
    [SerializeField] int ActualCombo;
    [SerializeField] int MaxCombo;
    [SerializeField] TextMeshProUGUI Combo;
    [SerializeField] HealthManager PlayerHealth;
    [SerializeField] float HealthMult;

    public int GetKillScore()
    { return KillScore; }

    public int GetMaxcombo()
    { return MaxCombo; }

    public float GetHealthMult() 
    {
        HealthMult = 1 + (PlayerHealth.GetHealth() / PlayerHealth.GetMaxHealth());
        return HealthMult; 
    }

    private void Start()
    {
        UpdateCombo();
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
}
