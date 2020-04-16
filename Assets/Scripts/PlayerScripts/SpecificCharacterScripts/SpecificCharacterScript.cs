using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public abstract class SpecificCharacterScript : MonoBehaviour {

    /* this script is for telling the different characters apart.  it is the base class for the scripts that will set up which 
     * Character it is and make them a singleton.  We will also need to make it Change Where it is located when we load a new 
     * scene.
     */

        
    public Vector3
       OffSet;
    public static bool returned;
    public StatEnums
        BaseStrength,
        BaseConstitution,
        BaseIntelligence,
        BaseDexterity;
    public bool
        IsCharacter;
    public string
        CharacterName;

    
   /* void OnEnable()
    {
        SceneManager.sceneLoaded += NewLevel;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= NewLevel;
    }
    */
    public void NewLevel()
    {
        StartCoroutine(WaitForEntranceExit());
    }

    /*void NewLevel(Scene DoesntMatter, LoadSceneMode wontUseAnyway)
    {
        StartCoroutine(WaitForEntranceExit());
        Debug.Log("WeHave Reset Our Position" + gameObject.name);
    }*/
    public IEnumerator DestroySoon()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    IEnumerator WaitForEntranceExit()
    {
        yield return null;
        yield return null;
        if (!SaveFileScript.loading)
        {
            if (returned)
            {
                if (ExitScript.Exit != null)
                {
                    gameObject.transform.position = ExitScript.Exit.transform.position + OffSet;

                    CameraScript.GameController.transform.position = ExitScript.Exit.transform.position + new Vector3(0, 40, -9);
                }
                else {
                    gameObject.transform.position = LocalExitScript.Exit.transform.position + OffSet;
                    CameraScript.GameController.transform.position = LocalExitScript.Exit.transform.position + new Vector3(0, 40, -9);
                }
                Debug.Log("Position Set to " + gameObject.transform.position);
            }
            else
            {
                if (EntranceScript.Entrance != null)
                    gameObject.transform.position = EntranceScript.Entrance.transform.position + OffSet;
                else gameObject.transform.position = LocalEntranceScript.Entrance.transform.position + OffSet;
                CameraScript.GameController.transform.position = LocalEntranceScript.Entrance.transform.position + new Vector3(0, 40, -9);
            }
            gameObject.GetComponent<CharacterScript>().LoadedNewLevel();
            CameraScript.GameController.Ready = true;
        }
    }
    
}
