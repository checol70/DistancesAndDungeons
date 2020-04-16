using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour {

    
    private int
        PlayerInCount;
    public static GameObject 
        Exit;
    public int SceneToBeLoaded;
    // Use this for initialization
    void OnEnable()
    {
        Exit = gameObject;
    }
	

    // Update is called once per frame

    public void LoadLevelCheck()
    {
        if (CheckForPlayerDistances())
        {
            SpecificCharacterScript.returned = false;
            SceneManager.LoadScene(2);
        }
    }
    public bool CheckForPlayerDistances()
    {
        Debug.Log("Checking");
        PlayerInCount = 0;
        foreach (GameObject player in CameraScript.GameController.Players)
        {
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= 2f)
            {
                PlayerInCount++;
                if(PlayerInCount >= CameraScript.GameController.Players.Length)
                {
                    return true;
                }
            }
            else return false;
        }
        return false;
    }
}
