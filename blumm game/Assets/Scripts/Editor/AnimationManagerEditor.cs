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
    private void OnEnable()
    {
        man = target as AnimationManager;
        if (man._contr != null)
        {

            man.stateNames.Clear();
            man.stateLengths.Clear();
            for (int i = 0; i < man._contr.layers[0].stateMachine.states.Length; i++)
            {
                man.stateNames.Add(man._contr.layers[0].stateMachine.states[i].state.name);
                man.stateLengths.Add(man._contr.layers[0].stateMachine.states[i].state.motion.averageDuration);
            }

        }
    }
    public override void OnInspectorGUI()
    {
        
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        man._contr = EditorGUILayout.ObjectField(man._contr, typeof(AnimatorController), false) as AnimatorController;

        //_animController = source as AnimatorController;
        if (EditorGUI.EndChangeCheck())
        {
            //man._contr = _animController;
            Debug.Log("ditre");
            if (man._contr != null)
            {
                man.stateNames.Clear();
                man.stateLengths.Clear();
                for (int i = 0; i < man._contr.layers[0].stateMachine.states.Length; i++)
                {
                    man.stateNames.Add(man._contr.layers[0].stateMachine.states[i].state.name);
                    man.stateLengths.Add(man._contr.layers[0].stateMachine.states[i].state.motion.averageDuration);
                }

            }
            //EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        //serializedObject.ApplyModifiedProperties();
        
        PrefabUtility.RecordPrefabInstancePropertyModifications(target);
    }
}
