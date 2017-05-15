using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventListener : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler,
    IPointerUpHandler,
    IPointerDownHandler,
    IPointerClickHandler,
    IScrollHandler,
    IMoveHandler,
    ICancelHandler,
    ISelectHandler,
    IDeselectHandler,
    IUpdateSelectedHandler,
    IDropHandler
{

    private delegate void VoidDelete(GameObject go, BaseEventData eventData);

    public void onPointEnter(PointerEventData eventData)
    {

    }
}
