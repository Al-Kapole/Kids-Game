using UnityEngine;

public class SGFileInfo : MonoBehaviour
{
    public GameObject colored;
    public Shade[] shades;
}


[System.Serializable]
public struct Shade
{
    public bool isCorrect;
    public Transform shadeObject;
}