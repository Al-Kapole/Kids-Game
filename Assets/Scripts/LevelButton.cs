using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public UnityEngine.UI.Text title;
    public UnityEngine.UI.Image image;

    public void Set(string _title, Color _color)
    {
        title.text = _title;
        image.color = _color;
    }
}
