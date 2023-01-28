using Assets.Scripts.Enums;
using UnityEditor;
using UnityEngine;

public class ItemEditor : EditorWindow
{
    ItemData _itemData;

    string _id;
    string _name;
    string _description;
    bool _isKeyItem;

    int _width = 1;
    int _height = 1;
    bool[,] _fieldsArray = new bool[0, 0];

    ItemType _itemType;
    Sprite _icon;

    //Consumable
    ConsumableEffect _effect;
    int _effectPower;

    //Weapon
    WeaponType _weaponType;
    WeaponData _weaponData;

    //Ammo
    int _stackMax;
    WeaponType _ammoType;


    int _combinableSize;
    public string[] _secondItem = new string[0];
    public string[] _result = new string[0];

    ScriptableObject scriptableObj;
    SerializedObject serialObj;
    SerializedProperty secondItemProperty;
    SerializedProperty resultProperty;

    ItemType _lastItemType;
    bool _hasCombinables;

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
        _isKeyItem = EditorGUILayout.Toggle("Key Item", _isKeyItem);
        _icon = (Sprite)EditorGUILayout.ObjectField(_icon, typeof(Sprite), false);
        _itemType = (ItemType)EditorGUILayout.EnumFlagsField(_itemType);

        GUILayout.Label("Additional Data (Weapon/Consumable/Ammo)", EditorStyles.miniBoldLabel);
        if (_itemType == ItemType.WEAPON)
        {
            _weaponType = (WeaponType)EditorGUILayout.EnumFlagsField(_weaponType);
            _weaponData = (WeaponData)EditorGUILayout.ObjectField(_weaponData, typeof(WeaponData), true);
        }

        if (_itemType == ItemType.CONSUMABLE)
        {
            _effect = (ConsumableEffect)EditorGUILayout.EnumFlagsField(_effect);
            _effectPower = EditorGUILayout.IntField("Effect Power", _effectPower);
        }

        if (_itemType == ItemType.AMMO)
        {
            _ammoType = (WeaponType)EditorGUILayout.EnumFlagsField(_ammoType);
            _stackMax = EditorGUILayout.IntField("Max stacks", _stackMax);
        }

        GUILayout.Label("Combinables", EditorStyles.miniBoldLabel);
        EditorGUI.BeginChangeCheck();
        _hasCombinables = EditorGUILayout.Toggle("Do it has a COMBINE items?", _hasCombinables);
        if (EditorGUI.EndChangeCheck()) SetArrays();
        if (_hasCombinables)
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
            _lastItemType = _itemData.itemType;

            _id = _itemData.id;
            _name = _itemData.ItemName;
            _description = _itemData.Description;
            _width = _itemData.Width;
            _height = _itemData.Height;
            _icon = _itemData.ItemIcon;
            _isKeyItem = _itemData.IsKeyItem;
            _itemType = _itemData.itemType;

            _effect = _itemData.Effect;
            _effectPower = _itemData.EffectPower;

            _weaponType = _itemData.weaponType;
            _weaponData = _itemData.weaponData;

            _stackMax = _itemData.StackMax;
            _ammoType = _itemData.AmmoType;


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

            if (secondItemProperty.arraySize > 0)
            {
                _hasCombinables = true;
                SetArrays();
            }
            else _hasCombinables = false;

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
            _itemData.IsKeyItem = _isKeyItem;
            _itemData.itemType = _itemType;

            //Consumable
            if (_itemType == ItemType.CONSUMABLE)
            {
                _itemData.Effect = _effect;
                _itemData.EffectPower = _effectPower;
            }

            //Weapon
            if (_itemType == ItemType.WEAPON)
            {
                _itemData.weaponType = _weaponType;
                _itemData.weaponData = _weaponData;
            }

            //Ammo
            if (_itemType == ItemType.AMMO)
            {
                _itemData.StackMax = _stackMax;
                _itemData.AmmoType = _ammoType;
            }


            _itemData.Width = _width;
            _itemData.Height = _height;
            _itemData.Fill = new bool[_width, _height];

            _itemData.SecondItem = new string[_secondItem.Length];
            _itemData.Result = new string[_result.Length];

            if (_secondItem.Length > 0)
            {
                Debug.Log("Second Item " + _secondItem.Length + " " + _secondItem[0]);
                Debug.Log("Result Item " + _result.Length + " " + _result[0]);
            }

            System.Array.Copy(_fieldsArray, _itemData.Fill, _fieldsArray.Length);
            System.Array.Copy(_secondItem, _itemData.SecondItem, _secondItem.Length);
            System.Array.Copy(_result, _itemData.Result, _result.Length);
            //itemData.fill = fieldsArray;

            _itemData.OnBeforeSerialize();

            EditorUtility.SetDirty(_itemData);
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
            _isKeyItem = _itemData.IsKeyItem;
            _itemType = _itemData.itemType;

            if (_itemData.Fill != null) _fieldsArray = _itemData.Fill;
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
        if (_hasCombinables && _secondItem != null)
        {
            secondItemProperty.arraySize = _secondItem.Length;
            resultProperty.arraySize = _result.Length;
            for (int i = 0; i < _secondItem.Length; i++)
            {
                secondItemProperty.GetArrayElementAtIndex(i).stringValue = _secondItem[i];
                resultProperty.GetArrayElementAtIndex(i).stringValue = _result[i];
            }
        }
        else
        {
            secondItemProperty.arraySize = 0;
            resultProperty.arraySize = 0;
        }

    }

}
