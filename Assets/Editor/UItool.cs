using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.PackageManager.UI;

public class UItool : EditorWindow
{
    public ObjectList myList= new ObjectList(); //List to contain details of JSON file
    GameObject cube;
    TextAsset jsonFile;
    Vector2 scroll;
    string uiParentName = "God Father";

    [MenuItem("Tools/Bsic UI Setup")] //Tool ddress
    public static void showWindow()
    {
        
        GetWindow(typeof(UItool));
    }

    public string[] Strings = { "Larry", "Curly", "Moe" };
    private void OnGUI()
    {
        //---------------------TITLE-------------------
        GUILayout.Label("CREATE UI TEMPLATE");
        GUILayout.Space(10);
        
        //---------------------TITLE-------------------


        //----------------PROPERTIES-------------------#start
        uiParentName = EditorGUILayout.TextField("UI Parent Name", uiParentName); //Scroll View
        jsonFile = EditorGUILayout.ObjectField("JSON File", jsonFile, typeof(TextAsset), false) as TextAsset; //Json File
        cube = EditorGUILayout.ObjectField("Game Object", cube, typeof(GameObject), false) as GameObject; //Object To Spawn

        scroll =  GUILayout.BeginScrollView(scroll); //#Scroll Start

        //----------------PROPERTIES-------------------#end
        
        ShowJsonArrayGUI(); //Show the JSON Data in an array format in Editor Window.

        GUILayout.EndScrollView(); //#Scroll end

        //----------------BUTTONS-------------------#start

        if (GUILayout.Button("Read Json")) //Reads the JSON File.
        {

            myList = jsonFile != null ? JsonUtility.FromJson<ObjectList>(jsonFile.text) : null;
        }
        

        if (GUILayout.Button("Save Json"))  //Saves the JSON File.
        {
            string json = JsonUtility.ToJson(myList);
            File.WriteAllText(AssetDatabase.GetAssetPath(jsonFile),json);
        }

        if (GUILayout.Button("clear Json")) //Clears the JSON File.
        {
            myList = null;
        }

        if (GUILayout.Button("Generate UI")) //Generates the UI Object.
        {
            GenerateObject();
        }

        if(GUILayout.Button("Destroy OBJ")) //Destroys the selected Object.
        {
            DestroyParent(uiParentName);
        }

        //----------------BUTTONS-------------------#end
    }

  
   void ShowJsonArrayGUI() //Function to shoa JSON data in array format in the inspector.
    {
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("myList");
        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties();
    }

    void GenerateObject() //Function to Generata Object.
    {
        
        if (cube == null) return;
       
        GameObject GodFather = new GameObject();
        GodFather.name = uiParentName;
        ObjectData[] objList = myList.StartUI; 

        for(int i=0;i<objList.Length;i++) // This loop Itertes though all the elements in the ObjList Array.
        {
           
            string objName = objList[i].name;
            Vector3 objPos = new Vector3(objList[i].posx, objList[i].posy, objList[i].posz);
            Vector3 objSize = new Vector3(objList[i].size, objList[i].size, objList[i].size);

            GameObject obj = Instantiate(cube,objPos,  Quaternion.identity,GodFather.transform);
            obj.transform.localScale = objSize; //sets Size of the new object.

            if (objList[i].parent!=null) //Sets the parent
            {
               GameObject parent =  SearchParent(GodFather, objList[i].parent);
                if(parent != null) 
                {
                    if (parent.name != objList[i].parent)
                    {
                        parent = GodFather;
                    }
                    obj.transform.SetParent(parent.transform);
                    
                }

            }

            obj.name = objName; //sets Position of the new object.
            obj.transform.position = objPos; //sets Name of the new object.
            

        }
    }
    GameObject foundparent;
    GameObject SearchParent(GameObject parentObj,string parentName)  //Function to Search the object to set as parent.
    {
        
        foreach(Transform t in parentObj.transform)
        {
            if(t.name==parentName)
            {
                foundparent= t.gameObject;
            }
            else if(t.childCount>0) //If child exists, calls the function again to search it child onjects.
            {
                SearchParent(t.gameObject, parentName);
            }
        }
        
        return foundparent;
    }

    void DestroyParent(string name) //Function to Destroy the selected object with specific string name.
    {
        GameObject selected = Selection.activeGameObject;
        if (selected != null) 
        {
            if(selected.name==name)
            {
                DestroyImmediate(selected);
            }
        }
    }
}
[System.Serializable]
public class ObjectData
{
    public string name;
    public string parent;
    public int posx;
    public int posy;
    public int posz;
    public int size;
}

[System.Serializable]
public class ObjectList
{
    public ObjectData[] StartUI;
}



