using UnityEngine;
using UnityEngine.UI;

public class CBNColorToggle : Toggle
{
    public GameObject onCompleteImg;
    public Text numText;

    private Coroutine tremple;
    private Image thisImage;

    protected override void Awake()
    {
        base.Awake();
        thisImage = GetComponent<Image>();
    }

    private bool _completed;
    internal bool completed
    {
        set
        {
            if (value)
            {
                onCompleteImg.SetActive(true);
                numText.gameObject.SetActive(false);
                interactable = false;
                _completed = true;
            }
        }
        get { return _completed; }
    }
    internal Color btnColor
    {
        set
        {
            thisImage.color = value;
        }
        get
        {
            return thisImage.color;
        }
    }

    public void Indicate()
    {
        if (tremple != null)
            StopCoroutine(tremple);
        tremple = StartCoroutine(Resizing());
    }

    private System.Collections.IEnumerator Resizing()
    {
        Transform thisTrns = GetComponent<Transform>();
        Vector3 bigScale = new Vector3(1.35f, 1.3f, 1f);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 5f;
            thisTrns.localScale = Vector3.Lerp(Vector3.one, bigScale, t);
            yield return null;
        }
        t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime * 5f;
            thisTrns.localScale = Vector3.Lerp(Vector3.one, bigScale, t);
            yield return null;
        }
    }
}
