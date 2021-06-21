using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    Test test;
    private void OnEnable()
    {
        test = target as Test;
        test.testList.Clear();
        for (int i = 0; i < test.size; i++)
        {
            test.testList.Add(i);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        SerializedProperty aa= serializedObject.FindProperty("testInt");
        SerializedProperty control = serializedObject.FindProperty("controlf");
        SerializedProperty list = serializedObject.FindProperty("testList");

        EditorGUILayout.PropertyField(aa);
        //test.testIntEd = EditorGUILayout.IntField(test.testIntEd);
        EditorGUILayout.PropertyField(control);
        //EditorGUILayout.ObjectField(control, typeof(AnimatorController), false) as AnimatorController;
        //serializedObject.ApplyModifiedProperties();
        serializedObject.ApplyModifiedProperties();
        PrefabUtility.RecordPrefabInstancePropertyModifications(test);
        //serializedObject.FindProperty("testInt");
    }


}
