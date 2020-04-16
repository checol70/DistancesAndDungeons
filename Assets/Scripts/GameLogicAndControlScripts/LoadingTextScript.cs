using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class LoadingTextScript : MonoBehaviour {

    private void OnEnable()
    {
        StartCoroutine(Animate());
    }
    private void OnDisable()
    {
        StopCoroutine(Animate());
    }
    private IEnumerator Animate()
    {
        do
        {
            gameObject.GetComponent<Text>().text = "Loading";
            yield return new WaitForSeconds(.5f);

            gameObject.GetComponent<Text>().text = "Loading.";
            yield return new WaitForSeconds(.5f);

            gameObject.GetComponent<Text>().text = "Loading..";
            yield return new WaitForSeconds(.5f);

            gameObject.GetComponent<Text>().text = "Loading...";
            yield return new WaitForSeconds(.5f);
        } while (true);
        
    }

}
