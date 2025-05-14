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
    [SerializeField] Collider Collider;
    [SerializeField] protected bool Stun;
    [SerializeField] protected bool CanMove = true;

    [SerializeField] protected GameObject Destination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void StunEffect(float stunTime)
    {
        Stun = true;
        CanMove = false;

        Collider.isTrigger = true;

        StartCoroutine(StunTime(stunTime));
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
        if (other.gameObject.layer != 6 && other.gameObject.layer != 7)
        {
            //Debug.Log("test");
            //Rigidbody.velocity = Vector3.zero;
            Collider.isTrigger = false;
        }
        if (other.gameObject == Destination)
        {
            //Debug.Log("aaaa");
            EndMove();
            Destroy(other.gameObject);
        }
    }
}
