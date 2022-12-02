using Assets.Scripts;
using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class _Item : MonoBehaviour
{
    [SerializeField]
    private CursorManager _cursorManager;

    [SerializeField]
    public string Description;

    [SerializeField]
    Material _baseMaterial;

    [SerializeField]
    Material _outlineMaterial;

    [SerializeField]
    bool _isStash;

    [SerializeField]
    TextMeshProUGUI _textMesh;

    [SerializeField]
    GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if (_gameManager == null) _gameManager = FindObjectOfType<GameManager>();
        _cursorManager = FindObjectOfType<CursorManager>();
        _baseMaterial = gameObject.GetComponent<SpriteRenderer>().material;
        _textMesh = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _textMesh.gameObject.SetActive(false);
        //Outline materials preparing
        Outlines basicOutline = Outlines.OutlineBasic;
        _outlineMaterial = Resources.Load($"Materials/{basicOutline.ToString()}") as Material;
        _outlineMaterial.SetColor("Outline Color", Color.white);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnUse(Transform player)
    {
        Debug.Log("KWA");
    }

    private void OnMouseEnter()
    {
        gameObject.GetComponent<SpriteRenderer>().material = _outlineMaterial;
        _cursorManager.ChangeCursorTo(CursorType.EYE);
        _textMesh.gameObject.SetActive(true);
    }

    private void OnMouseOver()
    {
        //Show Interact sign
        
    }

    private void OnMouseExit()
    {
        //TODO execute method 
        _cursorManager.ChangeCursorTo(CursorType.STANDARD);
        gameObject.GetComponent<SpriteRenderer>().material = _baseMaterial;
        _textMesh.gameObject.SetActive(false);
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log($"{Description}");
        _gameManager.ShowPlayerStash();
    }
}
