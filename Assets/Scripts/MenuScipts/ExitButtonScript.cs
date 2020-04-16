using UnityEngine;
using UnityEngine.UI;

public class ExitButtonScript : MonoBehaviour {

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OpenExitObject);
    }
    public void OpenExitObject()
    {
        StartMenuScript.startMenu.ExitObject.SetActive(true);
    }
}
