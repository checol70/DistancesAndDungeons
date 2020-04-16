using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EntranceScript : MonoBehaviour {
    public int
        SceneToBeLoaded;
    public static GameObject 
        Entrance;
    private int
        PlayerInCount;
    public GameObject[]
        Torches;
	// Use this for initialization
    void OnEnable()
    {
        Entrance = gameObject; 
    }
    
    //Used to check and load levels
    public void LoadLevelCheck()
    {
        //if (CheckForPlayerDistances())
        //{
        //    if(SceneManager.GetActiveScene().buildIndex == 0)
        //        SceneToBeLoaded = SceneManager.sceneCount -1;
        //    else
        //    {
        //        SceneToBeLoaded = SceneManager.GetActiveScene().buildIndex - 1;
        //    }
        //    SceneManager.LoadScene(SceneToBeLoaded, LoadSceneMode.Single);
        //}
    }
    public bool CheckForPlayerDistances()
    {
        PlayerInCount = 0;
        foreach (GameObject player in CameraScript.GameController.Players)
        {
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= 2f)
            {
                PlayerInCount++;
                if (PlayerInCount >= CameraScript.GameController.Players.Length)
                {
                    Debug.Log("Players Within Range");
                    SpecificCharacterScript.returned = true;
                    return true;
                    
                }
            }
            else
            {
                Debug.Log("Players Out of Range");
                return false;
            }
        }
        Debug.Log("Loop Failed");
        return false;
    }
}
