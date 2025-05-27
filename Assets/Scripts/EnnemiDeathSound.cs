using UnityEngine;

public class EnnemiDeathSound : MonoBehaviour
{
    AudioSource Source;

    void Awake()
    {
        Source = GetComponent<AudioSource>();
        
    }
    private void Start()
    {
        Debug.Log("sopawn");
    }

    public void Play(AudioClip sound)
    {
        Source.clip = sound;
        Source.PlayOneShot(Source.clip, 1f);

        Destroy(gameObject, sound.length + 0.5f);
    }
}
