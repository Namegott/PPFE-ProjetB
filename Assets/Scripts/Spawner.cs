using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] SpawnEnnemis;
    public Transform[] SpawnPositions;
    bool DoOnce = true;

    public int GetEnnemisNumber()
    {
        return SpawnEnnemis.Length;
    }

    public void SetPositions() //auto update with SpawnerEditor
    {
        SpawnPositions = GetComponentsInChildren<Transform>();

        List<Transform> list = new List<Transform>(SpawnPositions);
        list.RemoveAt(0);
        SpawnPositions = list.ToArray();
    }

    private void Start() //strangely not working
    {
        if (SpawnEnnemis.Length > SpawnPositions.Length)
        {
            Debug.LogError("Spawner Error 1 : there is " + SpawnEnnemis.Length + 1 + "ennemis set for " + SpawnPositions.Length + 1 + " locations.");
        }
        else if (SpawnPositions.Length > SpawnEnnemis.Length)
        {
            Debug.LogError("Spawner Error 2 : there is " + SpawnPositions.Length + 1 + "positons set for " + SpawnEnnemis.Length + 1 + " ennemis.");
        }
    }

    void Spawn()
    {
        Debug.Log("spawn");
        for (int i = 0; i < SpawnPositions.Length; i++)
        {
            //Debug.Log("spawn : " + SpawnEnnemis[i] + " at " + SpawnPositions[i].transform.position);
            Instantiate(SpawnEnnemis[i], SpawnPositions[i].transform.position, SpawnEnnemis[i].GetComponent<Transform>().transform.rotation);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && DoOnce)
        {
            DoOnce = false;
            Spawn();
        }
    }
}