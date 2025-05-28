using UnityEngine;

public class MapLimit : MonoBehaviour
{
    [SerializeField] Vector2 MapLimitX;
    [SerializeField] Vector2 MapLimitZ = new Vector2(1,20);

    private void Start()
    {
        if (MapLimitX == Vector2.zero)
        {
            Debug.LogError("Map length not set. All ennemis will move to 0-0.");
        }
    }

    public Vector2 GetMapLimitX()
    { 
        return MapLimitX; 
    }

    public Vector2 GetMapLimitZ() 
    {  
        return MapLimitZ; 
    }
}
