//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//public class Class_tool : EditorWindow
//{
//    public int width = 8;
//    public float offset = 8.0f;
//    public Vector2 gridSize;
//    public string prefabsPath = "Assets/Prefabs";
//    private List<string> instances;

//    [MenuItem("Alex/Spawner")]
//    static void Init()
//    {
//        Class_tool window = (Class_tool)EditorWindow.GetWindow(typeof(Class_tool));
//        window.Show();
//    }

//    void OnGUI()
//    {
//        //Canvas drawing elements
//        prefabsPath = EditorGUILayout.TextField("Prefabs path", prefabsPath);
//        //Space between object 
//        width = EditorGUILayout.IntField("Grid width", width);
//        offset = EditorGUILayout.FloatField("Offset", offset);
//        gridSize = EditorGUILayout.Vector2Field("Grid Size", gridSize);



//        if (GUILayout.Button("Build Tile sets"))
//        {
//            instances = new List<string>();

//            RetrievePrefabs(prefabsPath);
//            SpawnPrefabs();

//        }

//    }

//    void RetrievePrefabs(string path)
//    {
//        string[] prefabs = AssetDatabase.FindAssets("", new string[] { prefabsPath });

//        foreach (string assetguid in prefabs)
//        {
//            string assetpath = AssetDatabase.GUIDToAssetPath(assetguid);

//            if (AssetDatabase.IsValidFolder(assetpath))
//            {
//                RetrievePrefabs(assetpath);
//                continue;
//            }

//            if (!instances.Contains(assetpath))
//                instances.Add(assetpath);
//        }

//    }
//    void SpawnPrefabs()
//    {
//        int x_gridpos = 0, y_gridpos = 0;
//        foreach (string path in instances)
//        {
//            float pos_x = x_gridpos * (gridSize.x + offset);
//            float pos_y = y_gridpos * (gridSize.y + offset);

//            x_gridpos = (x_gridpos + 1) % width;
//            y_gridpos += x_gridpos % width == 0 ? 1 : 0;

//            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
//            //  GameObject prefab = PrefabUtility.InstantiatePrefab(obj) as GameObject;
//            //  prefab.transform.position = new Vector3(pos_x, 0, pos_y);
//            Debug.Log(instances.Count);
//        }
//    }
//}
