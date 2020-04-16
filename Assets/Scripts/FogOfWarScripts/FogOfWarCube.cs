using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarCube : MonoBehaviour {
    private void Start()
    {
        Transform transform = gameObject.transform.GetChild(0);
        transform.SetParent(gameObject.transform.parent);
    }
    public void Disappear(Vector3 from)
    {
        StartCoroutine(Disappearing(Mathf.Abs(Vector3.Distance(gameObject.transform.position, from) / 25)));
    }

    public IEnumerator Disappearing(float t)
    {
        yield return new WaitForSeconds(t);
        float scalar = 1;
        while(scalar > .2f)
        {
            t += Time.deltaTime *4;
            scalar = Mathf.Lerp(1f, .2f, t);
            gameObject.transform.localScale = Vector3.one * scalar;
            yield return new WaitForEndOfFrame();
        }
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
