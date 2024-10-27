using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public enum GameType { Coloring, ColorByNum, Shades, FindNumber, MatchingLine}

    public MainMenu mainMenu;
    public Transform btnsHolder;
    public GameObject buttonPrefab;
    
    private LevelButton[] buttons;
    

    public void Load(GameType _type)
    {
        Minigame[] levels;
        int length;
        switch(_type)
        {
            case GameType.Coloring:
                levels = Resources.LoadAll<Minigame>("Paintings");
                length = levels.Length;
                buttons = new LevelButton[length];
                for (int i = 0; i < length; i++)
                {
                    buttons[i] = Instantiate(buttonPrefab, btnsHolder, false).GetComponent<LevelButton>();
                    Minigame minigame = levels[i];
                    buttons[i].Set(minigame.title, minigame.uiColor);
                    buttons[i].GetComponent<Button>().onClick.AddListener(delegate { mainMenu.ToColoring(minigame); });
                }
                break;
            case GameType.ColorByNum:
                levels = Resources.LoadAll<Minigame>("ColorByNum");
                length = levels.Length;
                buttons = new LevelButton[length];
                for (int i = 0; i < length; i++)
                {
                    buttons[i] = Instantiate(buttonPrefab, btnsHolder, false).GetComponent<LevelButton>();
                    Minigame minigame = levels[i];
                    buttons[i].Set(minigame.title, minigame.uiColor);
                    buttons[i].GetComponent<Button>().onClick.AddListener(delegate { mainMenu.ToColorByNum(minigame); });
                }
                break;
            case GameType.Shades:
                levels = Resources.LoadAll<Minigame>("Shades");
                length = levels.Length;
                buttons = new LevelButton[length];
                for (int i = 0; i < length; i++)
                {
                    buttons[i] = Instantiate(buttonPrefab, btnsHolder, false).GetComponent<LevelButton>();
                    Minigame minigame = levels[i];
                    buttons[i].Set(minigame.title, minigame.uiColor);
                    buttons[i].GetComponent<Button>().onClick.AddListener(delegate { mainMenu.ToShadesGame(minigame); });
                }
                break;
            case GameType.FindNumber:
                levels = Resources.LoadAll<Minigame>("FindNumber");
                length = levels.Length;
                buttons = new LevelButton[length];
                for (int i = 0; i < length; i++)
                {
                    buttons[i] = Instantiate(buttonPrefab, btnsHolder, false).GetComponent<LevelButton>();
                    Minigame minigame = levels[i];
                    buttons[i].Set(minigame.title, minigame.uiColor);
                    buttons[i].GetComponent<Button>().onClick.AddListener(delegate { mainMenu.ToFindNumber(minigame); });
                }
                break;
            case GameType.MatchingLine:
                levels = Resources.LoadAll<Minigame>("MatchingLines");
                length = levels.Length;
                buttons = new LevelButton[length];
                for (int i = 0; i < length; i++)
                {
                    buttons[i] = Instantiate(buttonPrefab, btnsHolder, false).GetComponent<LevelButton>();
                    Minigame minigame = levels[i];
                    buttons[i].Set(minigame.title, minigame.uiColor);
                    buttons[i].GetComponent<Button>().onClick.AddListener(delegate { mainMenu.ToMatchingLines(minigame); });
                }
                break;
        }
    }

    public void ClearButtons()
    {
        int length = buttons.Length;
        for (int i = 0; i < length; i++)
            Destroy(buttons[i].gameObject);
        buttons = null;
    }
}
