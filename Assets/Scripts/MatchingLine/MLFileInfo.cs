using UnityEngine;

public class MLFileInfo : MonoBehaviour
{
    public Connection[] connections;
    public int totalCompleted;

    public bool Completed(out int _totalCorrect)
    {
        _totalCorrect = 0;
        if (totalCompleted == connections.Length)
        {
            _totalCorrect = CorrectConnections();
            return true;
        }
        return false;
    }

    public int CorrectConnections()
    {
        int correct = 0;
        int length = connections.Length;
        for (int i = 0; i < length; i++)
        {
            Connection connection = connections[i];
            if (connection.lineStart.connectedTo == connection.lineEnd)
            {
                connection.lineStart.lineImg.color = Color.green;
                correct++;
            }
            else
                connection.lineStart.lineImg.color = Color.red;
        }
        return correct;
    }
}

[System.Serializable]
public struct Connection
{
    public LineStart lineStart;
    public LineEnd lineEnd;
}