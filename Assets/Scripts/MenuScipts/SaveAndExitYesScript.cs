using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndExitYesScript : MonoBehaviour {

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(SaveAndExit);
    }

    public void SaveAndExit()
    {
        SaveFileScript.SaveSelected.Save();
        StartCoroutine(WaitToClose());
    }
    IEnumerator WaitToClose()
    {
        yield return new WaitForSeconds(.1f);
        yield return new WaitUntil(()=>!SaveFileScript.FinishedSaving.Values.Contains(false) || SaveFileScript.FinishedSaving.Count == 0);
        Application.Quit();
    }
}
