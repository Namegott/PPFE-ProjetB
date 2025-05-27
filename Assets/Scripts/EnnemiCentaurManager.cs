using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnnemiCentaurManager : EnnemiBase
{
    [SerializeField] float Distance;
    [SerializeField] float DistanceAttackMin;
    [SerializeField] float DistanceAttackMax;

    [SerializeField] float DelaySetupMin;
    [SerializeField] float DelaySetupMax;
    [SerializeField] float LoadAttackTime;
    [SerializeField] float DelayPostLoad;
    [SerializeField] float DelayPostAttack;

    [SerializeField] GameObject AttackHitbox;
    [SerializeField] float AttackSpeed;
    [SerializeField] bool Attack;

    [SerializeField] bool NeedMove;
    [SerializeField] GameObject MoveDestination;

    [Header("Visual")]
    [SerializeField] GameObject[] Visuals;
    [SerializeField] GameObject VisualParent;
    [SerializeField] bool RotateTorward;

    [Header("Sounds")]
    [SerializeField] AudioSource Source;
    [SerializeField] AudioSource Growl;
    [SerializeField] AudioClip BreathingSound;
    public float testson;

    private void Start()
    {
        Player = FindObjectOfType<MovementManager>().gameObject;
        Rigidbody = GetComponent<Rigidbody>();
        GroundCheck = GetComponentInChildren<GroundDetector>();

        StartCoroutine(SetupStartDelay());
    }

    private void Update()
    {
        if (RotateTorward)
        {
            Quaternion rotaQ = Quaternion.LookRotation(Player.transform.position - transform.position);
            if (NeedMove)
            {
                Vector3 rotaV = rotaQ.eulerAngles;
                //Debug.Log(rotaV);

                rotaQ = Quaternion.Euler(rotaV);
            }
            transform.rotation = rotaQ;
        }

        if (Attack)
        {
            Rigidbody.velocity = Direction.normalized * AttackSpeed;
        }
        else if (NeedMove)
        {
            if (Destination.transform.position != transform.position)
            {
                if (!Stun && CanMove)
                {
                    if (Direction != Destination.transform.position - transform.position) //update the path
                    {
                        Direction = Destination.transform.position - transform.position;
                    }
                    Rigidbody.velocity = Direction.normalized * Speed;
                }
                else if (!Stun && GroundCheck.GetGrounded() && !CanMove)
                {
                    CanMove = true;
                }
            }
        }
    }

    void Setup()
    {
        Distance = Vector3.Distance(Player.transform.position, transform.position);

        if (Distance >= DistanceAttackMin && Distance <= DistanceAttackMax)
        {
            StartCoroutine(LoadAttack());
        }
        else if (Distance < DistanceAttackMin) //too close
        {
            Debug.Log("move far");
            Vector3 target = Player.transform.position - transform.position;
            target = target.normalized * (DistanceAttackMax - DistanceAttackMin) * -1 + transform.position;
            target.z = Mathf.Clamp(target.z, 1, 19.5f);

            Destination = Instantiate(MoveDestination, target, Quaternion.identity);
            Direction = Destination.transform.position - transform.position;

            NeedMove = true;

            //visual
            ChangeVisual(1);
            RotateTorward = true;
        }
        else if (Distance > DistanceAttackMax) //too far
        {
            Debug.Log("move close");
            Vector3 target = Player.transform.position - transform.position;
            target = transform.position + target.normalized * (Distance - DistanceAttackMax);
            target.z = Mathf.Clamp(target.z, 1, 19.5f);
                /*Player.transform.position - transform.position;
            target = target.normalized * (DistanceAttackMax - DistanceAttackMin) + transform.position;
            target = new Vector3(target.x, target.y, Mathf.Clamp(target.z, 1, 19.5f));*/

            Destination = Instantiate(MoveDestination, target, Quaternion.identity);
            Direction = Destination.transform.position - transform.position;

            NeedMove = true;

            //visual
            ChangeVisual(1);
            RotateTorward = true;
        }
    }


    public override void EndStun()
    {
        base.EndStun();

        Setup();
    }

    public override void EndMove()
    {
        Debug.Log("aaa");
        if (Attack)
        {
            Debug.Log("bbb");
            GetComponent<BoxCollider>().isTrigger = false;
            Rigidbody.useGravity = true;
            Attack = false;
            StartCoroutine(PostAttack());
        }
        else if (NeedMove)
        {
            Debug.Log("end move");
            //visual
            ChangeVisual(0);
            RotateTorward = false;

            NeedMove = false;
            Direction = new Vector3(0, 0, 0);
            Destination = null;
            Setup();
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

    void OnDestroy()
    {
        ChangeVisual(0);
    }

    IEnumerator LoadAttack()
    {
        Debug.Log("attack");
        ChangeVisual(2);
        Growl.enabled = true;
        RotateTorward = true;
        yield return new WaitForSeconds(LoadAttackTime);

        RotateTorward = false;
        ChangeVisual(3);
        yield return new WaitForSeconds(DelayPostLoad);

        AttackHitbox.SetActive(true);

        GetComponent<BoxCollider>().isTrigger = true;
        Rigidbody.useGravity = false;

        //movement
        Vector3 target = transform.position + (transform.forward * (DistanceAttackMax * 2));
        target = new Vector3(target.x, target.y, Mathf.Clamp(target.z, 1, 19.5f));

        Destination = Instantiate(MoveDestination, target, Quaternion.identity);
        Direction = Destination.transform.position - transform.position;

        Growl.enabled = false;

        Attack = true;
    }

    IEnumerator PostAttack()
    {
        Debug.Log("end attack");
        ChangeVisual(0);
        Source.PlayOneShot(BreathingSound, testson);
        AttackHitbox.SetActive(false);
        yield return new WaitForSeconds(DelayPostAttack);

        Setup();
    }

    IEnumerator SetupStartDelay()
    {
        yield return new WaitForSeconds(Random.Range(DelaySetupMin, DelaySetupMax));
        Setup();
    }
}
