using UnityEngine;

public class VisualManager : MonoBehaviour
{
    [SerializeField] MovementManager Movement;
    [SerializeField] CombatManager Combat;

    [SerializeField] GameObject[] Visuals;

    void Update()
    {
        if (Combat.GetVisualState() == VisualState.CrouchKick)
        {
            ChangeVisual(9);
        }
        else if (Combat.GetVisualState() == VisualState.CrouchPunch)
        {
            ChangeVisual(8);
        }
        else if (Combat.GetVisualState() == VisualState.JumpKick)
        {
            ChangeVisual(7);
        }
        else if (Combat.GetVisualState() == VisualState.JumpPunch)
        {
            ChangeVisual(6);
        }
        else if (Combat.GetVisualState() == VisualState.Kick)
        {
            ChangeVisual(5);
        }
        else if (Combat.GetVisualState() == VisualState.Punch)
        {
            ChangeVisual(4);
        }
        else if (Combat.GetStance() == StanceType.Crouch)
        {
            ChangeVisual(3);
        }
        else if (Combat.GetStance() == StanceType.Jump)
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
