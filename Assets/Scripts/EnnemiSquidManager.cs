using System.Collections;
using UnityEngine;

public class EnnemiPoulpeManager : EnnemiBase
{
    [SerializeField] float Distance;
    [SerializeField] float DistanceAttackMin;
    [SerializeField] float DistanceAttackMax;

    [SerializeField] float DelaySetupMin;
    [SerializeField] float DelaySetupMax;
    [SerializeField] float DelayPreAttack;
    [SerializeField] float DelayPostAttack;

    [SerializeField] bool NeedMove;
    [SerializeField] GameObject MoveDestination;


    [Header("Projectile")]
    [SerializeField] GameObject Projectile;
    [SerializeField] float ImpactDuration;
    [SerializeField] float ProjDuration;

    [Header("Visual")]
    [SerializeField] GameObject[] Visuals;
    [SerializeField] GameObject VisualParent;
    [SerializeField] bool RotateTorward;

    [Header("Sounds")]
    [SerializeField] AudioSource Source;
    [SerializeField] AudioClip AimSound;
    [SerializeField] AudioClip ShootSound;

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
            Quaternion rotaQ = Quaternion.LookRotation((Player.transform.position - transform.position) * -1);
            if (NeedMove)
            {
                Vector3 rotaV = rotaQ.eulerAngles;
                //Debug.Log(rotaV);

                rotaQ = Quaternion.Euler(rotaV);
            }
            VisualParent.transform.rotation = rotaQ;
        }

        if (NeedMove)
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
        //Debug.Log(Distance);
        if (Distance >= DistanceAttackMin && Distance <= DistanceAttackMax)
        {
            StartCoroutine(Attack());
        }
        else if (Distance < DistanceAttackMin) //too close
        {
            Vector3 target = Player.transform.position - transform.position;
            target = target.normalized * (DistanceAttackMax - DistanceAttackMin) * -1 + transform.position;
            target = new Vector3(target.x, target.y, Mathf.Clamp(target.z, 1, 19.5f));

            Destination = Instantiate(MoveDestination, target, Quaternion.identity);
            Direction = Destination.transform.position - transform.position;
            
            NeedMove = true;

            //visual
            ChangeVisual(1);
            RotateTorward = true;
        }
        else if (Distance > DistanceAttackMax) //too far
        {
            Vector3 target = Player.transform.position - transform.position;
            target = target.normalized * (DistanceAttackMax - DistanceAttackMin) + transform.position;
            target.x = Mathf.Clamp(target.z, 1, 19.5f);
            Destination = Instantiate(MoveDestination, target , Quaternion.identity);

            Direction = Destination.transform.position - transform.position;
            
            NeedMove = true;

            //visual
            ChangeVisual(1);
            RotateTorward = true;
        }
    }

    public override void EndMove()
    {
        //visual
        ChangeVisual(0);
        RotateTorward = false;
        
        NeedMove = false;
        Direction = new Vector3(0, 0, 0);
        Destination = null;
        StartCoroutine(Attack());
    }
    
    public override void EndStun()
    {
        base.EndStun();

        Setup();
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

    IEnumerator SetupStartDelay()
    {
        yield return new WaitForSeconds(Random.Range(DelaySetupMin, DelaySetupMax));
        Setup();
    }
    
    IEnumerator Attack()
    {
        ChangeVisual(2);
        RotateTorward = true;

        Source.PlayOneShot(AimSound, 0.10f);

        yield return new WaitForSeconds(DelayPreAttack);

        RotateTorward = false;
        GameObject proj = Instantiate(Projectile);
        proj.GetComponent<ProjSquidManager>().Setup(transform.position, Player.transform.position, ImpactDuration, ProjDuration);

        Source.PlayOneShot(ShootSound, 1f);

        yield return new WaitForSeconds(ProjDuration + ImpactDuration);

        ChangeVisual(0);
        VisualParent.transform.rotation = Quaternion.Euler(0, 0, 0);
        //Debug.Log("idle : " + VisualParent.transform.rotation.eulerAngles);
        yield return new WaitForSeconds(DelayPostAttack);

        Setup();
    }
}
