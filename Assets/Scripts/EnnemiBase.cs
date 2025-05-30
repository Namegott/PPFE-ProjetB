using System.Collections;
using UnityEngine;

public class EnnemiBase : MonoBehaviour
{
    [Header("Parent")]
    [SerializeField] protected GameObject Player;
    [SerializeField] protected Vector3 Direction;
    [SerializeField] protected float Speed = 1;
    [SerializeField] Vector3 Velo;
    [SerializeField] protected GroundDetector GroundCheck;

    [SerializeField] protected Rigidbody Rigidbody;
    [SerializeField] protected Collider Collider;
    [SerializeField] protected bool Stun;
    [SerializeField] Coroutine StunCoroutine;
    [SerializeField] protected bool CanMove = true;

    [SerializeField] protected GameObject Destination;
    protected Vector2 MapLimitX;
    protected Vector2 MapLimitZ;

    public GameObject GetDestination()
    {
        return Destination;
    }

    public virtual void StunEffect(float stunTime)
    {
        Debug.Log("j'uis stun");
        Stun = true;
        CanMove = false;

        Collider.isTrigger = true;

        if (StunCoroutine != null)
        {
            StopCoroutine(StunCoroutine);
        }
        
        StunCoroutine = StartCoroutine(StunTime(stunTime));
    }

    public virtual void EndStun()
    {
        if (GroundCheck.GetGrounded())
        {
            Stun = false;
            CanMove = true;
            Rigidbody.velocity = Vector3.zero;

            Collider.isTrigger = false;
        }
        else
        {
            Stun = false;
            Collider.isTrigger = false;
        }
    }

    public virtual void EndMove()
    {

    }

    IEnumerator StunTime(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);

        EndStun();
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject + " / " + Destination);
        if (other.gameObject.layer != 6 && other.gameObject.layer != 7)
        {
            //Debug.Log("test");
            //Rigidbody.velocity = Vector3.zero;
            if (Stun)
            {
                Collider.isTrigger = false;
            }
        }
        /*if (other.gameObject == Destination)
        {
            Debug.Log("aaaa");
            EndMove();
            Destroy(other.gameObject);
        }*/
    }
}
