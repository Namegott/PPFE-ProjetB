using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int AttackDamageMin;
    [SerializeField] int AttackDamageMax;
    [SerializeField] float StunDuration = 0.25f;

    [SerializeField] Vector3 PropulsionForce;

    [SerializeField] ScoreManager ScoreManager;

    private void Start()
    {
        ScoreManager = FindAnyObjectByType<ScoreManager>();
    }
    void OnTriggerEnter(Collider hitTarget)
    {
        //applique les degats
        int damage = Random.Range(AttackDamageMin, AttackDamageMax);
        hitTarget.gameObject.GetComponent<HealthManager>().Damage(damage);

        EnnemiBase ennemi = hitTarget.gameObject.GetComponent<EnnemiBase>();
        if (ennemi != null)
        {
            ennemi.StunEffect(StunDuration);
            hitTarget.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (gameObject.layer == 7)
            {
                Debug.Log("Player has hit a " + ennemi.gameObject.name);
                ScoreManager.AddCombo();
            }
        }

        // applique des effets de knockback
        hitTarget.GetComponent<Rigidbody>().AddForce(PropulsionForce, ForceMode.Impulse);
    }
}
