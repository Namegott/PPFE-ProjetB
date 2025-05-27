using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float Delay;

    void Start()
    {
        Destroy(gameObject, Delay);
    }
}
