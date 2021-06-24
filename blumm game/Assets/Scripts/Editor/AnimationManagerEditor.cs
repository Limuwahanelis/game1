using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(AnimationManager))]
public class AnimationManagerEditor : Editor
{
    public AnimationManager man;
    SerializedProperty control;
    SerializedProperty stateNames;
    SerializedProperty stateLengths;
    private void OnEnable()
    {
        control = serializedObject.FindProperty("animatorController");
        stateNames = serializedObject.FindProperty("stateNames");
        stateLengths = serializedObject.FindProperty("stateLengths");
        man = target as AnimationManager;
        serializedObject.Update();

        if (man.animatorController != null)
        {
            stateLengths.ClearArray();
            stateNames.ClearArray();
            for (int i = 0; i < man.animatorController.layers[0].stateMachine.states.Length; i++)
            {
                AnimatorState state = man.animatorController.layers[0].stateMachine.states[i].state;
                stateLengths.InsertArrayElementAtIndex(stateLengths.arraySize);
                stateNames.InsertArrayElementAtIndex(stateNames.arraySize);
                if (state.motion==null)
                {
                    stateLengths.GetArrayElementAtIndex(stateLengths.arraySize - 1).floatValue = 0;
                    stateNames.GetArrayElementAtIndex(stateNames.arraySize - 1).stringValue = "Empty";
                }
                else
                {
                    stateLengths.GetArrayElementAtIndex(stateLengths.arraySize - 1).floatValue = state.motion.averageDuration;
                    stateNames.GetArrayElementAtIndex(stateNames.arraySize - 1).stringValue = state.name;
                }
                
                
            }

        }
        serializedObject.ApplyModifiedProperties();
    }
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(control);
        serializedObject.ApplyModifiedProperties();

        serializedObject.Update();
       if (EditorGUI.EndChangeCheck())
       {
            if (man.animatorController != null)
            {
                stateLengths.ClearArray();
                stateNames.ClearArray();
                for (int i = 0; i < man.animatorController.layers[0].stateMachine.states.Length; i++)
                {
                    AnimatorState state = man.animatorController.layers[0].stateMachine.states[i].state;
                    stateNames.InsertArrayElementAtIndex(stateNames.arraySize);
                    stateLengths.InsertArrayElementAtIndex(stateLengths.arraySize);
                    if (state.motion == null)
                    {
                        stateLengths.GetArrayElementAtIndex(stateLengths.arraySize - 1).floatValue = 0;
                        stateNames.GetArrayElementAtIndex(stateNames.arraySize - 1).stringValue = "Empty";
                    }
                    else
                    {
                        stateLengths.GetArrayElementAtIndex(stateLengths.arraySize - 1).floatValue = state.motion.averageDuration;
                        stateNames.GetArrayElementAtIndex(stateNames.arraySize - 1).stringValue = state.name;
                    }
                   
                }

            }
        }
        serializedObject.ApplyModifiedProperties();


        PrefabUtility.RecordPrefabInstancePropertyModifications(man);
    }

    
}
