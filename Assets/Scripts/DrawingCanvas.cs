using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Kapolas.Tools.Extensions;


public class DrawingCanvas : MonoBehaviour
{
    public Color eraseColor = Color.white;
    public BrushToggle brushToggle;
    public Transform canvasHolder;
    public GameObject tools;
    public GameObject block;
    public GameObject loading;
    public int offsetY;
    public Toggle currToggle;
    public Image learningImg;
    public Image coloredUI;
    public Text info;

    private Image[] drawableImages;
    private Camera mainCam;
    private Mode mode = Mode.Pencil;
    private int drawingSize = 32;
    private Color drawingColor = Color.black;
    private Vector2 pencilFrom;
    private Vector2 pencilTo;
    private bool drawPencil;
    private Texture2D stickerTexture;
    private Vector2 crossHairPos;
    private string currPrefix;
    private UnityEngine.Events.UnityAction onThumbnailSaved;

    private Image currImg;
    private TextureDrawingAux currentTexDrawingAux;
    private Dictionary<Image, TextureDrawingAux> itda;
    private Tools.PgMouseOrTouchInput pgmoti;

    void Start()
    {
        mainCam = Camera.main;
        pgmoti = Tools.PgMouseOrTouchInput.Instance;
        drawingColor = brushToggle.brushColor = currToggle.image.color;
        StartCoroutine(ToolsRefresh());
    }
    private IEnumerator ToolsRefresh()
    {
        yield return null;
        tools.SetActive(false);
        yield return null;
        tools.SetActive(true);
    }

    public void Load(Minigame _painting)
    {
        gameObject.SetActive(true);
        learningImg.sprite = _painting.learningImage;
        coloredUI.color = _painting.uiColor;
        info.text = _painting.info;
        StartCoroutine(LoadImages(_painting.minigamePrefab));
    }

    public void DeleteCanvas()
    {
        Destroy(drawableImages[0].transform.parent.gameObject);
        gameObject.SetActive(false);
    }

    private IEnumerator LoadImages(GameObject _obj)
    {
        currPrefix = _obj.name;
        GameObject drawing = Instantiate(_obj, canvasHolder, false);
        drawing.transform.SetSiblingIndex(0);

        drawableImages = drawing.transform.GetObjecstWithoutTag<Image>(Const.TAG_Lines);

        itda = new Dictionary<Image, TextureDrawingAux>();
        TextureDrawingAux textureDrawingAux;

        int length = drawableImages.Length;
        for (int i = 0; i < length; i++)
        {
            string filename = currPrefix + drawableImages[i].name + ".png";

            EventTrigger et = drawableImages[i].gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry dragEntry = new EventTrigger.Entry();
            dragEntry.eventID = EventTriggerType.Drag;
            dragEntry.callback.AddListener(OnPointerDrag);
            et.triggers.Add(dragEntry);

            if (drawableImages[i].sprite == null)
            {
                Texture2D tex;
                if (LoadImage(filename, out tex))
                {
                    textureDrawingAux = new TextureDrawingAux(tex, false);
                    Sprite sprite = Sprite.Create(textureDrawingAux.Texture, new Rect(0, 0, textureDrawingAux.Width, textureDrawingAux.Height), Vector2.one * 0.5f);
                    drawableImages[i].sprite = sprite;
                    textureDrawingAux.fileName = filename;
                }
                else
                {
                    int width = (int)drawableImages[i].rectTransform.rect.width;
                    int height = (int)drawableImages[i].rectTransform.rect.height;
                    textureDrawingAux = new TextureDrawingAux(width, height, 1);
                    textureDrawingAux.Clear(eraseColor);
                    //Create a sprite from the texture and display it in imageCanvas
                    Sprite sprite = Sprite.Create(textureDrawingAux.Texture, new Rect(0, 0, textureDrawingAux.Width, textureDrawingAux.Height), Vector2.one * 0.5f);
                    drawableImages[i].sprite = sprite;
                    textureDrawingAux.SaveToFile(filename);
                }
            }
            else
            {
                Texture2D tex;
                if (LoadImage(filename, out tex))
                {
                    textureDrawingAux = new TextureDrawingAux(tex, true);
                    Sprite sprite = Sprite.Create(textureDrawingAux.Texture, new Rect(0, 0, textureDrawingAux.Width, textureDrawingAux.Height), Vector2.one * 0.5f);
                    drawableImages[i].sprite = sprite;
                    textureDrawingAux.fileName = filename;
                }
                else
                {
                    textureDrawingAux = new TextureDrawingAux(drawableImages[i].sprite.texture, true);
                    textureDrawingAux.SaveToFile(filename);
                }
                drawableImages[i].alphaHitTestMinimumThreshold = 0.1f;
            }
            itda.Add(drawableImages[i], textureDrawingAux);
            yield return null;
        }
        yield return null;
    }

