using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int AttackDamageMin;
    [SerializeField] int AttackDamageMax;
    [SerializeField] float StunDuration = 0.25f;

    [SerializeField] Vector3 PropulsionForce;

    void OnTriggerEnter(Collider hitTarget)
    {
        //applique les degats
        int damage = Random.Range(AttackDamageMin, AttackDamageMax);
        hitTarget.gameObject.GetComponent<HealthManager>().Damage(damage);

        EnnemiNavigation nav = hitTarget.gameObject.GetComponent<EnnemiNavigation>();
        if (nav != null)
        {
            nav.StunEffect(StunDuration);
            hitTarget.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        // applique des effets de knockback
        hitTarget.GetComponent<Rigidbody>().AddForce(PropulsionForce, ForceMode.Impulse);
    }
}
