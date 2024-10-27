using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ShadesGame : MonoBehaviour
{
    public Transform canvasHolder;
    public GameObject loading;
    public Image learningImg;
    public Image coloredUI;
    public Text info;
    public GameObject completedBtn;

    private Vector3 originalPos;
    private Vector3 offset;
    private Transform draggableObj;
    private bool dragging;
    private SGFileInfo fileInfo;
    private Tools.PgMouseOrTouchInput pgmoti;
    private IEnumerator moveToPosCorr;
    private GeneralGameInfo ggi;

    void Start()
    {
        pgmoti = Tools.PgMouseOrTouchInput.Instance;
        ggi = GeneralGameInfo.Instance;
        moveToPosCorr = MoveToPos(originalPos, 0.35f);
    }

    public void Clean()
    {
        Destroy(fileInfo.gameObject);
        gameObject.SetActive(false);
    }

    public void Load(Minigame _minigame)
    {
        gameObject.SetActive(true);
        learningImg.sprite = _minigame.learningImage;
        coloredUI.color = completedBtn.GetComponent<Image>().color = _minigame.uiColor;
        completedBtn.SetActive(false);
        info.text = _minigame.info;
        LoadMG(_minigame.minigamePrefab);
    }

    private void LoadMG(GameObject _minigamePrefab)
    {
        fileInfo = Instantiate(_minigamePrefab, canvasHolder, false).GetComponent<SGFileInfo>();
        fileInfo.transform.SetSiblingIndex(0);

        draggableObj = fileInfo.colored.transform;


        EventTrigger et = draggableObj.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry beginDragEntry = new EventTrigger.Entry();
        beginDragEntry.eventID = EventTriggerType.BeginDrag;
        beginDragEntry.callback.AddListener(OnBeginDrag);
        et.triggers.Add(beginDragEntry);

        EventTrigger.Entry dragEntry = new EventTrigger.Entry();
        dragEntry.eventID = EventTriggerType.Drag;
        dragEntry.callback.AddListener(OnPointerDrag);
        et.triggers.Add(dragEntry);


        EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
        endDragEntry.eventID = EventTriggerType.EndDrag;
        endDragEntry.callback.AddListener(OnEndDrag);
        et.triggers.Add(endDragEntry);
    }

    public void Ready()
    {
        originalPos = draggableObj.transform.position;
    }

    private void OnBeginDrag(BaseEventData _eventData)
    {
        StopCoroutine(moveToPosCorr);
        offset = draggableObj.transform.position - Input.mousePosition;
    }
    private void OnPointerDrag(BaseEventData _eventData)
    {
        draggableObj.transform.position = Input.mousePosition + offset;
    }
    private void OnEndDrag(BaseEventData _eventData)
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        int length = fileInfo.shades.Length;
        for(int i = 0; i < length; i++)
        {
            Shade shade = fileInfo.shades[i];
            float distance = Vector3.Distance(shade.shadeObject.position, draggableObj.transform.position);
            if(distance < 45)
            {
                if (shade.isCorrect)
                {
                    draggableObj.gameObject.GetComponent<EventTrigger>().enabled = false;
                    moveToPosCorr = MoveToPos(shade.shadeObject.position, 0.3f);
                    StartCoroutine(moveToPosCorr);
                    StartCoroutine(Resizing(shade.shadeObject, 1.3f, 0.3f));
                    return;
                }
                else
                    ggi.Tremple(this, shade.shadeObject, 3, 0.5f);
                break;
            }
        }
        moveToPosCorr = MoveToPos(originalPos, 0.35f);
        StartCoroutine(moveToPosCorr);
    }

    private IEnumerator MoveToPos(Vector3 _pos, float _sec)
    {
        Vector3 currentPos = draggableObj.transform.position;
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime / _sec;
            draggableObj.transform.position = Vector3.Lerp(currentPos, _pos, t);
            yield return null;
        }
    }

    public IEnumerator Resizing(Transform _transform, float _scale, float _sec)
    {
        Vector3 scale = new Vector3(_scale, _scale);
        float halfSec = _sec * 0.5f;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / halfSec;
            _transform.localScale = Vector3.Lerp(Vector3.one, scale, t);
            yield return null;
        }
        t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime / halfSec;
            _transform.localScale = Vector3.Lerp(Vector3.one, scale, t);
            yield return null;
        }
        completedBtn.SetActive(true);
    }
}
