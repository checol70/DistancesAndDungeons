using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSelectButtonScript : MonoBehaviour {

    public GameObject CharacterAssigned;
    public static GameObject SelectedButton;
    public static GameObject PreviousButton;
    public GameObject
        HealthIndicator,
        ManaIndicator,
        MaxManaIndicator;
    public bool Alive;
    private Button myButton;
    private Vector3 NewCameraPosition;
    public GameObject 
        QuickSlotSorter;
    

    // called by characterScript, so that we can have everything set up
    public void Begin()
    {
        myButton = gameObject.GetComponent<Button>();
        CharacterAssigned.GetComponent<CharacterScript>().CharacterButton = gameObject.GetComponent<Button>();
        HealthIndicator.GetComponent<CharacterSelectButtonHealthIndicatorScript>().Assign();
        ManaIndicator.GetComponent<CharacterSelectButtonManaIndicatorScript>().Assign();
        MaxManaIndicator.GetComponent<CharacterSelectButtonMaxManaIndicatorScript>().Assign();
        Alive = true;
        myButton.onClick.AddListener(Selected);
        myButton.onClick.AddListener(CharacterAssigned.GetComponent<CharacterHealthScript>().BigSliderChange);
        QuickSlotSorter.SetActive(false);
    }
    public void Selected()
    {
        //Previous button needs to be saved so that we can return it to normal
        PreviousButton = SelectedButton;

        // Setting Selected button for later when we choose another character
        SelectedButton = gameObject;

        //Resetting the scale on the last button
        if (PreviousButton != null)
        {
            PreviousButton.transform.localScale = Vector3.one;
            PreviousButton.GetComponent<Button>().interactable = PreviousButton.GetComponent<CharacterSelectButtonScript>().Alive;
            PreviousButton.GetComponent<CharacterSelectButtonScript>().QuickSlotSorter.SetActive(false);
        }

        // enlarging this button so that we can tell which character is selected easily
        gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1f);

        //Setting Active Character so that we can move the character or adjust equipment
        CameraScript.GameController.ActivePlayer = CharacterAssigned;
        CameraScript.GameController.ResetOffset();

        // Setting ourself to be uninteractable
        gameObject.GetComponent<Button>().interactable = false;
        QuickSlotSorter.SetActive(true);

    }
    public void Deceased()
    {
        gameObject.GetComponent<Button>().interactable = false;
        Alive = false;
    }
}
