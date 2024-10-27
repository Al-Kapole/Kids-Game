using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BrushToggle : MonoBehaviour
{
    public RectTransform selector;
    public GameObject eraserBG;
    public GameObject paintBG;
    public Image brushColorImg;
    public bool toggleOn
    {
        set
        {
            StartCoroutine(MoveSelector(value));
        }
    }
    public Color brushColor
    {
        set
        {
            brushColorImg.color = value;
        }
    }

    private Toggle thisToggle;

    private void Start()
    {
        thisToggle = GetComponent<Toggle>();
    }

    public void SetToggle(bool _value)
    {
        thisToggle.isOn = _value;
        toggleOn = _value;
    }

    private IEnumerator MoveSelector(bool _value)
    {
        Vector2 currPos = selector.anchoredPosition;
        Vector2 newPos = new Vector2(_value ? 50: -50, 0); 
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime * 15f;
            selector.anchoredPosition = Vector2.Lerp(currPos, newPos, t);
            yield return null;
        }
        eraserBG.SetActive(_value);
        paintBG.SetActive(!_value);
        brushColorImg.gameObject.SetActive(!_value);
    }
}
