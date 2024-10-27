using UnityEngine;
using UnityEngine.UI;

public class NumPadBtn : Button
{
    public int number;
    public Image bgImage;
    public Text numTxt;

    protected override void Awake()
    {
        base.Awake();
        numTxt.text = number.ToString();
    }
}
