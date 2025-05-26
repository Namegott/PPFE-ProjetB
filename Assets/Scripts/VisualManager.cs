using UnityEngine;

public class VisualManager : MonoBehaviour
{
    [SerializeField] MovementManager Movement;
    [SerializeField] CombatManager Combat;

    [SerializeField] GameObject VisualRoot;
    [SerializeField] GameObject AttackHitboxRight;
    [SerializeField] GameObject AttackHitboxLeft;
    [SerializeField] GameObject[] Visuals;
    [SerializeField] ParticleSystem MoveParticles;

    void Update()
    {
        if (Combat.GetVisualState() == VisualState.Block)
        {
            ChangeVisual(7);
            if (MoveParticles.isEmitting)
            {
                MoveParticles.Stop();
            }
        }
        else if (Combat.GetVisualState() == VisualState.Slide)
        {
            ChangeVisual(6);
            if (MoveParticles.isEmitting)
            {
                MoveParticles.Stop();
            }
        }
        else if (Combat.GetVisualState() == VisualState.Uppercut)
        {
            ChangeVisual(5);
            if (MoveParticles.isEmitting)
            {
                MoveParticles.Stop();
            }
        }
        else if (Combat.GetVisualState() == VisualState.Punch1)
        {
            ChangeVisual(4);
            if (MoveParticles.isEmitting)
            {
                MoveParticles.Stop();
            }
        }
        else if (Combat.GetVisualState() == VisualState.Punch0)
        {
            ChangeVisual(3);
            if (MoveParticles.isEmitting)
            {
                MoveParticles.Stop();
            }
        }
        else if (Movement.GetMoving() && Movement.GetMoveState())
        {
            ChangeVisual(2);
            if (!MoveParticles.isEmitting)
            {
                MoveParticles.Play();
            }
        }
        else if (Movement.GetMoving())
        {
            ChangeVisual(1);
            if (!MoveParticles.isEmitting)
            {
                MoveParticles.Play();
            }
        }
        else
        {
            ChangeVisual(0);
            if (MoveParticles.isEmitting)
            {
                MoveParticles.Stop();
            }
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
