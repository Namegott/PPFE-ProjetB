using System.Collections;
using UnityEngine;

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
    [SerializeField] ParticleSystem[] MoveParticles;
    [SerializeField] ParticleSystem LoadParticles;
    [SerializeField] ParticleSystem RunParticles;

    [Header("Sounds")]
    [SerializeField] AudioSource Source;
    [SerializeField] AudioSource Growl;
    [SerializeField] AudioClip BreathingSound;

    private void Start()
    {
        Player = FindObjectOfType<MovementManager>().gameObject;
        Rigidbody = GetComponent<Rigidbody>();
        GroundCheck = GetComponentInChildren<GroundDetector>();

        MapLimitX = FindFirstObjectByType<MapLimit>().GetMapLimitX();
        MapLimitZ = FindFirstObjectByType<MapLimit>().GetMapLimitZ();

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
            target.x = Mathf.Clamp(target.x, MapLimitX.x, MapLimitX.y);                                                         //pour pas sortir de la map
            target.z = Mathf.Clamp(target.z, MapLimitZ.x, MapLimitZ.y);

            Destination = Instantiate(MoveDestination, target, Quaternion.identity);
            Direction = Destination.transform.position - transform.position;

            NeedMove = true;

            //visual
            ChangeVisual(1); foreach (ParticleSystem particles in MoveParticles)
            {
                particles.Play();
            }
            RotateTorward = true;
        }
        else if (Distance > DistanceAttackMax) //too far
        {
            Debug.Log("move close");
            Vector3 target = Player.transform.position - transform.position;
            target = transform.position + target.normalized * (Distance - DistanceAttackMax);
            target.x = Mathf.Clamp(target.x, MapLimitX.x, MapLimitX.y);                                                         //pour pas sortir de la map
            target.z = Mathf.Clamp(target.z, MapLimitZ.x, MapLimitZ.y);
            /*Player.transform.position - transform.position;
        target = target.normalized * (DistanceAttackMax - DistanceAttackMin) + transform.position;
        target = new Vector3(target.x, target.y, Mathf.Clamp(target.z, 1, 19.5f));*/

            Destination = Instantiate(MoveDestination, target, Quaternion.identity);
            Direction = Destination.transform.position - transform.position;

            NeedMove = true;

            //visual
            ChangeVisual(1);
            foreach (ParticleSystem particles in MoveParticles)
            {
                particles.Play();
            }
            RotateTorward = true;
        }
    }

    public override void EndStun()
    {
        base.EndStun();

        //Setup();
    }

    public override void EndMove()
    {
        Debug.Log("aaa");
        if (Attack)
        {
            Debug.Log("bbb");
            //GetComponent<BoxCollider>().isTrigger = false;
            //Rigidbody.useGravity = true;
            Attack = false;
            StartCoroutine(PostAttack());
        }
        else if (NeedMove)
        {
            Debug.Log("end move");
            //visual
            ChangeVisual(0);
            foreach (ParticleSystem particles in MoveParticles)
            {
                particles.Stop();
            }
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
        LoadParticles.Play();
        RotateTorward = true;
        yield return new WaitForSeconds(LoadAttackTime);

        RotateTorward = false;
        LoadParticles.Stop();
        ChangeVisual(3);
        yield return new WaitForSeconds(DelayPostLoad);

        RunParticles.Play();
        AttackHitbox.SetActive(true);

        Debug.Log(Collider + " / " + Collider.gameObject + " / " + Collider.gameObject.name);
        Debug.Log(Collider.isTrigger);
        Collider.isTrigger = true;
        GetComponent<Collider>().isTrigger = true;
        Rigidbody.useGravity = false;
        Debug.Log(Collider.isTrigger);

        //movement
        Vector3 target = transform.position + (transform.forward * (DistanceAttackMax * 1.5f));
        target.x = Mathf.Clamp(target.x, MapLimitX.x, MapLimitX.y);                                                         //pour pas sortir de la map
        target.z = Mathf.Clamp(target.z, MapLimitZ.x, MapLimitZ.y);
        target = new Vector3(target.x, target.y, target.z);

        Destination = Instantiate(MoveDestination, target, Quaternion.identity);
        Direction = Destination.transform.position - transform.position;

        Growl.enabled = false;

        Attack = true;
    }

    IEnumerator PostAttack()
    {
        Debug.Log("end attack");
        RunParticles.Stop();
        ChangeVisual(0);

        Rigidbody.velocity = Vector3.zero;

        Source.PlayOneShot(BreathingSound, 1f);
        AttackHitbox.SetActive(false);

        Collider.isTrigger = false;
        Rigidbody.useGravity = true;
        yield return new WaitForSeconds(DelayPostAttack / 2);


        yield return new WaitForSeconds(DelayPostAttack / 2);

        Setup();
    }

    IEnumerator SetupStartDelay()
    {
        yield return new WaitForSeconds(Random.Range(DelaySetupMin, DelaySetupMax));
        Setup();
    }
}
