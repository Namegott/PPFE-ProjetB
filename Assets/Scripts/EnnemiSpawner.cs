using System.Collections;
using UnityEngine;

public class EnnemiSpawner : MonoBehaviour
{
    [SerializeField] float SpawnDelay = 2.5f;
    [SerializeField] GameObject Ennemi;

    void Start()
    {
        StartCoroutine(SpawnTime());
    }

    IEnumerator SpawnTime()
    {
        yield return new WaitForSeconds(SpawnDelay);

        Instantiate(Ennemi, new Vector3(0, 1, 0), new Quaternion(0, 0, 0, 0));
        StartCoroutine(SpawnTime());
    }
}
