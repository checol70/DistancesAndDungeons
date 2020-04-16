using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageTextScript : MonoBehaviour
{
    private float
        TimeTillDestroy,
        Scaler;
    public GameObject
        Messenger;

    

    public void Ready(string Damage, Color32 DamageType)
    {
        Messenger = gameObject;
        gameObject.transform.rotation = gameObject.transform.parent.rotation;
        TimeTillDestroy = 1;
        Destroy(Messenger, TimeTillDestroy);
        
        Scaler = 1;
        GetComponent<Text>().text = Damage;
        GetComponent<Text>().color = DamageType;
        gameObject.GetComponent<Graphic>().CrossFadeAlpha(0f, 1f, false);
        Scaler = TimeTillDestroy;
    }

    private void Update()
    {
        gameObject.transform.localPosition = new Vector3(0, Mathf.Lerp(1.25f, 0, Scaler), 0);
        Scaler -= Time.deltaTime;
    }
    
}
