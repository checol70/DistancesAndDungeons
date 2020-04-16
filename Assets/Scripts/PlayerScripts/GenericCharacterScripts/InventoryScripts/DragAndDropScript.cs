using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragAndDropScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    
    public GameObject
        OnTopSlot,
        SelectedItemRevealer,
        StatShower;
    public static GameObject
        HighlightObject, 
        DraggedItem;
    public static bool
        ItemSelected;
    public Transform
        PreviousSlot;
    public ItemType
        itemType;

    private void Start()
    {
        if (gameObject.transform.root.gameObject != gameObject)
        {
            OnTopSlot = gameObject.transform.root.gameObject.GetComponent<CharacterScript>().OnTopSlot;
        }
        else StartCoroutine(FindParent());
    }
    IEnumerator FindParent()
    {
        yield return new WaitWhile(() => gameObject.transform.root.gameObject == gameObject);
        OnTopSlot = gameObject.transform.root.gameObject.GetComponent<CharacterScript>().OnTopSlot;
    }
    void RaycastStopper()
    {
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    void RaycastContinuer()
    {
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        PreviousSlot = gameObject.transform.parent;
        DraggedItem = this.gameObject;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        gameObject.transform.SetParent(OnTopSlot.transform);
    }
    public void OnDrag(PointerEventData eventData)
    {
        gameObject.transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (gameObject.transform.parent.gameObject == OnTopSlot)
        {
            gameObject.transform.SetParent(PreviousSlot);
        }
        transform.localPosition = Vector3.zero;
    }
    public void OnPointerEnter(PointerEventData pointer)
    {
        StatShower = Instantiate(Resources.Load("WeaponStatShower") as GameObject);
        StartCoroutine(StatShower.GetComponent<StatShowerScript>().ShowStats(gameObject));
        StatShower.transform.SetParent(OnTopSlot.transform, true);
    }
    public void OnPointerExit(PointerEventData pointer)
    {
        Destroy(StatShower);
    }

}
