using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnerEditor : MonoBehaviour
{
    [SerializeField] GameObject SpawnIndicator;
    [SerializeField] Mesh Squid,Centaur;
    Transform[] SpawnPositions;
    List<Transform> SpawnPositionsList;

    private void Update()
    {
        //Debug.Log("a");
        int size = GetComponent<Spawner>().GetEnnemisNumber();
        //w Debug.Log(size);

        //spawn and unspawn
        if (size > gameObject.transform.childCount)
        {
            //Debug.Log("manque");
            for (int i = gameObject.transform.childCount; i < size; i++)
            {
                Instantiate(SpawnIndicator, transform.position, transform.rotation, gameObject.transform);
            }

            SpawnPositions = GetComponentsInChildren<Transform>();

            SpawnPositionsList = new List<Transform>(SpawnPositions);
            SpawnPositionsList.RemoveAt(0);
            SpawnPositions = SpawnPositionsList.ToArray();

            GetComponent<Spawner>().SetPositions();
        }
        else if (size < gameObject.transform.childCount)//no longer working (for child destruction only), don't know why, will investigate later, maybe
        {
            //Debug.Log("trop");
            for (int i = gameObject.transform.childCount; i > size; i--)
            {
                //Debug.Log(i + " / " + SpawnPositions.Length);
                DestroyImmediate(SpawnPositions[i-1].gameObject);
                //Debug.Log("e");
            }

            SpawnPositions = GetComponentsInChildren<Transform>();

            SpawnPositionsList = new List<Transform>(SpawnPositions);
            SpawnPositionsList.RemoveAt(0);
            SpawnPositions = SpawnPositionsList.ToArray();

            GetComponent<Spawner>().SetPositions();
        }

        //set the mesh based on the ennemi type for level design clarity

    }
}
