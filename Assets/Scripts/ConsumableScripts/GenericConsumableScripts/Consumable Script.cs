using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class ConsumableScript : MonoBehaviour, IPointerClickHandler {

    public int
        NumberHeld;
    public Text
        AmountShower;
    public ConsumableEnum
        Type;
    public float
        range,
        AOE,
        cost;
    public int 
        damage;
    public string
        Explanation;
    
    

	// Use this for initialization
	void Awake () {
		if (NumberHeld == 0)
        {
            NumberHeld = 1;
        }
        if (gameObject.GetComponent<DragAndDropScript>() != null)
            gameObject.GetComponent<DragAndDropScript>().itemType = ItemType.Consumable;
        else StartCoroutine(WaitForDragAndDropScript());
    }

    public IEnumerator WaitForDragAndDropScript()
    {
        yield return new WaitUntil(() => gameObject.GetComponent<DragAndDropScript>() != null);
        gameObject.GetComponent<DragAndDropScript>().itemType = ItemType.Consumable;
    }

    public abstract bool CanBeConsumed();
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (CanBeConsumed())
            {
                Consumed();
                
            }
        }
    }
    

    public void PickedUpConsumable()
    {
        NumberHeld++;
        AmountShower.text = NumberHeld.ToString();
    }
    public abstract void ConsumedEffect();
    public void Consumed()
    {
        NumberHeld--;
        AmountShower.text = NumberHeld.ToString();
        ConsumedEffect();
        if(NumberHeld <=0)
        {
            Destroy(gameObject.GetComponent<DragAndDropScript>().StatShower);
            Destroy(gameObject);
        }
    }
    public void Dropped()
    {
        NumberHeld--;
        AmountShower.text = NumberHeld.ToString();
        Instantiate(Resources.Load("Dropped" + Type));
    }
}
