using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EditorFeatures: MonoBehaviour, IBeginDragHandler,  IDragHandler, IEndDragHandler, IScrollHandler
{
	private ScrollRect scrollRect;

    public void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        scrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        scrollRect.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scrollRect.OnEndDrag(eventData);
    }

    public void OnScroll(PointerEventData data)
    {
        scrollRect.OnScroll(data);
    }
}