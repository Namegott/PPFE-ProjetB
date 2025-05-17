using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiDestination : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnnemiBase>().GetDestination() == gameObject)
        {
            Debug.Log("oui");
            other.gameObject.GetComponent<EnnemiBase>().EndMove();
            Destroy(gameObject);
        }
    }
}
