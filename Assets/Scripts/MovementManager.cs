using System.Collections;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [SerializeField] Transform Transform;
    [SerializeField] VisualManager VisualManager;

    [SerializeField] float Speed = 5;
    [SerializeField] float SpeedVertical = 10;
    [SerializeField] Vector3 Direction;

    public bool GamePause;

    [SerializeField] AudioSource Source;
    [SerializeField] AudioClip[] StepSounds;
    [SerializeField] bool PlaySound = true;
    [SerializeField] float DelayStepSoundMin = 0.3f;
    [SerializeField] float DelayStepSoundMax = 0.6f;

    bool IsMoving;
    bool MovingLeft;
    bool State;

    void Start()
    {
        Transform = GetComponent<Transform>();
    }

    void Update()
    {
        if (!GamePause)
        {
            Movement();
        }
    }

    void Movement()
    {
        // récupere le mouvement horizontal
        if (Input.GetAxis("Horizontal") != 0)
        {
            Direction.x = Input.GetAxis("Horizontal");
        }
        else 
        {
            Direction.x = 0;
        }

        // récupere le mouvement vertical
        if (Input.GetAxis("Vertical") != 0)
        {
            Direction.z = Input.GetAxis("Vertical");
        }
        else
        {
            Direction.z = 0;
        }

        // pour visual
        if (Direction.x != 0 || Direction.z != 0) 
        {
            IsMoving = true;

            if (PlaySound)
            {
                PlaySound = false;
                State = !State;
                Source.PlayOneShot(StepSounds[Random.Range(0, StepSounds.Length)], 0.5f);
                StartCoroutine(DelayStep());
            }

            //Debug.Log(Direction.x);

            if (Direction.x > 0.1)
            {
                VisualManager.DirectionSwitch("Right");
            }
            else if (Direction.x < -0.1)
            {
                VisualManager.DirectionSwitch("Left");
            }
        }
        else
        {
            IsMoving = false;
        }

        

        // pas de petit mouvement a la manette et égalise les mouvements clavier/manette
        Direction = Direction.normalized;

        // applique le déplacement
        Transform.position = new Vector3(Transform.position.x + (Speed * Direction.x * Time.deltaTime), Transform.position.y, Transform.position.z + (SpeedVertical * Direction.z * Time.deltaTime));
    }

    public string GetDirection()
    {
        if (MovingLeft)
        {
            return "Left";
        }
        else
        {
            return "Right";
        }
    }

    public bool GetMoving()
    {
        return IsMoving;
    }

    public bool GetMoveState()
    {
        return State;
    }

    IEnumerator DelayStep()
    {
        yield return new WaitForSeconds(Random.Range(DelayStepSoundMin, DelayStepSoundMax));
        PlaySound = true;
    }
}
