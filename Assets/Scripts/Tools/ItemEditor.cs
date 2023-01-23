using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Assets.Scripts.Enums;
using System;

public class ItemEditor : EditorWindow
{
    ItemData _itemData;
    SerializedProperty _SPitemData;

    string _id;
    string _name;
    string _description;

    int _width = 1;
    int _height = 1;
    bool[,] _fieldsArray = new bool[0, 0];

    ItemType _itemType;
    Sprite _icon;

    int _combinableSize;
    public string[] _secondItem = new string[0];
    public string[] _result = new string [0];

    ScriptableObject scriptableObj;
    SerializedObject serialObj;
    SerializedProperty secondItemProperty;
    SerializedProperty resultProperty;

    private void OnEnable()
    {
        
    }

    [MenuItem("Window/ItemEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ItemEditor));
    }

    void OnGUI()
    {
        GUIStyle Description = new GUIStyle()
        {
            fixedHeight = 200,
            richText = true,
        };

        EditorGUI.BeginChangeCheck();
        _itemData = (ItemData)EditorGUILayout.ObjectField(_itemData, typeof(ItemData), true);
        if (EditorGUI.EndChangeCheck())
        {
            ApplyNewItem();
        }

        GUILayout.Label("Item", EditorStyles.boldLabel);
        _id = EditorGUILayout.TextField("ID", _id);
        _name = EditorGUILayout.TextField("Name", _name);
        _description = EditorGUILayout.TextField("Description", _description);
        _icon = (Sprite)EditorGUILayout.ObjectField(_icon, typeof(Sprite), false);
        _itemType = (ItemType)EditorGUILayout.EnumFlagsField(_itemType);

        GUILayout.Label("Combinables", EditorStyles.miniBoldLabel);
        if (secondItemProperty != null && resultProperty != null)
        {
            EditorGUILayout.PropertyField(secondItemProperty, true);
            EditorGUILayout.PropertyField(resultProperty, true);
            serialObj.ApplyModifiedProperties();
            Debug.Log("Second " + _secondItem.Length);
            Debug.Log("Result " + _result.Length);
        }
        
        GUILayout.Label("Array width/height", EditorStyles.miniBoldLabel);
        _width = EditorGUILayout.IntField("Width", _width);
        _height = EditorGUILayout.IntField("Height", _height);
        
        EditorGUI.BeginChangeCheck();
        
        if (_width != _fieldsArray.GetLength(0) || _height != _fieldsArray.GetLength(1))
        {
            _fieldsArray = new bool[_width, _height];
        }
        if (EditorGUI.EndChangeCheck())
        {
            UpdateEditorItem();
        }
        ChangeArrayWidthAndHeight();
        if (GUILayout.Button("Apply"))
        {
            SaveItemChanges();
        }
    }

    void ApplyNewItem()
    {
        if (_itemData != null)
        {
            

            _id = _itemData.id;
            _name = _itemData.ItemName;
            _description = _itemData.Description;
            _width = _itemData.Width;
            _height = _itemData.Height;
            _icon = _itemData.ItemIcon;

            if (_itemData.Fill != null) _fieldsArray = _itemData.Fill;
            else
            {
                Debug.Log("fill is empty " + _itemData.Fill.Length);
                _itemData.OnAfterDeserialize();
                _fieldsArray = _itemData.Fill;
            }

            scriptableObj = this;
            serialObj = new SerializedObject(scriptableObj);
            secondItemProperty = serialObj.FindProperty("_secondItem");
            resultProperty = serialObj.FindProperty("_result");

            secondItemProperty.ClearArray();
            resultProperty.ClearArray();

            _secondItem = _itemData.SecondItem;
            _result = _itemData.Result;

            for (int i = 0; i < _secondItem.Length; i++)
            {

            }            

        }
    }

    private void SaveItemChanges()
    {
        if (_itemData != null)
        {
            _itemData.id = _id;
            _itemData.ItemName = _name;
            _itemData.Description = _description;
            _itemData.ItemIcon = _icon;


            _itemData.Width = _width;
            _itemData.Height = _height;
            _itemData.Fill = new bool[_width, _height];

            _itemData.SecondItem = new string[_secondItem.Length];
            _itemData.Result = new string [_result.Length];

            Debug.Log("Second Item " + _secondItem.Length + " " + _secondItem[0]);
            Debug.Log("Result Item " + _result.Length + " " + _result[0]);

            System.Array.Copy(_fieldsArray, _itemData.Fill, _fieldsArray.Length);
            System.Array.Copy(_secondItem, _itemData.SecondItem, _secondItem.Length);
            System.Array.Copy(_result, _itemData.Result, _result.Length);
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

    private void UpdateEditorItem()
    {
        if (_itemData != null)
        {
            _name = _itemData.ItemName;
            _description = _itemData.Description;
            _width = _itemData.Width;
            _height = _itemData.Height;
            _icon = _itemData.ItemIcon;

            if(_itemData.Fill != null) _fieldsArray = _itemData.Fill;
            else
            {
                Debug.Log("fill is empty " + _itemData.Fill.Length);
                _itemData.OnAfterDeserialize();
                _fieldsArray = _itemData.Fill;
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

    void SetArrays()
    {
        

    }

/*    void ChangeCombinableArrays()
    {
        int i = 0;
        foreach (KeyValuePair<string, string> comb in _itemData.Combinables)
        {
            _secondItem[i] = comb.Key;
            _result[i] = comb.Value;
            i++;
        }
    }*/

}
