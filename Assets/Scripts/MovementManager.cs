using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [SerializeField] Transform Transform;
    [SerializeField] VisualManager VisualManager;

    [SerializeField] float Speed = 5;
    [SerializeField] Vector3 Direction;

    bool IsMoving;
    bool MovingLeft;

    void Start()
    {
        Transform = GetComponent<Transform>();
    }

    void Update()
    {
        Movement();
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

            Debug.Log(Direction.x);

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
        Transform.position = new Vector3(Transform.position.x + (Speed * Direction.x * Time.deltaTime), Transform.position.y, Transform.position.z + (Speed * Direction.z * Time.deltaTime));
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
}
