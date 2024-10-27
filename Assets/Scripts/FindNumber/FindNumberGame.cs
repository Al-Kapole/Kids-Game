using UnityEngine;
using UnityEngine.UI;

public class FindNumberGame : MonoBehaviour
{
    public Transform canvasHolder;
    public GameObject loading;
    //public Image learningImg;
    public Image coloredUI;
    public Text info;
    public GameObject completedBtn;
    public GameObject numpadHolder;

    private FNFileInfo fileInfo;
    private NumPadBtn[] numPad;
    private Tools.PgMouseOrTouchInput pgmoti;
    private GeneralGameInfo ggi;

    void Start()
    {
        pgmoti = Tools.PgMouseOrTouchInput.Instance;
        ggi = GeneralGameInfo.Instance;

        numPad = numpadHolder.GetComponentsInChildren<NumPadBtn>();
    }
    public void Clean()
    {
        Destroy(fileInfo.gameObject);
        gameObject.SetActive(false);
    }
    public void Load(Minigame _minigame)
    {
        EnabledNumpad(true);
        gameObject.SetActive(true);
        coloredUI.color = completedBtn.GetComponent<Image>().color = _minigame.uiColor;
        completedBtn.SetActive(false);
        info.text = _minigame.info;
        LoadMG(_minigame.minigamePrefab);
    }

    public void LoadMG(GameObject _minigamePrefab)
    {
        fileInfo = Instantiate(_minigamePrefab, canvasHolder, false).GetComponent<FNFileInfo>();
        fileInfo.transform.SetSiblingIndex(0);
    }

    public void NumPressed(NumPadBtn _btn)
    {
        if (fileInfo.CheckAnswer(_btn.number))
            EnabledNumpad(true);
        else
        {
            _btn.interactable = false;
            ggi.Tremple(this, _btn.bgImage.transform, 5, 0.3f);
        }

        if (fileInfo.finished)
        {
            EnabledNumpad(false);
            completedBtn.SetActive(true);
        }
    }

    public void EnabledNumpad(bool _value)
    {
        if (numPad == null) return;
        foreach (NumPadBtn btn in numPad)
            btn.interactable = _value;
    }
}
