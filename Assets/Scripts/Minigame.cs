using UnityEngine;

[CreateAssetMenu(fileName = "Minigame", menuName = "Kapolas/Minigame", order = 1)]
public class Minigame : ScriptableObject
{
    public string title;
    public Sprite learningImage;
    public Color uiColor;
    public string info;
    public GameObject minigamePrefab;
}
