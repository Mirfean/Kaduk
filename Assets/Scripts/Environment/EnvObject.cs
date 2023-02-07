using Assets.Scripts;
using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnvObject : OutlineObject
{
    [SerializeField]
    private CursorManager _cursorManager;

    [SerializeField]
    public string Description;

    /*    [SerializeField]
        Material _baseMaterial;

        [SerializeField]
        Material _outlineMaterial;*/

    [SerializeField] public bool IsStash;

    [SerializeField] public bool IsPlayerStash;

    [SerializeField]
    GameManager _gameManager;

    [SerializeField]
    List<ItemData> _defaultItems;

    public ItemData[] StackItems;

    public int[] StackAmounts;

    public List<ItemData> Items { get { return _defaultItems; } set { _defaultItems = value; } }

    public Sprite DefaultSprite;

    public Sprite InUseSprite;

    new void Start()
    {
        base.Start();
        _cursorManager = FindObjectOfType<CursorManager>();
        _gameManager = FindObjectOfType<GameManager>();

        if (DefaultSprite == null) DefaultSprite = GetComponent<SpriteRenderer>().sprite;

        if (IsStash)
        {
        }


        //if (items == null) items = new List<ItemData>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnUse(Transform player)
    {
        Debug.Log("KWA");
    }

    private new void OnMouseEnter()
    {
        if (_gameManager.GetPlayerSTATE() == InteractionState.DEFAULT)
        {
            base.OnMouseEnter();
            gameObject.GetComponent<SpriteRenderer>().material = _outlineMaterial;
            HandleCursorAndText(true);
        }


    }

    private void OnMouseOver()
    {
        //Show Interact sign

    }

    private new void OnMouseExit()
    {
        //TODO execute method 
        base.OnMouseExit();
        gameObject.GetComponent<SpriteRenderer>().material = _baseMaterial;
        HandleCursorAndText(false);

    }

    private void OnMouseUpAsButton()
    {
        GameManager.HandleEnvAction(this);
    }

    void HandleCursorAndText(bool status)
    {
        if (status)
        {
            _cursorManager.ChangeCursorTo(CursorType.EYE);
        }
        else
        {
            _cursorManager.ChangeCursorTo(CursorType.STANDARD);
        }
    }

    internal void CloseSprite()
    {
        GetComponent<SpriteRenderer>().sprite = DefaultSprite;
    }
}
