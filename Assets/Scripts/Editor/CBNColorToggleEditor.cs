using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(CBNColorToggle))]
public class CBNColorToggleEditor : ToggleEditor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("onCompleteImg"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("numText"));
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(10);
        base.OnInspectorGUI();
    }
}
