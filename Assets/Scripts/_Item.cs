using Assets.Scripts;
using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class _Item : OutlineObject
{
    [SerializeField]
    private CursorManager _cursorManager;

    [SerializeField]
    public string Description;

/*    [SerializeField]
    Material _baseMaterial;

    [SerializeField]
    Material _outlineMaterial;*/

    [SerializeField]
    bool _isStash;

    [SerializeField]
    bool _isPlayerStash;

    [SerializeField]
    TextMeshProUGUI _textMesh;

    [SerializeField]
    GameManager _gameManager;

    [SerializeField]
    List<ItemData> items;

    public List<ItemData> Items { get { return items; } set { items = value; } }

    new void Start()
    {
        base.Start();
        _cursorManager = FindObjectOfType<CursorManager>();
        _gameManager = FindObjectOfType<GameManager>();
        if (_isStash)
        {
            _textMesh = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _textMesh.gameObject.SetActive(false);
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
        if (_gameManager.GetPlayerSTATE() == InteractionState.DEFAULT)
        {
            if (_gameManager == null && (_isStash || _isPlayerStash)) _gameManager = FindObjectOfType<GameManager>();
            Debug.Log($"{Description}");
            if (_isStash)
            {
                _gameManager.ShowNormalStash(this);
            }
            if (_isPlayerStash)
            {
                _gameManager.ShowPlayerStash();
            }
        }
    }

    void HandleCursorAndText(bool status)
    {
        if (status)
        {
            _cursorManager.ChangeCursorTo(CursorType.EYE);
            if (_textMesh != null) _textMesh.gameObject.SetActive(true);
        }
        else
        {
            _cursorManager.ChangeCursorTo(CursorType.STANDARD);
            if (_textMesh != null) _textMesh.gameObject.SetActive(false);
        }
    }

}
