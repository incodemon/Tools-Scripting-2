//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.IO;
//using UnityEditor.Callbacks;

//public class AssetHandler
//{
//    [OnOpenAsset()]
//    public static bool OpenEditor(int instanceID, int line)
//    {
//        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
//        if(obj != null)
//        {
//            SpriteEditorWindow.Open(obj);
//            return true;
//        }
//        return false;
//    }
//}

//[CustomEditor(typeof(GameObject))]
//public class SpriteCustomEditorWindow : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        if (GUILayout.Button("Open Editor"))
//        {

//            SpriteEditorWindow.Open((GameObject)target);
//        }
//    }
//}
