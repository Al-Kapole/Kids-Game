using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineEnd : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool _connected;
    public bool connected
    {
        get { return _connected; }
        set
        {
            _connected = value;
            this.enabled = !value;
        }
    }
    

    public delegate void OnDropCallback(LineEnd _lineEnd);
    public OnDropCallback onDropCB;


    private Transform icon;

    void Start()
    {
        icon = transform.GetChild(0);
    }


    public void OnDrop(PointerEventData eventData)
    {
        onDropCB?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        icon.localScale = new Vector2(2f,2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        icon.localScale = Vector2.one;
    }
}
