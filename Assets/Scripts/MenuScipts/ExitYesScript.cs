using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitYesScript : MonoBehaviour {

    private void Start()
    {
        Button myButton = gameObject.GetComponent<Button>();
        if(myButton.onClick == null)
        {
            myButton.onClick.AddListener(Exit);
        }
    }

    public void Exit()
    {
        StartCoroutine(WaitToClose());
    }
    IEnumerator WaitToClose()
    {
        yield return new WaitUntil(() => !SaveFileScript.FinishedSaving.Values.Contains(false) || SaveFileScript.FinishedSaving.Count == 0);
        Application.Quit();
    }
}
