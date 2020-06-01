//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.IO;

//public class ExtendedEditorWindow : EditorWindow
//{

//    protected SerializedObject serializedObject;
//    protected SerializedProperty currentProperty;

//    protected void DrawProperties(GameObject prop, bool drawChildren)
//    {
//        string lastPorpPath = string.Empty;
//        foreach (SerializedProperty p in prop)
//        {
//            if (p.isArray && p.propertyType == SerielizedPropertyType.Generic)
//            {
//                EditorGUILayout.BeginHorizonatl();
//                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
//                EditorGUILayout.EndHorizontal();

//                if (p.isExpanded)
//                {
//                    EditorGUI.indentLevel++;
//                    DrawProperties(p,drawChildren);
//                    EditorGUI.indentLevel--;
//                }
//            }
//            else
//            {
//                if(!string.IsNullOrEmpty(lastPorpPath) && p.propertyPath.Contains(lastPorpPath)) { continue; }
//                lastPorpPath = p.propertyPath;
//                EditorGUILayout.PropertyField(p, drawChildren);
//            }
//        }

//    }



//}
