using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class ProjSquidManager : MonoBehaviour
{
    [SerializeField] SplineContainer Spline;
    [SerializeField] SplineAnimate ProjAnimation;
    [SerializeField] GameObject ImpactIndicator;
    [SerializeField] GameObject HitBox;

    float ImpactDuration;
    float ProjDuration;
    [SerializeField] float ScalingPerSecond;

    bool DoOnce = true;


    void Update()
    {
        //movement of the proj
        if (ProjAnimation.ElapsedTime < ProjAnimation.Duration)
        {
            float scalePlus = ScalingPerSecond * Time.deltaTime;
            ImpactIndicator.transform.localScale = new Vector3(ImpactIndicator.transform.localScale.x + scalePlus, ImpactIndicator.transform.localScale.y, ImpactIndicator.transform.localScale.z + scalePlus);
        }
        //impact
        else if (DoOnce)
        {
            DoOnce = false;
            Impact();
        }
    }

    public void Setup(Vector3 squid, Vector3 player, float impactDuration, float projDuration)
    {
        //set variable
        ImpactDuration = impactDuration;
        ProjDuration = projDuration;

        float scaleAim = HitBox.transform.localScale.x - ImpactIndicator.transform.localScale.x;
        ScalingPerSecond = scaleAim / ProjDuration;

        //setup impact
        ImpactIndicator.transform.position = transform.position + player;
        HitBox.transform.position = transform.position + player;

        //get middle point of spline
        float middleX = squid.x + ((player.x - squid.x) / 2);
        float middleZ = squid.z + ((player.z - squid.z) / 2);

        //setup spline
        Spline.Spline.SetKnot(0, new BezierKnot(squid));
        Spline.Spline.SetKnot(1, new BezierKnot(new Vector3(middleX, 5, middleZ)));
        Spline.Spline.SetKnot(2, new BezierKnot(player));

        //setup proj movement
        ProjAnimation.Duration = ProjDuration;
        ProjAnimation.Play();

    }

    void Impact()
    {
        HitBox.SetActive(true);
        Destroy(Spline.gameObject);
        Destroy(ProjAnimation.gameObject);

        StartCoroutine(ExecImpactDuration());
    }

    IEnumerator ExecImpactDuration()
    {
        yield return new WaitForSeconds(ImpactDuration);
        Destroy(gameObject);
    }
}
