using UnityEngine;
using UnityEngine.UI;

public class TipScript : MonoBehaviour {

    private Text tip;
    private void Awake()
    {
        tip = gameObject.GetComponent<Text>();
    }

    private void OnEnable()
    {
        // take a random number to randomize tips
        int TipSelector = Random.Range(0, 8);
        switch (TipSelector)
        {
            case 0:
                {
                    tip.text = "Beware of Tiles that grow brighter.  They are probably a trap!";
                    break;
                }
            case 1:
                {
                    tip.text = "Stairs and doors run on timers.  Be sure you don't get on the wrong side!";
                    break;
                }
            case 2:
                {
                    tip.text = "Pay attention to each characters strengths. They will level those up faster!";
                    break;
                }
            case 3:
                {
                    tip.text = "Got extra movement? Don't worry, it will help your heroes get their health and mana back!";
                    break;
                }
            case 4:
                {
                    tip.text = "Mana refills over time. If you run out just wait somewhere safe";
                    break;
                }
            case 5:
                {
                    tip.text = "Monsters are vicious, but quickly forget things.  Try to hide if you are running from them!";
                    break;
                }
            case 6:
                {
                    tip.text = "If a weapon has a raised cost it will have a special quality.";
                    break;
                }
            case 7:
                {
                    tip.text = "Switching weapons doesn't cost anything.";
                    break;
                }
                //tip.text = "";
        }
    }
}
