using System.Collections;
using UnityEngine;

public class EnnemiNavigation : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] float PathUpdateTime = 0.5f;
    [SerializeField] Vector3 Direction;
    [SerializeField] float Speed = 1;
    [SerializeField] Vector3 Velo;
    [SerializeField] GroundDetector GroundCheck;

    [SerializeField] Rigidbody Rigidbody;
    [SerializeField] Collider Collider;
    [SerializeField] bool Stun;
    [SerializeField] bool CanMove = true;

    void Start()
    {
        Player = FindObjectOfType<MovementManager>().gameObject;
        Rigidbody = GetComponent<Rigidbody>();
        GroundCheck = GetComponentInChildren<GroundDetector>();

        Direction = Player.transform.position - transform.position;
        PathUpdate();
    }

    void Update()
    {
        Velo = Rigidbody.velocity;

        if (!Stun && CanMove)
        {
            Debug.Log("bouge");
            Rigidbody.velocity = Direction.normalized * Speed;
        }

        if (!Stun && GroundCheck.GetGrounded() && !CanMove)
        {
            CanMove = true;
            PathUpdate();
        }
    }

    public void PathUpdate()
    {
        if (Direction != Player.transform.position - transform.position)
        {
            Direction = Player.transform.position - transform.position;
        }

        StartCoroutine(PathUpdateDelay(PathUpdateTime));
    }

    public void StunEffect(float stunTime)
    {
        Stun = true;
        CanMove = false;

        Collider.isTrigger = true;

        StartCoroutine(StunTime(stunTime));
    }

    void EndStun()
    {
        if (GroundCheck.GetGrounded())
        {
            Stun = false;
            CanMove = true;
            Rigidbody.velocity = Vector3.zero;

            Collider.isTrigger = false;

            PathUpdate();
        }
        else
        {
            Stun = false;
            Collider.isTrigger = false;
        }
    }

    IEnumerator StunTime(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);

        EndStun();
    }

    IEnumerator PathUpdateDelay(float time)
    {
        yield return new WaitForSeconds(time);
        PathUpdate();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6 && other.gameObject.layer != 7)
        {
            Debug.Log("test");
            //Rigidbody.velocity = Vector3.zero;
            Collider.isTrigger = false;
        }
    }
}
