using UnityEngine;
using System.Collections.Generic;

public class CBNFileInfo : MonoBehaviour
{
    public CBNColors[] parts;

    private List<GameObject> uncoloredList;
    private GameObject currObj;

    public bool NumIsComplete(int _num)
    {
        int completedObjects = 0;
        CBNPart[] objects = parts[_num].objects;
        int length = objects.Length;
        for(int i = 0; i < length; i++)
        {
            if (objects[i].partCompleted)
                completedObjects++;
        }
        return completedObjects == length;
    }
    //Check if the saved image is in the correct number
    public bool IsCorrectNum(int _num)
    {
        foreach (CBNPart part in parts[_num].objects)
        {
            if (part.imagePart == currObj)
            {
                part.partCompleted = true;
                part.numberPart.SetActive(false);
                uncoloredList.Remove(currObj);
                currObj = null;
                return true;
            }
        }
        currObj = null;
        return false;
    }

    //Check if the list contains the given image
    public bool IsUncoloredImage(GameObject _obj)
    {
        foreach (GameObject obj in uncoloredList)
        {
            if (obj == _obj)
            {
                currObj = _obj;
                return true;
            }
        }
        return false;
    }

    //Create List of only the images
    public List<GameObject> GetImagesObjects()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (CBNColors cbnCol in parts)
        {
            foreach (CBNPart cbnPart in cbnCol.objects)
            {
                cbnPart.imagePart.GetComponent<UnityEngine.UI.Image>().alphaHitTestMinimumThreshold = 0.1f;
                list.Add(cbnPart.imagePart);
            }
        }
        uncoloredList = list;
        return uncoloredList;
    }
}

[System.Serializable]
public class CBNColors
{
    public bool colorCompleted;
    public Color color;
    public CBNPart[] objects;
}

[System.Serializable]
public class CBNPart
{
    public bool partCompleted;
    public GameObject imagePart;
    public GameObject numberPart;
}