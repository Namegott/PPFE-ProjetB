using System.Collections;
using UnityEngine;

public class EnnemiSpawner : MonoBehaviour
{
    [SerializeField] float SpawnDelay = 2.5f;
    [SerializeField] GameObject Ennemi;
    [SerializeField] bool AutoSpawn;

    void Start()
    {
        StartCoroutine(SpawnTime());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            AutoSpawn = !AutoSpawn;
            if (!AutoSpawn)
            {
                StopAllCoroutines();
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Spawn();
        }
    }

    void Spawn()
    {
        Instantiate(Ennemi, new Vector3(0, 1, 0), new Quaternion(0, 0, 0, 0));
    }

    IEnumerator SpawnTime()
    {
        if (AutoSpawn)
        {
            yield return new WaitForSeconds(SpawnDelay);

            Spawn();
            StartCoroutine(SpawnTime());
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 150, 25), "Auto spawn ? " + AutoSpawn);
    }
}
