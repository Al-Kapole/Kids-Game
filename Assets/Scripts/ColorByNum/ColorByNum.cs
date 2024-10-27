using UnityEngine;
using UnityEngine.UI;

public class ColorByNum : MonoBehaviour
{
    public Transform canvasHolder;
    public GameObject loading;
    public Image learningImg;
    public Image coloredUI;
    public Text info;
    public Transform colorsHolder;
    public GameObject togglePrefab;
    public GameObject completedBtn;
    
    private CBNFileInfo fileInfo;

    private CBNColorToggle[] toggles;
    private Color drawingColor = Color.black;
    private int currentColorNum;

    private Image currImg;
    private Tools.PgMouseOrTouchInput pgmoti;

    void Start()
    {
        pgmoti = Tools.PgMouseOrTouchInput.Instance;
    }

    public void Load(Minigame _minigame)
    {
        gameObject.SetActive(true);
        learningImg.sprite = _minigame.learningImage;
        coloredUI.color = completedBtn.GetComponent<Image>().color = _minigame.uiColor;
        completedBtn.SetActive(false);
        info.text = _minigame.info;
        LoadImages(_minigame.minigamePrefab);
    }

    public void Clean()
    {
        Destroy(fileInfo.gameObject);
        int length = toggles.Length;
        for (int i = 0; i < length; i++)
            Destroy(toggles[i].gameObject);
        toggles = null;
        gameObject.SetActive(false);
    }

    private void LoadImages(GameObject _minigamePrefab)
    {
        fileInfo = Instantiate(_minigamePrefab.gameObject, canvasHolder, false).GetComponent<CBNFileInfo>();
        fileInfo.transform.SetSiblingIndex(0);
        fileInfo.GetImagesObjects();
        LoadToggles(fileInfo);
    }

    private void LoadToggles(CBNFileInfo _fileInfo)
    {
        int length = _fileInfo.parts.Length;
        toggles = new CBNColorToggle[length];
        for (int i = 0; i < length; i++)
        {
            CBNColorToggle toggle = Instantiate(togglePrefab, colorsHolder, false).GetComponent<CBNColorToggle>();
            toggle.btnColor = _fileInfo.parts[i].color;
            toggle.numText.text = (i + 1).ToString();
            toggle.group = colorsHolder.GetComponent<ToggleGroup>();
            toggle.onValueChanged.AddListener(delegate { SetColor(toggle); });
            toggles[i] = toggle;
        }
        toggles[0].isOn = true;
    }

    void Update()
    {
        GameObject obj;
        if (pgmoti.InputControlDown(out obj))
        {
            if (obj == null) return;
            if(fileInfo.IsUncoloredImage(obj))
            {
                if(fileInfo.IsCorrectNum(currentColorNum))
                {
                    obj.GetComponent<Image>().color = drawingColor;
                    if (fileInfo.NumIsComplete(currentColorNum))
                    {
                        toggles[currentColorNum].completed = true;
                        SetNextAvailableToggle();
                    }
                }
                else
                    toggles[currentColorNum].Indicate();
            }
        }
    }
    private void SetNextAvailableToggle()
    {
        int completed = 0;
        int length = toggles.Length;
        for(int i = 0; i < length; i++)
        {
            currentColorNum++;
            if (currentColorNum == length)
                currentColorNum = 0;
            if (!toggles[currentColorNum].completed)
            {
                toggles[currentColorNum].isOn = true;
                break;
            }
            else
                completed++;
        }
        if (completed == length)
            completedBtn.SetActive(true);
    }


    private void SetColor(CBNColorToggle _toggle)
    {
        if (_toggle.isOn)
        {
            currentColorNum = _toggle.transform.GetSiblingIndex();
            SetColor(_toggle.btnColor);
        }
    }
    public void SetColor(Color color)
    {
        drawingColor = color;
    }
}
