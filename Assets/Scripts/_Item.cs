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
    TextMeshProUGUI _textMesh;

    [SerializeField]
    GameManager _gameManager;

/*    // Start is called before the first frame update
    new void Start()
    {
        if (_gameManager == null) _gameManager = FindObjectOfType<GameManager>();
        _cursorManager = FindObjectOfType<CursorManager>();
        _baseMaterial = gameObject.GetComponent<SpriteRenderer>().material;
        if (_isStash)
        {
            _textMesh = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _textMesh.gameObject.SetActive(false);
        }
        //Outline materials preparing
        Outlines basicOutline = Outlines.OutlineBasic;
        _outlineMaterial = Resources.Load($"Materials/{basicOutline.ToString()}") as Material;
        _outlineMaterial.SetColor("Outline Color", Color.white);
    }*/

    new void Start()
    {
        base.Start();
        if (_gameManager == null) _gameManager = FindObjectOfType<GameManager>();
        _cursorManager = FindObjectOfType<CursorManager>();
        if (_isStash)
        {
            _textMesh = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _textMesh.gameObject.SetActive(false);
        }
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
        base.OnMouseEnter();
        gameObject.GetComponent<SpriteRenderer>().material = _outlineMaterial;
        HandleCursorAndText(true);
        
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

        Debug.Log($"{Description}");
        if (_isStash)
        {
            _gameManager.ShowPlayerStash();
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
