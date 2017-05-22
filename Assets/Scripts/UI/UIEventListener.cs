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

    public delegate void VoidDelegate(GameObject go, BaseEventData eventData);

    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onScroll;
    public VoidDelegate onCancel;
    public VoidDelegate onDrop;
    public VoidDelegate onMove;
    public VoidDelegate onUpdateSelect;
    public VoidDelegate onDeselect;

    public void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null)
            onUpdateSelect(gameObject, eventData);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null)
            onSelect(gameObject, eventData);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (onDeselect != null)
            onDeselect(gameObject, eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null)
            onEnter(gameObject, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null)
            onExit(gameObject, eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null)
            onDown(gameObject, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null)
            onUp(gameObject, eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
            onClick(gameObject, eventData);
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (onScroll != null)
            onScroll(gameObject, eventData);
    }

    public void OnMove(AxisEventData eventData)
    {
        if (onMove != null)
            onMove(gameObject, eventData);
    }

    public void OnCancel(BaseEventData eventData)
    {
        if (onCancel != null)
            onCancel(gameObject, eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (onDrop != null)
            onDrop(gameObject, eventData);
    }

    public static UIEventListener Get(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if(listener == null)
        {
            listener = go.AddComponent<UIEventListener>();
        }
        return listener;
    }
}
