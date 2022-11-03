using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemEditor : EditorWindow
{
    ItemData itemData;

    bool[,] fieldsArray = new bool[0, 0];
    int width = 1;
    int height = 1;
    string description;
    Sprite icon;

    

    [MenuItem("Window/ItemEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ItemEditor));
    }

    void OnGUI()
    {
        
        EditorGUI.BeginChangeCheck();
        itemData = (ItemData)EditorGUILayout.ObjectField(itemData, typeof(ItemData), true);
        if (EditorGUI.EndChangeCheck())
        {
            ApplyNewItem();
        }

        GUILayout.Label("Item", EditorStyles.boldLabel);
        description = EditorGUILayout.TextField("Description", description);
        icon = (Sprite)EditorGUILayout.ObjectField(icon, typeof(Sprite), false);
        
        GUILayout.Label("Array width/height", EditorStyles.boldLabel);
        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);
        EditorGUI.BeginChangeCheck();
        if (width != fieldsArray.GetLength(0) || height != fieldsArray.GetLength(1))
        {
            fieldsArray = new bool[width, height];
        }
        if (EditorGUI.EndChangeCheck())
        {
            ApplyNewItem();
        }
        ChangeArrayWidthAndHeight();
        if (GUILayout.Button("Apply"))
        {
            SaveItemChanges();

        }
    }

    private void SaveItemChanges()
    {
        if (itemData != null)
        {
            itemData.description = description;
            itemData.itemIcon = icon;
            itemData.width = width;
            itemData.height = height;
            itemData.fill = new bool[width, height];

            System.Array.Copy(fieldsArray, itemData.fill, fieldsArray.Length);
            //itemData.fill = fieldsArray;
            
            itemData.OnBeforeSerialize();
            
            //PrefabUtility.RecordPrefabInstancePropertyModifications(itemData);
            EditorUtility.SetDirty(itemData);
            /*AssetDatabase.SaveAssetIfDirty(itemData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();*/
            this.SaveChanges();
            
            
        }
    }

    private void ApplyNewItem()
    {
        if (itemData != null)
        {
            description = itemData.description;
            width = itemData.width;
            height = itemData.height;
            icon = itemData.itemIcon;
            if(itemData.fill != null)
            {
                Debug.Log("fill is not empty? " + itemData.fill.Length);
                fieldsArray = itemData.fill;
            }
            else
            {
                Debug.Log("fill is empty " + itemData.fill.Length);
                itemData.OnAfterDeserialize();
                fieldsArray = itemData.fill;
            }

            foreach(bool x in itemData.fill)
            {
                if (x)
                {
                    Debug.Log("FILL asd");
                }
            }
        }
    }

    void ChangeArrayWidthAndHeight()
    {
        for (int j = 0; j < height; j++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < width; i++)
            {
                fieldsArray[i, j] = EditorGUILayout.Toggle(fieldsArray[i, j]);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}
