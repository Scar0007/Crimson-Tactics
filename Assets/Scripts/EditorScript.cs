using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorScript : EditorWindow
{
    #region Variables
    int currentSelectionCount = 0;
    #endregion

    #region Builtin Methods
    public static void LaunchEditor()
    {
        var editorWin = GetWindow<EditorScript>("Toggle Obstacles");
        editorWin.Show();
    }

    private void OnGUI()
    {
        GetSelection();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Selection Count: "+ currentSelectionCount.ToString(), EditorStyles.boldLabel);
        EditorGUILayout.Space();

        //Button to disable Obstacles 

        if(GUILayout.Button("Disable Selected Obstacles", GUILayout.ExpandWidth(true), GUILayout.Height(25)))
        {
            DisableSelectedObstacles();
        }

        EditorGUILayout.EndVertical();
        // Creating Grid of 10x10 buttons 
        //Unable to attach an object to each button
        /* 
        EditorGUILayout.BeginHorizontal();
        for(int i = 0; i < 10; i++)
        {
            EditorGUILayout.BeginVertical();
            for(int j = 0; j < 10; j++)
            {
                if(GUILayout.Button("Toggle Obstacle", GUILayout.ExpandWidth(false),GUILayout.Height(25)))
                {
                    ToggleSelectedObjects();
                }
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();
        */

        EditorGUILayout.Space();
        
        Repaint();
    }
    #endregion

    #region Custom Methods
    void GetSelection()
    {
        currentSelectionCount = 0;
        currentSelectionCount = Selection.gameObjects.Length;
    }

    void DisableSelectedObstacles()
    {
        // check for selection 
        if(currentSelectionCount == 0)
        {
            if(currentSelectionCount == 0)
            {
                EditorUtility.DisplayDialog("Disable Ostacle", "Atleat one object must be selected", "Ok");
                return;
            }
            
        }
        // disable Obstacles
        GameObject[] selectedObjects = Selection.gameObjects;
        for(int i = 0; i < selectedObjects.Length; i++)
        {
            DestroyImmediate(selectedObjects[i]);
        } 
    }
    #endregion
}
