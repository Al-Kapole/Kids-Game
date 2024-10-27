using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public enum GameState { MainMenu, LevelSelection, Coloring, ColorByNum, ShadesGame, FindNumber, MatchingLines }


    public float transitionTime;
    public GameObject basicMenu;
    public LevelSelection levelSelection;
    public DrawingCanvas drawingCannvas;
    public ColorByNum colorByNum;
    public ShadesGame shadesGame;
    public FindNumberGame findNumber;
    public MatchingLine matchingLines;
    public GameObject backBtn;
    public GameObject block;

    private GameState currentState;
    private UnityEngine.Events.UnityEvent onTransitionComplete = new UnityEngine.Events.UnityEvent();

    public void BackPressed()
    {
        switch(currentState)
        {
            case GameState.LevelSelection:
                BackFromLevelSelection();
                break;
            case GameState.Coloring:
                BackFromColoring();
                break;
            case GameState.ColorByNum:
                BackFromColorByNum();
                break;
            case GameState.ShadesGame:
                BackFromShadesGame();
                break;
            case GameState.FindNumber:
                BackFromFindNumber();
                break;
            case GameState.MatchingLines:
                BackFromMatchingLines();
                break;
        }
    }

    public void ToLevelSelection(GameTypeBtn _button)
    {
        StartCoroutine(WindowTransitionUp(basicMenu, levelSelection.gameObject));
        levelSelection.Load(_button.gameType);
        currentState = GameState.LevelSelection;
    }
    public void BackFromLevelSelection()
    {
        StartCoroutine(WindowTransitionDown(levelSelection.gameObject, basicMenu));
        onTransitionComplete.AddListener(levelSelection.ClearButtons);
        currentState = GameState.MainMenu;
    }

    #region Coloring Game
    public void ToColoring(Minigame _minigame)
    {
        StartCoroutine(WindowTransitionUp(levelSelection.gameObject, drawingCannvas.gameObject));
        drawingCannvas.Load(_minigame);
        currentState = GameState.Coloring;
    }
    public void BackFromColoring()
    {
        StartCoroutine(WindowTransitionDown(drawingCannvas.gameObject, levelSelection.gameObject));
        onTransitionComplete.AddListener(drawingCannvas.DeleteCanvas);
        currentState = GameState.LevelSelection;
    }
    #endregion

    #region Color By Num
    public void ToColorByNum(Minigame _minigame)
    {
        StartCoroutine(WindowTransitionUp(levelSelection.gameObject, colorByNum.gameObject));
        colorByNum.Load(_minigame);
        currentState = GameState.ColorByNum;
    }
    public void BackFromColorByNum()
    {
        StartCoroutine(WindowTransitionDown(colorByNum.gameObject, levelSelection.gameObject));
        onTransitionComplete.AddListener(colorByNum.Clean);
        currentState = GameState.LevelSelection;
    }
    #endregion

    #region Shades
    public void ToShadesGame(Minigame _minigame)
    {
        StartCoroutine(WindowTransitionUp(levelSelection.gameObject, shadesGame.gameObject));
        shadesGame.Load(_minigame);
        onTransitionComplete.AddListener(shadesGame.Ready);
        currentState = GameState.ShadesGame;
    }
    public void BackFromShadesGame()
    {
        StartCoroutine(WindowTransitionDown(shadesGame.gameObject, levelSelection.gameObject));
        onTransitionComplete.AddListener(shadesGame.Clean);
        currentState = GameState.LevelSelection;
    }
    #endregion

    #region Find number
    public void ToFindNumber(Minigame _minigame)
    {
        StartCoroutine(WindowTransitionUp(levelSelection.gameObject, findNumber.gameObject));
        findNumber.Load(_minigame);
        currentState = GameState.FindNumber;
    }
    public void BackFromFindNumber()
    {
        StartCoroutine(WindowTransitionDown(findNumber.gameObject, levelSelection.gameObject));
        onTransitionComplete.AddListener(findNumber.Clean);
        currentState = GameState.LevelSelection;
    }
    #endregion

    #region Matching Lines
    public void ToMatchingLines(Minigame _minigame)
    {
        StartCoroutine(WindowTransitionUp(levelSelection.gameObject, matchingLines.gameObject));
        matchingLines.Load(_minigame);
        currentState = GameState.MatchingLines;
    }
    public void BackFromMatchingLines()
    {
        StartCoroutine(WindowTransitionDown(matchingLines.gameObject, levelSelection.gameObject));
        onTransitionComplete.AddListener(matchingLines.Clean);
        currentState = GameState.LevelSelection;
    }
    #endregion



    private IEnumerator WindowTransitionUp(GameObject _from, GameObject _to)
    {
        backBtn.SetActive(false);
        block.SetActive(true);

        _to.SetActive(true);
        RectTransform fRect = _from.GetComponent<RectTransform>();
        Vector2 fAnchorMin = fRect.anchorMin;
        Vector2 fOffsetMax = fRect.offsetMax;

        RectTransform tRect = _to.GetComponent<RectTransform>();
        Vector2 tAnchorMax = new Vector2(1, 0);
        Vector2 tOffsetMin = new Vector2(0, -1080);
        tRect.anchorMax = tAnchorMax;
        tRect.offsetMin = tOffsetMin;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / transitionTime;
            fRect.anchorMin = Vector2.Lerp(fAnchorMin, new Vector2(0, 1), t);
            fRect.offsetMax = Vector2.Lerp(fOffsetMax, new Vector2(0, 1080), t);

            tRect.anchorMax = Vector2.Lerp(tAnchorMax, new Vector2(1, 1), t);
            tRect.offsetMin = Vector2.Lerp(tOffsetMin, new Vector2(0, 0), t);
            yield return null;
        }
        _from.gameObject.SetActive(false);

        block.SetActive(false);
        backBtn.SetActive(true);
        onTransitionComplete?.Invoke();
        onTransitionComplete?.RemoveAllListeners();
    }
    private IEnumerator WindowTransitionDown(GameObject _from, GameObject _to)
    {
        backBtn.SetActive(false);
        block.SetActive(true);

        _to.SetActive(true);
        RectTransform fRect = _from.GetComponent<RectTransform>();
        Vector2 fAnchorMax = fRect.anchorMax;
        Vector2 fOffsetMin = fRect.offsetMin;

        RectTransform tRect = _to.GetComponent<RectTransform>();
        Vector2 tAnchorMin = tRect.anchorMin;
        Vector2 tOffsetMax = tRect.offsetMax;
        tRect.anchorMin = tAnchorMin;
        tRect.offsetMax = tOffsetMax;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / transitionTime;
            fRect.anchorMax = Vector2.Lerp(fAnchorMax, new Vector2(1, 0), t);
            fRect.offsetMin = Vector2.Lerp(fOffsetMin, new Vector2(0, -1080), t);

            tRect.anchorMin = Vector2.Lerp(tAnchorMin, new Vector2(0, 0), t);
            tRect.offsetMax = Vector2.Lerp(tOffsetMax, new Vector2(0, 0), t);
            yield return null;
        }
        _from.gameObject.SetActive(false);

        block.SetActive(false);
        if(currentState != GameState.MainMenu)
            backBtn.SetActive(true);
        onTransitionComplete?.Invoke();
        onTransitionComplete?.RemoveAllListeners();
    }
}
