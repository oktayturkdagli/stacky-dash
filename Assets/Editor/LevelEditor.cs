#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

public class ObjectCreator : EditorWindow
{
    private int count = 1;
    private Vector3 startPosition = Vector3.zero, incrementVector = Vector3.one;
    private Vector3 defaultStartPosition = Vector3.zero;
    private Vector3 defaultIncrementVector = Vector3.one;
    private Object baseParent, baseObject, baseWayParent, baseWayObject;
    private GameObject parent, wayParent;
    private bool isActiveAxisX = true, isActiveAxisY = false, isActiveAxisZ = false;
    private bool isInvertX = false, isInvertY = false, isInvertZ = false;


    [MenuItem("Window/Level Editor")]
    public static void Init()
    {
        EditorWindow editorWindow = GetWindow(typeof(ObjectCreator), false, "Level Editor", true);
        editorWindow.minSize = new Vector2(350, 500);
        editorWindow.maxSize = new Vector2(350, 500);
    }

    void OnGUI()
    {
        SetParent();
        SetWayParent();
        DrawMyEditor();
        HandleButtons();
    }

    void DrawMyEditor()
    {
        EditorGUILayout.Space(); GUILayout.Label("Create Object", EditorStyles.boldLabel); EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Object", EditorStyles.boldLabel, GUILayout.Width(120));
        baseObject = EditorGUILayout.ObjectField(baseObject, typeof(GameObject), true); GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Parent", EditorStyles.boldLabel, GUILayout.Width(120));
        baseParent = EditorGUILayout.ObjectField(baseParent, typeof(GameObject), true); GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Way Object", EditorStyles.boldLabel, GUILayout.Width(120));
        baseWayObject = EditorGUILayout.ObjectField(baseWayObject, typeof(GameObject), true); GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Way Parent", EditorStyles.boldLabel, GUILayout.Width(120));
        baseWayParent = EditorGUILayout.ObjectField(baseWayParent, typeof(GameObject), true); GUILayout.FlexibleSpace();
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
        bool buttonRemoveLastest = GUILayout.Button(" Remove Lastest ");
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
            RemoveWay();
        }
        
        if (buttonRemoveLastest)
        {
            RemoveLastest();
            RemoveWayLastest();
        }
        
        if (buttonProduce)
        {
            for (int i = 0; i < count; i++)
            {
                CreateObject();
                CreateWayObject();
            }
        }

        if (buttonUp)
        {
            ResetInvertsAndActives();
            for (int i = 0; i < count; i++)
            {
                isActiveAxisZ = true;
                CreateObject();
                CreateWayObject();
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
                CreateWayObject();
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
                CreateWayObject();
            }
        }
        else if (buttonRight)
        {
            ResetInvertsAndActives();
            for (int i = 0; i < count; i++)
            {
                isActiveAxisX = true;
                CreateObject();
                CreateWayObject();
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
        int childCount = parent.transform.childCount;
        if (childCount < 1)
            return;
        
        DestroyImmediate(parent.transform.GetChild(parent.transform.childCount - 1).gameObject);
        
        //Decrease Position
        if (parent.transform.childCount > 0)
            startPosition = parent.transform.GetChild(parent.transform.childCount - 1).localPosition;
        else
            startPosition = defaultStartPosition;
    }
    
    void RemoveWay()
    {
        int childCount = wayParent.transform.childCount;
        if (childCount < 1)
            return;
        
        DestroyImmediate(wayParent.transform.GetChild(wayParent.transform.childCount - 1).gameObject);
    }
    
    void RemoveLastest()
    {
        int childCount = parent.transform.childCount;
        if (count > childCount)
            count = childCount;
        
        for (int i = 0; i < count; i++)
        {
            Transform lastChild = parent.transform.GetChild(parent.transform.childCount - 1);
            DestroyImmediate(lastChild.gameObject);
        }
        
        //Decrease Position
        if (parent.transform.childCount > 0)
            startPosition = parent.transform.GetChild(parent.transform.childCount - 1).localPosition;
        else
            startPosition = defaultStartPosition;
    }
    
    void RemoveWayLastest()
    {
        int childCount = wayParent.transform.childCount;
        if (count > childCount)
            count = childCount;
        
        for (int i = 0; i < count; i++)
        {
            DestroyImmediate(wayParent.transform.GetChild(wayParent.transform.childCount - 1).gameObject);
        }
    }
    
    void CreateObject()
    {
        
        GameObject myGameObject = baseObject as GameObject; // did add from Editor?
        myGameObject = PrefabUtility.InstantiatePrefab(myGameObject) as GameObject;
        
        if (!myGameObject)
            myGameObject = PrefabUtility.InstantiatePrefab(Selection.gameObjects[0]) as GameObject; // is Prefab?

        if (!myGameObject)
            myGameObject = Instantiate(Selection.gameObjects[0]); // is Gameobject from scene?

        if (!myGameObject)
            return;
        
        IncreasePosition();
        myGameObject.transform.SetParent(parent.transform);
        myGameObject.transform.localPosition = startPosition;
    }
    
    void CreateWayObject()
    {
        GameObject myGameObject = baseWayObject as GameObject; // did add from Editor?
        if (!myGameObject)
            return;
        myGameObject = PrefabUtility.InstantiatePrefab(myGameObject) as GameObject;
        
        myGameObject.transform.SetParent(wayParent.transform);
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
        if (parent)
            return;

        GameObject myParentGameObject = baseParent as GameObject; // did add from Editor?

        if (!myParentGameObject)
            myParentGameObject = new GameObject("Stacks Parent");

        parent = myParentGameObject;
        baseParent = myParentGameObject;
    }
    
    void SetWayParent()
    {
        if (wayParent)
            return;

        GameObject myParentGameObject = baseWayParent as GameObject; // did add from Editor?

        if (!myParentGameObject)
            myParentGameObject = new GameObject("Ways Parent");

        wayParent = myParentGameObject;
        baseWayParent = myParentGameObject;
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