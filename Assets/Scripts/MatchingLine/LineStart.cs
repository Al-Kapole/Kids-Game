using UnityEngine;
using UnityEngine.EventSystems;

public class LineStart : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void OnDragCallback(LineStart _lineStart);
    public OnDragCallback onBeginDragCB;
    public OnDragCallback onDragCB;
    public OnDragCallback onEndDragCB;

    internal LineEnd connectedTo;
    internal UnityEngine.UI.Image lineImg;

    internal Transform thisTransform;

    private Transform icon;
    private bool dragging;

    void Start()
    {
        thisTransform = this.transform;
        icon = thisTransform.GetChild(0);
    }
    public void Connected(LineEnd _lineEnd, UnityEngine.UI.Image _lineImg)
    {
        connectedTo = _lineEnd;
        lineImg = _lineImg;
        this.enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        icon.localScale = new Vector2(2f, 2f);
        dragging = true;
        onBeginDragCB?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        onDragCB?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.localScale = Vector2.one;
        onEndDragCB?.Invoke(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (dragging) return;
        icon.localScale = new Vector2(2f,2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (dragging) return;
        icon.localScale = Vector2.one;
    }
}
