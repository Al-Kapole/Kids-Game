using System.Collections;
using UnityEngine;

public class FNFileInfo : MonoBehaviour
{
    public FNQuestionInfo[] questions;
    public Transform hider;
    public float[] hiderPoses;

    public bool finished { get { return currentQ == questions.Length; } }
    private int currentQ = 0;

    private void Awake()
    {
        questions = GetComponentsInChildren<FNQuestionInfo>();
    }

    public bool CheckAnswer(int _num)
    {
        if (questions[currentQ].correctNum == _num)
        {
            questions[currentQ].correctIcon.SetActive(true);
            questions[currentQ].numText.text = _num.ToString();
            currentQ++;
            if(!finished)
                StartCoroutine(HiderDown());
            return true;
        }
        return false;
    }

    private IEnumerator HiderDown()
    {
        RectTransform fRect = hider.GetComponent<RectTransform>();
        Vector2 fOffsetMax = fRect.offsetMax;
        
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 3f;
            fRect.offsetMax = Vector2.Lerp(fOffsetMax, new Vector2(0, hiderPoses[currentQ]), t);
            yield return null;
        }
    }
}
