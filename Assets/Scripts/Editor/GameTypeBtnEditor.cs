using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(GameTypeBtn))]
public class GameTypeBtnEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gameType"));
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(10);
        base.OnInspectorGUI();
    }
}
