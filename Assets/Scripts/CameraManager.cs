using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject Camera;
    [SerializeField] bool CameraMovement;
    [SerializeField] float Speed = 10;
    [SerializeField] Coroutine DelayCoRoutine;

    // Start is called before the first frame update
    void Start()
    {
        Camera = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraMovement)
        {
            Camera.transform.position = new Vector3(Camera.transform.position.x + (Speed * Time.deltaTime), Camera.transform.position.y, Camera.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 && other.gameObject.tag == "Player")
        {
            if (DelayCoRoutine != null)
            {
                StopCoroutine(DelayCoRoutine);
            }
            CameraMovement = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7 && other.gameObject.tag == "Player")
        {
            if (DelayCoRoutine != null)
            {
                StopCoroutine(DelayCoRoutine);
            }
            DelayCoRoutine = StartCoroutine(DelayEnd());
        }
    }

    IEnumerator DelayEnd()
    {
        yield return new WaitForSeconds(0.5f);
        CameraMovement = false;
    }
}
