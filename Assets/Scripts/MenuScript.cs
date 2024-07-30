using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MenuScript 
{
    [MenuItem("Tools/Toggle Obstacles")]
    public static void ToggleObstacles()
    {
        //Open Popup for editor
        EditorScript.LaunchEditor();
    }
}
