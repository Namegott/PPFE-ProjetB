using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] bool IsGrounded;
    [SerializeField] CombatManager Combat;

    public void GroundedSwitch(bool b)
    {
        IsGrounded = b;
    }

    public bool GetGrounded()
    {
        return IsGrounded;
    }

    void OnTriggerEnter()
    {
        //Debug.Log("sol");
        if (!GetGrounded())
        {
            GroundedSwitch(true);
            if (Combat != null) 
            {
                Combat.Base();
            }
        }
    }

    void OnTriggerStay()
    {
        
    }

    void OnTriggerExit()
    {
        if (GetGrounded())
        {
            GroundedSwitch(false);
        }
    }
}
