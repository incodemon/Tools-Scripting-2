using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MVDTools : EditorWindow
{
    public struct PlacementSettings
    {
        public bool active;

        // Filter by tag
        // Filter by layer
        // Add some additional transform information
        // Physics
        // Attach to parent
    }

    // Editor version
    public static string version = "0.1a";

    // Display textures
    public static Texture2D texture_logo;

    // Predefined sizes
    public Vector2 tx_logo_size = new Vector2(200, 75);

    private int width = 8;
    private float offset = 8f;
    private Vector2 gridSize;

    private string prefabsPath = "Assets/Prefabs";
    private List<string> instances;

    static private MVDToolsBrowser prefabBrowser;
    static private bool[] foldout = new bool[] { false, false, true };
    static private string[] searchPath = new string[] { "Assets" };
    static private PlacementSettings placementSettings;

    static Vector3 tmp_position = new Vector3();
    static Vector3 tmp_rotation = new Vector3();
    static Vector3 tmp_scale = new Vector3();

    static private GameObject dummyObject;

    // Init function, we create the window and initialize settings
    [MenuItem("Master/MVD Tools Panel")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MVDTools window = (MVDTools)EditorWindow.GetWindow(typeof(MVDTools));
        window.Show();

        UpdateResources();
    }

    /* Editor UI METHODS */

    // Reloading resources method
    [UnityEditor.Callbacks.DidReloadScripts]
    static void UpdateResources()
    {
        prefabBrowser = new MVDToolsBrowser();
        prefabBrowser.eventFilter.AddListener(GenerateSearch);
        prefabBrowser.eventRefresh.AddListener(GenerateSearch);
        GenerateSearch();

        texture_logo = Resources.Load("logo_lasalle") as Texture2D;
    }

    // Method used to draw anything on our window screen.
    void OnGUI()
    {
        // OnGUI displays UI elements on editor window refresh (Editor window refresh when needed)

        DisplayStyles();
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUI.DrawTexture(new Rect(Screen.width * .5f - tx_logo_size.x * .5f, 15, tx_logo_size.x, tx_logo_size.y), texture_logo, ScaleMode.StretchToFill, true);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(tx_logo_size.y + 10f);
            DisplaySeparator(50);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Tools Version: " + version, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

        DisplayPanel("Utilities", () => DisplayUtilities(), ref foldout[0]);
        DisplayPanel("Prefab Loader", () => DisplayPrefabLoader(), ref foldout[1]);
        DisplayPanel("Prefab Spawner", () => DisplayPrefabSpawner(), ref foldout[2]);
    }

    private void DisplayPrefabLoader()
    {
        // Canvas drawing elements
        prefabsPath = EditorGUILayout.TextField("Prefabs path", prefabsPath);
        width = EditorGUILayout.IntField("Grid width", width);
        offset = EditorGUILayout.FloatField("Offset", offset);
        gridSize = EditorGUILayout.Vector2Field("Grid Size", gridSize);

        GUIStyle editorStyle = new GUIStyle(GUI.skin.button);
        Color myStyleColor = Color.blue;
        editorStyle.fontStyle = FontStyle.Bold;
        editorStyle.normal.textColor = myStyleColor;

        if (GUILayout.Button("Build Tilesets", editorStyle))
        {
            instances = new List<string>();

            RetrievePrefabs(prefabsPath);
            SpawnPrefabs();
        }
    }

    // Method used to display the utility tools.
    private void DisplayUtilities()
    {
        // TO-DO
        // Implement the UI for the necessary utilities required for the final deliver.
    }

    // Method used to display the prefab placement tools.
    private void DisplayPrefabSpawner()
    {
        /* DO YOUR CODING HERE FOR UI */

        //Display a dropdown list with all layers, and get the selected one.

        // AssetObject to attach to parent

        // Build the necessary UI to display the transform settings
        EditorGUILayout.Vector3Field("Position", tmp_position);
        EditorGUILayout.Vector3Field("Rotation", tmp_rotation);
        EditorGUILayout.Vector3Field("Scale", tmp_scale);
        prefabBrowser.Display();

        // TO-DO
        // Add UI to modify transform settings
        // Add UI to modify tag and layer settings

        if (GUILayout.Button("Place"))
        {
            placementSettings.active = !placementSettings.active;
            UpdatePlacementTool();
        }
    }

    static void DisplayStyles()
    {
        prefabBrowser.LoadStyles();
    }

    // Display a separator line as needed.
    static void DisplaySeparator(int width)
    {
        string line = string.Empty;
        for (int i = 0; i < width; i++) line += "_";

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField(line);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    // Display a foldout inside a vertical box.
    static void DisplayPanel(string title, System.Action toexecute, ref bool foldout)
    {
        //GUI.color = color;
        EditorGUILayout.BeginVertical("Box");
        {
            GUI.color = Color.white;
            GUILayout.Space(3);
            GUILayout.BeginHorizontal();
            foldout = EditorGUILayout.Foldout(foldout, title, foldout);
            GUILayout.EndHorizontal();
            GUILayout.Space(3);

            if (foldout)
            {
                // Method to be called.
                toexecute();
            }
        }
        GUILayout.EndVertical();
    }

    // Functions to execute functionality

    private static void GenerateSearch(string filterName = "")
    {
        List<AssetInfo> assetList = new List<AssetInfo>();

        // Find assets at the given path
        string[] guids = AssetDatabase.FindAssets("t:prefab", searchPath);

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.Object));
            Texture2D preview = AssetPreview.GetAssetPreview(asset) ?? AssetPreview.GetMiniThumbnail(asset);

            if (filterName != string.Empty && !asset.name.Contains(filterName))
                continue;

            AssetInfo assetInfo = new AssetInfo(guid, assetPath);
            assetInfo.preview = preview;
            assetInfo.Name = asset.name;
            assetList.Add(assetInfo);
        }

        prefabBrowser.FillBrowser(assetList.ToArray());
    }

    //private void CreatePlacementTool()
    //{
    //    Selection.activeObject = null;
    //    GameObject obj = prefabBrowser.SelectedPrefab; // This method gives me the selected prefab from the window.

    //    dummyObject = PrefabUtility.InstantiatePrefab(obj) as GameObject;
    //    //dummyObject.hideFlags = HideFlags.HideInHierarchy;
    //    dummyObject.AddComponent<ClickSpawn>();
    //    ClickSpawn clickSpawn = dummyObject.GetComponent<ClickSpawn>();
    //    clickSpawn.prefab = obj;

    //    // TO-DO
    //    // Add parameters to configure the clickspawn component
    //    // Additional transform settings 

    //    //clickSpawn.layerIndex = 2;
    //    dummyObject.name = "MVDT_DummyObject";

    //    MVDUtils.RecursiveSetLayer(dummyObject.transform, 2);
    //    MVDUtils.ChangeAllMaterials(dummyObject, Resources.Load("mtl_debug_placement") as Material);
    //}

    private void UpdatePlacementTool()
    {
        Object.DestroyImmediate(dummyObject);
        if (placementSettings.active)
        {
            //CreatePlacementTool();
        }
    }

    // Get all the prefabs from the folder
    void RetrievePrefabs(string path)
    {
        string[] prefabs = AssetDatabase.FindAssets("", new string[] { prefabsPath });

        foreach (string assetguid in prefabs)
        {
            string assetpath = AssetDatabase.GUIDToAssetPath(assetguid);

            Debug.Log(assetpath);

            if (AssetDatabase.IsValidFolder(assetpath))
            {
                //RetrievePrefabs(assetpath);
                continue;
            }

            if (!instances.Contains(assetpath))
                instances.Add(assetpath);
        }
    }

    // Spawn the prefabs on the scene
    void SpawnPrefabs()
    {
        int x_gridpos = 0, y_gridpos = 0;

        foreach (string path in instances)
        {
            // Get the grid position
            float pos_x = x_gridpos * (gridSize.x + offset);
            float pos_y = y_gridpos * (gridSize.y + offset);

            // Update the grid position
            x_gridpos = (x_gridpos + 1) % width;
            y_gridpos += x_gridpos % width == 0 ? 1 : 0;

            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            GameObject prefab = PrefabUtility.InstantiatePrefab(obj) as GameObject;
            prefab.transform.position = new Vector3(pos_x, 0, pos_y);
        }
    }
}