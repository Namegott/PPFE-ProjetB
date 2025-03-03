using System.Collections;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] StanceType Stance;
    [SerializeField] GroundDetector Grounded;
    [SerializeField] Rigidbody Rb;
    [SerializeField] CapsuleCollider CapsuleCollider;

    [SerializeField] GameObject[] AttacksHitbox;
    [SerializeField] float[] AttacksDurations;
    [SerializeField] bool CanAttack = true;
    [SerializeField] VisualState VisualState = VisualState.None;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        { 
            Jump();
        }

        if (Input.GetButtonDown("Crouch"))
        {
            Crouch(); 
        }
        if (Input.GetButtonUp("Crouch"))
        {
            UnCrouch();
        }

        if (Input.GetButtonDown("Punch"))
        { 
            Punch();
        }

        if (Input.GetButtonDown("Kick"))
        {
            Kick(); 
        }
    }

    public void StanceSwitch(StanceType newStance)
    {
        Stance = newStance;
    }

    void Jump()
    {
        if (Grounded.GetGrounded())
        {
            
            Debug.Log("jump");
            Grounded.GroundedSwitch(false);
            Rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
            StanceSwitch(StanceType.Jump);
        }
    }

    void Crouch()
    {
        if (Grounded.GetGrounded())
        {
            Debug.Log("crouch");
            StanceSwitch(StanceType.Crouch);
            CapsuleCollider.height = 1;
            CapsuleCollider.center = new Vector3(0, 0.5f, 0);
        }
    }

    void UnCrouch()
    {
        StanceSwitch(StanceType.Base);
        CapsuleCollider.height = 2;
        CapsuleCollider.center = new Vector3(0, 1, 0);
    }

    void Punch()
    {
        Debug.Log("punch");
        if (CanAttack)
        {
            CanAttack = false;

            switch (Stance)
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
            }
        }
        
    }

    void Kick()
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
    }

    public VisualState GetVisualState()
    {
        return VisualState;
    }

    public StanceType GetStance()
    { 
        return Stance;
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

public enum StanceType
{
    Base = 0,
    Jump = 1,
    Crouch = 2
}

public enum VisualState
{
    None = 0,
    Punch = 1,
    JumpPunch = 2,
    CrouchPunch = 3,
    Kick = 4,
    JumpKick = 5,
    CrouchKick = 6
}