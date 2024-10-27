using UnityEngine;
using UnityEngine.UI;

public class MatchingLine : MonoBehaviour
{
    public GameObject linePrefab;
    public Transform canvasHolder;
    public Image coloredUI;
    public Text info;
    public GameObject completedBtn;

    private RectTransform lineImg;
    private MLFileInfo fileInfo;
    private Camera  mainCam;
    private float distance;
    private float AngleRad;
    private float AngleDeg;
    private Vector2 inputPos;
    private Tools.PgMouseOrTouchInput pgmoti;
    private LineStart currLineStart;

    private void Start()
    {
        mainCam = Camera.main;
        pgmoti = Tools.PgMouseOrTouchInput.Instance;
    }

    public void Clean()
    {
        Destroy(fileInfo.gameObject);
        gameObject.SetActive(false);
    }

    public void Load(Minigame _minigame)
    {
        gameObject.SetActive(true);
        coloredUI.color = completedBtn.GetComponent<Image>().color = _minigame.uiColor;
        completedBtn.SetActive(false);
        info.text = _minigame.info;
        LoadMG(_minigame.minigamePrefab);
    }
    private void LoadMG(GameObject _minigamePrefab)
    {
        fileInfo = Instantiate(_minigamePrefab, canvasHolder, false).GetComponent<MLFileInfo>();
        fileInfo.transform.SetSiblingIndex(0);

        int length = fileInfo.connections.Length;
        for (int i = 0; i < length; i++)
        {
            fileInfo.connections[i].lineStart.onBeginDragCB = OnBeginDrag;
            fileInfo.connections[i].lineStart.onDragCB = OnDrag;
            fileInfo.connections[i].lineStart.onEndDragCB = OnEndDrag;
            fileInfo.connections[i].lineEnd.onDropCB = OnDrop;
        }
    }

    private void OnBeginDrag(LineStart _obj)
    {
        lineImg = Instantiate(linePrefab, _obj.thisTransform, false).GetComponent<RectTransform>();
        lineImg.transform.SetSiblingIndex(0);

        lineImg.position = _obj.transform.position;
        currLineStart = _obj;
    }
    private void OnDrag(LineStart _obj)
    {
        inputPos = pgmoti.InputPosition();
        Rotation();
        CalculateWidth();
    }
    private void OnEndDrag(LineStart _obj)
    {
        Destroy(lineImg.gameObject);
    }
    private void OnDrop(LineEnd _lineEnd)
    {
        _lineEnd.connected = true;
        Vector2 objPos = _lineEnd.transform.position;
        currLineStart.Connected(_lineEnd, lineImg.GetComponent<Image>());
        AngleRad = Mathf.Atan2(objPos.y - lineImg.position.y, objPos.x - lineImg.position.x);
        AngleDeg = (180 / Mathf.PI) * AngleRad;
        lineImg.rotation = Quaternion.Euler(0, 0, AngleDeg);
        distance = Vector2.Distance(lineImg.position, objPos);
        lineImg.offsetMax = new Vector2(distance * 2, 5);
        fileInfo.totalCompleted++;

        int correct;
        if (fileInfo.Completed(out correct))
        {
            completedBtn.SetActive(true);
            completedBtn.GetComponentInChildren<Text>().text = "Correct: " + correct + "/" + fileInfo.connections.Length;
        }
    }


    private void Rotation()
    {
        AngleRad = Mathf.Atan2(inputPos.y - lineImg.position.y, inputPos.x - lineImg.position.x);
        AngleDeg = (180 / Mathf.PI) * AngleRad;
        lineImg.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }
    private void CalculateWidth()
    {
        distance = Vector2.Distance(lineImg.position, inputPos);
        //Calculation should be adjusted according to the game's resolution.
        lineImg.offsetMax = new Vector2(distance * 2, 5);//This will only work correctly for 960x540 resolution.
    }
}
