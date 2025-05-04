using UnityEngine;

public class VisualManager : MonoBehaviour
{
    [SerializeField] MovementManager Movement;
    [SerializeField] CombatManager Combat;

    [SerializeField] GameObject VisualRoot;
    [SerializeField] GameObject AttackHitboxRight;
    [SerializeField] GameObject AttackHitboxLeft;
    [SerializeField] GameObject[] Visuals;

    void Update()
    {
        if (Combat.GetVisualState() == VisualState.Block)
        {
            ChangeVisual(6);
        }
        else if (Combat.GetVisualState() == VisualState.Slide)
        {
            ChangeVisual(5);
        }
        else if (Combat.GetVisualState() == VisualState.Uppercut)
        {
            ChangeVisual(4);
        }
        else if (Combat.GetVisualState() == VisualState.Punch1)
        {
            ChangeVisual(3);
        }
        else if (Combat.GetVisualState() == VisualState.Punch0)
        {
            ChangeVisual(2);
        }
        else if (Movement.GetMoving())
        {
            ChangeVisual(1);
        }
        else
        {
            ChangeVisual(0);
        }
    }

    public void DirectionSwitch(string Direction)
    {
        if (Direction == "Left") 
        {
            VisualRoot.transform.localScale = new Vector3(-1, 1, 1);
            AttackHitboxRight.SetActive(false);
            AttackHitboxLeft.SetActive(true);
        }
        else if (Direction == "Right")
        {
            VisualRoot.transform.localScale = new Vector3(1, 1, 1);
            AttackHitboxLeft.SetActive(false);
            AttackHitboxRight.SetActive(true);
        }
    }

    void ChangeVisual(int visualNumber)
    {
        if (!Visuals[visualNumber].activeSelf)
        {
            foreach (GameObject visual in Visuals)
            {
                visual.SetActive(false);
            }
            Visuals[visualNumber].SetActive(true);
        }
    }
}
