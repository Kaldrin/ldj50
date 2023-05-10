using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EndLevelTrigger))]
public class EndLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector(); // for other non-HideInInspector fields

        EndLevelTrigger script = (EndLevelTrigger)target;

        // draw checkbox for the bool
        //script.nextLevelInHierarchy = EditorGUILayout.Toggle("Next Level In Hierarchy", script.nextLevelInHierarchy);
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("nextLevelInHierarchy"));
        if (!script.nextLevelInHierarchy) // if bool is true, show other fields
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("nextLevel"));
        }
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnCorridor"));
        if (script.spawnCorridor)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("endingSide"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}