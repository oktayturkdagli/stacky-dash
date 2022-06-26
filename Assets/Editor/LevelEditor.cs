#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

public class LevelEditor : EditorWindow
{
    private int count = 1;
    private Vector3 startPosition = Vector3.zero, incrementVector = Vector3.one;
    private Vector3 defaultStartPosition = Vector3.zero, defaultIncrementVector = Vector3.one;
    private Object baseStackParent, baseStack, baseRoadParent, baseRoad;
    private GameObject stackParent, roadParent;
    private bool isActiveAxisX = true, isActiveAxisY = false, isActiveAxisZ = false;
    private bool isInvertX = false, isInvertY = false, isInvertZ = false;


    [MenuItem("Window/Level Editor")]
    public static void Init()
    {
        EditorWindow editorWindow = GetWindow(typeof(LevelEditor), false, "Level Editor", true);
        editorWindow.minSize = new Vector2(350, 500);
        editorWindow.maxSize = new Vector2(350, 500);
    }

    void OnGUI()
    {
        SetParent();
        SetRoadParent();
        DrawMyEditor();
        HandleButtons();
    }

    void DrawMyEditor()
    {
        EditorGUILayout.Space(); GUILayout.Label("Create Object", EditorStyles.boldLabel); EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Object", EditorStyles.boldLabel, GUILayout.Width(120));
        baseStack = EditorGUILayout.ObjectField(baseStack, typeof(GameObject), true); GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Parent", EditorStyles.boldLabel, GUILayout.Width(120));
        baseStackParent = EditorGUILayout.ObjectField(baseStackParent, typeof(GameObject), true); GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Road Object", EditorStyles.boldLabel, GUILayout.Width(120));
        baseRoad = EditorGUILayout.ObjectField(baseRoad, typeof(GameObject), true); GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Road Parent", EditorStyles.boldLabel, GUILayout.Width(120));
        baseRoadParent = EditorGUILayout.ObjectField(baseRoadParent, typeof(GameObject), true); GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Start Position", EditorStyles.boldLabel, GUILayout.Width(120));
        startPosition = EditorGUILayout.Vector3Field("", startPosition); GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Increment Vector", EditorStyles.boldLabel, GUILayout.Width(120));
        incrementVector = EditorGUILayout.Vector3Field("", incrementVector);GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal(GUILayout.Width(330));
        GUILayout.Label("Invert", EditorStyles.boldLabel, GUILayout.Width(120));
        GUILayout.Label("X ", EditorStyles.boldLabel);
        isInvertX = EditorGUILayout.Toggle(isInvertX, GUILayout.Width(20));
        GUILayout.FlexibleSpace();
        GUILayout.Label("Y ", EditorStyles.boldLabel);
        isInvertY = EditorGUILayout.Toggle(isInvertY, GUILayout.Width(20));
        GUILayout.FlexibleSpace();
        GUILayout.Label("Z ", EditorStyles.boldLabel);
        isInvertZ = EditorGUILayout.Toggle(isInvertZ, GUILayout.Width(20));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal(GUILayout.Width(330));
        GUILayout.Label("Active Axes", EditorStyles.boldLabel, GUILayout.Width(120));
        GUILayout.Label("X ", EditorStyles.boldLabel);
        isActiveAxisX = EditorGUILayout.Toggle(isActiveAxisX, GUILayout.Width(20));
        GUILayout.FlexibleSpace();
        GUILayout.Label("Y ", EditorStyles.boldLabel);
        isActiveAxisY = EditorGUILayout.Toggle(isActiveAxisY, GUILayout.Width(20));
        GUILayout.FlexibleSpace();
        GUILayout.Label("Z ", EditorStyles.boldLabel);
        isActiveAxisZ = EditorGUILayout.Toggle(isActiveAxisZ, GUILayout.Width(20));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Count", EditorStyles.boldLabel, GUILayout.Width(120));
        count = EditorGUILayout.IntField(count, GUILayout.Width(205)); GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    void HandleButtons()
    {
        //Build Button
        EditorGUILayout.Space(); EditorGUILayout.Space();
        GUILayout.BeginHorizontal(GUILayout.Width(330)); GUILayout.FlexibleSpace();
        bool buttonReset = GUILayout.Button(" Reset ");
        bool buttonRemove = GUILayout.Button(" Remove ");
        bool buttonProduce = GUILayout.Button(" Produce ");
        GUILayout.EndHorizontal();
        
        EditorGUILayout.Space(50);
        GUILayout.BeginHorizontal(GUILayout.Width(330)); 
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(); 
        GUILayout.BeginHorizontal();  GUILayout.FlexibleSpace(); 
        GUILayout.Label("Use Arrows", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();  GUILayout.FlexibleSpace(); 
        bool buttonUp = GUILayout.Button(" ^ ", GUILayout.Width(30)); 
        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        bool buttonLeft = GUILayout.Button(" < ", GUILayout.Width(30));
        EditorGUILayout.Space(20);
        bool buttonRight = GUILayout.Button(" > ", GUILayout.Width(30));
        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();  GUILayout.FlexibleSpace(); 
        bool buttonDown = GUILayout.Button(" v ", GUILayout.Width(30));
        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.EndVertical(); 
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        if (buttonReset)
        {
            Reset();
        }
        
        if (buttonRemove)
        {
            Remove();
            RemoveRoad();
        }

        if (buttonProduce)
        {
            for (int i = 0; i < count; i++)
            {
                CreateObject();
                CreateRoadObject();
            }
        }

        if (buttonUp)
        {
            ResetInvertsAndActives();
            for (int i = 0; i < count; i++)
            {
                isActiveAxisZ = true;
                CreateObject();
                CreateRoadObject();
            }
        }
        else if (buttonDown)
        {
            ResetInvertsAndActives();
            for (int i = 0; i < count; i++)
            {
                isActiveAxisZ = true;
                isInvertZ = true;
                CreateObject();
                CreateRoadObject();
            }
        }
        else if (buttonLeft)
        {
            ResetInvertsAndActives();
            for (int i = 0; i < count; i++)
            {
                isActiveAxisX = true;
                isInvertX = true;
                CreateObject();
                CreateRoadObject();
            }
        }
        else if (buttonRight)
        {
            ResetInvertsAndActives();
            for (int i = 0; i < count; i++)
            {
                isActiveAxisX = true;
                CreateObject();
                CreateRoadObject();
            }
        }
    }
    
    void Reset()
    {
        startPosition = defaultStartPosition;
        incrementVector = defaultIncrementVector;
        isActiveAxisX = true;
        isActiveAxisY = false;
        isActiveAxisZ = false;
        isInvertX = false;
        isInvertY = false;
        isInvertZ = false;
    }

    void Remove()
    {
        int childCount = stackParent.transform.childCount;
        if (childCount < 1)
            return;
        
        DestroyImmediate(stackParent.transform.GetChild(stackParent.transform.childCount - 1).gameObject);
        
        //Decrease Position
        if (stackParent.transform.childCount > 0)
            startPosition = stackParent.transform.GetChild(stackParent.transform.childCount - 1).localPosition;
        else
            startPosition = defaultStartPosition;
    }
    
    void RemoveRoad()
    {
        int childCount = roadParent.transform.childCount;
        if (childCount < 1)
            return;
        
        DestroyImmediate(roadParent.transform.GetChild(roadParent.transform.childCount - 1).gameObject);
    }

    void CreateObject()
    {
        
        GameObject myGameObject = baseStack as GameObject; // did add from Editor?
        myGameObject = PrefabUtility.InstantiatePrefab(myGameObject) as GameObject;
        
        if (!myGameObject)
            myGameObject = PrefabUtility.InstantiatePrefab(Selection.gameObjects[0]) as GameObject; // is Prefab?

        if (!myGameObject)
            myGameObject = Instantiate(Selection.gameObjects[0]); // is Gameobject from scene?

        if (!myGameObject)
            return;
        
        IncreasePosition();
        myGameObject.transform.SetParent(stackParent.transform);
        myGameObject.transform.localPosition = startPosition;
    }
    
    void CreateRoadObject()
    {
        GameObject myGameObject = baseRoad as GameObject; // did add from Editor?
        if (!myGameObject)
            return;
        myGameObject = PrefabUtility.InstantiatePrefab(myGameObject) as GameObject;
        
        myGameObject.transform.SetParent(roadParent.transform);
        myGameObject.transform.localPosition = startPosition;
    }
    
    void IncreasePosition()
    {
        Vector3 tempIncrementVector = incrementVector;
        if (!isActiveAxisX)
            tempIncrementVector.x = 0;
        if (!isActiveAxisY)
            tempIncrementVector.y = 0;
        if (!isActiveAxisZ)
            tempIncrementVector.z = 0;
        
        if (isInvertX)
            tempIncrementVector.x *= -1;
        if (isInvertY)
            tempIncrementVector.y *= -1;
        if (isInvertZ)
            tempIncrementVector.z *= -1;
        
        startPosition += tempIncrementVector;
    }

    void SetParent()
    {
        if (stackParent)
            return;
        
        GameObject myParentGameObject = GameObject.Find("Stacks Parent");

        if (!myParentGameObject)
            myParentGameObject = baseStackParent as GameObject; // did add from Editor?

        if (!myParentGameObject)
            myParentGameObject = new GameObject("Stacks Parent");

        stackParent = myParentGameObject;
        baseStackParent = myParentGameObject;
        
        //Set Position
        if (stackParent.transform.childCount > 0)
            startPosition = stackParent.transform.GetChild(stackParent.transform.childCount - 1).localPosition;
        else
            startPosition = defaultStartPosition;
    }
    
    void SetRoadParent()
    {
        if (roadParent)
            return;
        
        GameObject myParentGameObject = GameObject.Find("Roads Parent");
        
        if (!myParentGameObject)
            myParentGameObject = baseRoadParent as GameObject; // did add from Editor?


        if (!myParentGameObject)
            myParentGameObject = new GameObject("Roads Parent");

        roadParent = myParentGameObject;
        baseRoadParent = myParentGameObject;
    }
    
    void ResetInvertsAndActives()
    {
        isInvertZ = false;
        isInvertY = false;
        isInvertX = false;
        isActiveAxisX = false;
        isActiveAxisY = false;
        isActiveAxisZ = false;
    }
    
    
}

#endif