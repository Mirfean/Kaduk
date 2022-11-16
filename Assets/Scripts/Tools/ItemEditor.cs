using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemEditor : EditorWindow
{
    ItemData _itemData;

    bool[,] _fieldsArray = new bool[0, 0];
    int _width = 1;
    int _height = 1;
    string _description;
    Sprite _icon;

    

    [MenuItem("Window/ItemEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ItemEditor));
    }

    void OnGUI()
    {
        
        EditorGUI.BeginChangeCheck();
        _itemData = (ItemData)EditorGUILayout.ObjectField(_itemData, typeof(ItemData), true);
        if (EditorGUI.EndChangeCheck())
        {
            ApplyNewItem();
        }

        GUILayout.Label("Item", EditorStyles.boldLabel);
        _description = EditorGUILayout.TextField("Description", _description);
        _icon = (Sprite)EditorGUILayout.ObjectField(_icon, typeof(Sprite), false);
        
        GUILayout.Label("Array width/height", EditorStyles.boldLabel);
        _width = EditorGUILayout.IntField("Width", _width);
        _height = EditorGUILayout.IntField("Height", _height);
        EditorGUI.BeginChangeCheck();
        if (_width != _fieldsArray.GetLength(0) || _height != _fieldsArray.GetLength(1))
        {
            _fieldsArray = new bool[_width, _height];
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
        if (_itemData != null)
        {
            _itemData.Description = _description;
            _itemData.ItemIcon = _icon;
            _itemData.Width = _width;
            _itemData.Height = _height;
            _itemData.Fill = new bool[_width, _height];

            System.Array.Copy(_fieldsArray, _itemData.Fill, _fieldsArray.Length);
            //itemData.fill = fieldsArray;
            
            _itemData.OnBeforeSerialize();
            
            //PrefabUtility.RecordPrefabInstancePropertyModifications(itemData);
            EditorUtility.SetDirty(_itemData);
            /*AssetDatabase.SaveAssetIfDirty(itemData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();*/
            this.SaveChanges();
            
            
        }
    }

    private void ApplyNewItem()
    {
        if (_itemData != null)
        {
            _description = _itemData.Description;
            _width = _itemData.Width;
            _height = _itemData.Height;
            _icon = _itemData.ItemIcon;
            if(_itemData.Fill != null)
            {
                Debug.Log("fill is not empty? " + _itemData.Fill.Length);
                _fieldsArray = _itemData.Fill;
            }
            else
            {
                Debug.Log("fill is empty " + _itemData.Fill.Length);
                _itemData.OnAfterDeserialize();
                _fieldsArray = _itemData.Fill;
            }

            foreach(bool x in _itemData.Fill)
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
        for (int j = 0; j < _height; j++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < _width; i++)
            {
                _fieldsArray[i, j] = EditorGUILayout.Toggle(_fieldsArray[i, j]);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}
