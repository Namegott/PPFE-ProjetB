using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiPoulpeManager : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] float Distance;
    [SerializeField] float DistanceAttackMin;
    [SerializeField] float DistanceAttackMax;

    [SerializeField] GameObject Projectile;
    [SerializeField] float ImpactDuration;
    [SerializeField] float ProjDuration;

    [SerializeField] float DelaySetupMin;
    [SerializeField] float DelaySetupMax;
    [SerializeField] float DelayPreAttack;
    [SerializeField] float DelayPostAttack;

    [SerializeField] GameObject[] Visuals;
    [SerializeField] GameObject VisualParent;
    [SerializeField] bool RotateTorward;

    private void Start()
    {
        Player = FindObjectOfType<MovementManager>().gameObject;
        StartCoroutine(SetupStartDelay());
    }

    private void Update()
    {
        if (RotateTorward)
        {
            VisualParent.transform.rotation = Quaternion.LookRotation((Player.transform.position - transform.position) * -1);
        }
    }

    void Setup()
    {
        Distance = Vector3.Distance(Player.transform.position, transform.position);
        Debug.Log(Distance);
        if (Distance > DistanceAttackMin && Distance < DistanceAttackMax)
        {
            StartCoroutine(Attack());
        }
        else if (Distance < DistanceAttackMin) //too close
        {
            //ChangePosition(Vector3.RotateTowards(transform.position, Player.transform.position, 1, 0));
        }
        else if (Distance > DistanceAttackMax) //too far
        {

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

    IEnumerator SetupStartDelay()
    {
        yield return new WaitForSeconds(Random.Range(DelaySetupMin, DelaySetupMax));
        Setup();
    }
    
    IEnumerator Attack()
    {
        ChangeVisual(2);
        RotateTorward = true;
        yield return new WaitForSeconds(DelayPreAttack);

        RotateTorward = false;
        GameObject proj = Instantiate(Projectile);
        proj.GetComponent<ProjSquidManager>().Setup(transform.position, Player.transform.position, ImpactDuration, ProjDuration);
        yield return new WaitForSeconds(ProjDuration + ImpactDuration);

        ChangeVisual(0);
        VisualParent.transform.rotation = Quaternion.Euler(0, 0, 0);
        Debug.Log("idle : " + VisualParent.transform.rotation.eulerAngles);
        yield return new WaitForSeconds(DelayPostAttack);

        Setup();
    }
}
