using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools;
using Kapolas.Tools.Extensions;
using UnityEngine.EventSystems;

public class SeparateOutlines : MonoBehaviour
{
    public Image img;
    public Texture2D texture;

    private Camera mainCam;
    private PgMouseOrTouchInput pgmoti;

    void Start()
    {
        mainCam = Camera.main;
        pgmoti = PgMouseOrTouchInput.Instance;
        texture = img.sprite.texture;
        //texture.FlowFill(this, 0, 0, Color.white, null);
    }

    void Update()
    {
        GameObject obj;
        if (pgmoti.InputControlDown(out obj))
        {
            if (obj == null) return;
            Image currImg = obj.GetComponent<Image>();
            Vector2 localCursor;
            //calculate the coordinates of the cursor on the texture
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(currImg.rectTransform, pgmoti.InputPosition(), null, out localCursor))
                return;
            localCursor.x += currImg.rectTransform.pivot.x * currImg.rectTransform.rect.width;
            localCursor.y += currImg.rectTransform.pivot.y * currImg.rectTransform.rect.height;

            texture.FloodFillArea(Mathf.RoundToInt(localCursor.x), Mathf.RoundToInt(localCursor.y), Color.red);
            texture.Apply();
            //Debug.Log(Mathf.RoundToInt(localCursor.x) +" | "+ Mathf.RoundToInt(localCursor.y));
        }
    }
}