    private bool LoadImage(string _fileName, out Texture2D _tex)
    {
        //If a saved file exists, load it and draw it on the texture, else start a new one
        if (File.Exists(Application.persistentDataPath + "/" + _fileName))
        {
            byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + _fileName);

            _tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            _tex.LoadImage(bytes);
            return true;
        }
        _tex = null;
        return false;
    }

    void Update()
    {
        GameObject obj;
        if (pgmoti.InputControlDown(out obj))
        {
            if (obj == null) return;
            currImg = obj.GetComponent<Image>();
            if (currImg == null || !itda.ContainsKey(currImg))
            {
                currImg = null;
                currentTexDrawingAux = null;
                return;
            }
            Vector2 localCursor;
            //calculate the coordinates of the cursor on the texture
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(currImg.rectTransform, Input.mousePosition, null, out localCursor))
                return;
            currentTexDrawingAux = itda[currImg];
            localCursor.x += currImg.rectTransform.pivot.x * currImg.rectTransform.rect.width;
            localCursor.y += currImg.rectTransform.pivot.y * currImg.rectTransform.rect.height;
            localCursor.y += offsetY;


            crossHairPos = pgmoti.InputPosition();
            crossHairPos.y += offsetY;
            
            //if it's eraser, set the color to white, else use drawing color
            Color color = mode == Mode.Eraser ? eraseColor : drawingColor;
            //draw a single circle where we touched the image

            currentTexDrawingAux.DrawCircleFill(Mathf.RoundToInt(localCursor.x), Mathf.RoundToInt(localCursor.y), drawingSize, color, mode == Mode.Eraser);
            block.SetActive(true);
        }
        else if (pgmoti.InputControlUp() && currImg != null)
        {
            currentTexDrawingAux.SaveToFile();
            currImg = null;
            currentTexDrawingAux = null;
            block.SetActive(false);
        }
        //Drawn during Update.
        //We're not doing this in the event callbacks to improve responsiveness
        if (drawPencil)
        {
            Color color = mode == Mode.Eraser ? eraseColor : drawingColor;
            drawPencil = false;

            currentTexDrawingAux.DrawPencil(pencilFrom, pencilTo, drawingSize, color, mode == Mode.Eraser);
        }
    }
    
    public void PencilSize(Slider _slider)
    {
        drawingSize = (int)_slider.value;
    }
    public void TogglePencilWithSize(int size)
    {
        mode = Mode.Pencil;
        drawingSize = size;
    }

    public void ToggleEraserWithSize(int size)
    {
        mode = Mode.Eraser;
        drawingSize = size;
    }
    public bool toggleEraser
    {
        set
        {
            if (value)
                mode = Mode.Eraser;
            else
            {
                drawingColor = currToggle.image.color;
                mode = Mode.Pencil;
            }
        }
    }
    public bool toggleOffset
    {
        set
        {
            offsetY = value ? 100 : 0;
        }
    }

    public void SetColor(Color color)
    {
        drawingColor = brushToggle.brushColor = color;
    }
    public void SetColor(Toggle _toggle)
    {
        if (_toggle.isOn)
        {
            mode = Mode.Pencil;
            brushToggle.SetToggle(false);
            SetColor(_toggle.image.color);
        }
    }
    
    public void ClearCanvas()
    {
        currentTexDrawingAux.Clear(eraseColor);
    }
    public void ClearAll()
    {
        StartCoroutine(ClearingPaintting());
    }
    private IEnumerator ClearingPaintting()
    {
        loading.SetActive(true);
        block.SetActive(true);
        foreach (TextureDrawingAux tda in itda.Values)
        {
            tda.Clear(eraseColor);
            tda.SaveToFile();
            yield return null;
        }
        block.SetActive(false);
        loading.SetActive(false);
    }

    //Callback. Traces a pencil line between the 2 points
    public void OnPointerDrag(BaseEventData eventData)
    {
        if (currImg == null)
            return;

        PointerEventData pointerData = eventData as PointerEventData;

        //calculate the coordinates of the cursor on the texture, both when we begin the drag and end it
        Vector2 crtPos;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(currImg.rectTransform, pointerData.position, pointerData.pressEventCamera, out crtPos))
            return;
        Vector2 prevPos;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(currImg.rectTransform, pointerData.position - pointerData.delta, pointerData.pressEventCamera, out prevPos))
            return;

        crossHairPos = pgmoti.InputPosition();
        crossHairPos.y += offsetY;

        //if all went ok, set to draw the line on the next frame
        pencilFrom = prevPos;
        pencilTo = crtPos;

        pencilFrom.x += currImg.rectTransform.pivot.x * currImg.rectTransform.rect.width;
        pencilFrom.y += currImg.rectTransform.pivot.y * currImg.rectTransform.rect.height;
        pencilFrom.y += offsetY;

        pencilTo.x += currImg.rectTransform.pivot.x * currImg.rectTransform.rect.width;
        pencilTo.y += currImg.rectTransform.pivot.y * currImg.rectTransform.rect.height;
        pencilTo.y += offsetY;

        drawPencil = true;
    }
    
    public enum Mode
    {
        //Set pixels traced over to set color
        Pencil = 0,
        //Set pixels traced over to white
        Eraser = 1
    }
}
