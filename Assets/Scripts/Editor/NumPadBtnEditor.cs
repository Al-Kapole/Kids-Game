using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(NumPadBtn))]
public class NumPadBtnEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("number"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bgImage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("numTxt"));
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(10);
        base.OnInspectorGUI();
    }
}
