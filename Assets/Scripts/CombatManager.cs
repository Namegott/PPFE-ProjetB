using System.Collections;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] float JumpForce;
    /*[SerializeField] StanceType Stance;*/
    [SerializeField] GroundDetector Grounded;
    [SerializeField] Rigidbody Rb;
    [SerializeField] CapsuleCollider CapsuleCollider;
    [SerializeField] MovementManager MovementManager;

    [SerializeField] HealthManager HealthManager;
    [SerializeField] float DurationBlockAll;
    [SerializeField] bool CanBlock;
    [SerializeField] float CdBlock;

    [SerializeField] int PunchState = 0;
    [SerializeField] GameObject[] AttacksHitbox;
    [SerializeField] float[] AttacksDurations;
    [SerializeField] bool CanAttack = true;
    [SerializeField] VisualState VisualState = VisualState.None;

    
    void Update()
    {
        if (Input.GetButtonDown("Punch"))
        {
            Punch();
        }

        if (Input.GetButtonDown("Uppercut"))
        {
            Uppercut();
        }

        if (Input.GetButtonDown("Slide"))
        {
             Slide();
        }

        if (Input.GetButtonDown("Defense"))
        {
            if (CanBlock)
            {
                Debug.Log("Def");

                HealthManager.BlockStart(DurationBlockAll);
                VisualState = VisualState.Block;
            }
        }
        if (Input.GetButtonUp("Defense"))
        {
            if (CanBlock)
            {
                Debug.Log("plus Def");
                CanBlock = false;

                StartCoroutine(BlockCooldown(CdBlock));
                HealthManager.BlockEnd();
                VisualState = VisualState.None;
            }
        }
    }

    /*public void StanceSwitch(StanceType newStance)
    {
        Stance = newStance;
    }*/

    /*void Jump()
    {
        if (Grounded.GetGrounded())
        {
            
            Debug.Log("jump");
            Grounded.GroundedSwitch(false);
            Rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
            StanceSwitch(StanceType.Jump);
        }
    }*/

    /*void Crouch()
    {
        if (Grounded.GetGrounded())
        {
            Debug.Log("crouch");
            StanceSwitch(StanceType.Crouch);
            CapsuleCollider.height = 1;
            CapsuleCollider.center = new Vector3(0, 0.5f, 0);
        }
    }*/

    /*void UnCrouch()
    {
        StanceSwitch(StanceType.Base);
        CapsuleCollider.height = 2;
        CapsuleCollider.center = new Vector3(0, 1, 0);
    }*/

    public void Base()
    {
        VisualState = VisualState.None;
    }

    void Punch()
    {
        Debug.Log("punch");
        if (CanAttack)
        {
            CanAttack = false;

            if (PunchState == 0)
            {
                PunchState = 1;
                
                //right
                AttacksHitbox[1].gameObject.SetActive(false);
                AttacksHitbox[0].gameObject.SetActive(true);
                //left
                AttacksHitbox[5].gameObject.SetActive(false);
                AttacksHitbox[4].gameObject.SetActive(true);

                StartCoroutine(AttackDuration(AttacksDurations[0]));
                VisualState = VisualState.Punch0;
            }
            else if (PunchState == 1)
            {
                //right
                AttacksHitbox[0].gameObject.SetActive(false);
                AttacksHitbox[1].gameObject.SetActive(true);
                //left
                AttacksHitbox[4].gameObject.SetActive(false);
                AttacksHitbox[5].gameObject.SetActive(true);

                PunchState = 0;
                StartCoroutine(AttackDuration(AttacksDurations[0]));
                VisualState = VisualState.Punch1;
            }

            /*switch (Stance)
            {
                case StanceType.Base:
                    AttacksHitbox[0].gameObject.SetActive(true);
                    StartCoroutine(AttackDuration(AttacksDurations[0]));
                    VisualState = VisualState.Punch;
                    break;

                case StanceType.Jump:
                    AttacksHitbox[1].gameObject.SetActive(true);
                    StartCoroutine(AttackDuration(AttacksDurations[1]));
                    VisualState = VisualState.JumpPunch;
                    break;

                case StanceType.Crouch:
                    AttacksHitbox[2].gameObject.SetActive(true);
                    StartCoroutine(AttackDuration(AttacksDurations[2]));
                    VisualState = VisualState.CrouchPunch;
                    break;
            }*/
        }
    }

    void Uppercut()
    {
        if (CanAttack)
        {
            if (!Grounded.GetGrounded())
            {
                Grounded.gameObject.SetActive(false);
                Grounded.gameObject.SetActive(true);
            }

            if (Grounded.GetGrounded())
            {
                CanAttack = false;

                //saut
                Grounded.GroundedSwitch(false);
                Rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
                //attaque
                AttacksHitbox[2].gameObject.SetActive(true);
                AttacksHitbox[6].gameObject.SetActive(true);
                StartCoroutine(AttackDuration(AttacksDurations[1]));
                VisualState = VisualState.Uppercut;
            }
        }
    }

    void Slide()
    {
        if (CanAttack)
        {
            CanAttack = false;

            AttacksHitbox[3].gameObject.SetActive(true);
            AttacksHitbox[7].gameObject.SetActive(true);
            StartCoroutine(AttackDuration(AttacksDurations[2]));
            VisualState = VisualState.Slide;
        }
    }

    /*void Kick()
    {
        Debug.Log("kick");
        if (CanAttack)
        {
            CanAttack = false;

            switch (Stance)
            {
                case StanceType.Base:
                    AttacksHitbox[3].gameObject.SetActive(true);
                    StartCoroutine(AttackDuration(AttacksDurations[3]));
                    VisualState = VisualState.Kick;
                    break;

                case StanceType.Jump:
                    AttacksHitbox[4].gameObject.SetActive(true);
                    StartCoroutine(AttackDuration(AttacksDurations[4]));
                    VisualState = VisualState.JumpKick;
                    break;

                case StanceType.Crouch:
                    AttacksHitbox[5].gameObject.SetActive(true);
                    StartCoroutine(AttackDuration(AttacksDurations[5]));
                    VisualState = VisualState.CrouchKick;
                    break;
            }
        }
    }*/

    public VisualState GetVisualState()
    {
        return VisualState;
    }

    /*public StanceType GetStance()
    { 
        return Stance;
    }*/

    IEnumerator BlockCooldown(float cdBlock)
    {
        yield return new WaitForSeconds(cdBlock);
        CanBlock = true;
    }

    IEnumerator AttackDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        foreach (GameObject AttackHitbox in AttacksHitbox)
        {
            AttackHitbox.gameObject.SetActive(false);
        }

        CanAttack = true;
        VisualState = VisualState.None;
    }
}

/*public enum StanceType
{
    Base = 0,
    Jump = 1,
    Crouch = 2
}*/

public enum VisualState
{
    None = 0,
    Punch0 = 1,
    Punch1 = 2,
    Uppercut = 3,
    Slide = 4,
    Block = 5
}