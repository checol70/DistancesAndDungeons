using UnityEngine;

public class StaticDungeonScript : MonoBehaviour
{

    public static GameObject dungeon;
    // Use this for initialization
    void Awake()
    {
        if (dungeon == null)
        {
            dungeon = gameObject;
        }
        else if (dungeon != gameObject)
        {
            Destroy(gameObject);
        }
    }
}