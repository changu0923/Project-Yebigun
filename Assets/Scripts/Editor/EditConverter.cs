using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SocketConverter))]
public class EditConverter : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SocketConverter script = target as SocketConverter;
        if (GUILayout.Button("Convert"))
        {
            script.Convert();
        }
    }
}
