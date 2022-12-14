using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : OutlineObject
{
    [SerializeField]
    private Door _destination;

    public Door Destination { get { return _destination; } }

    [SerializeField]
    private Transform _spawnPoint;

    public Transform SpawnPoint { get { return _spawnPoint; } }

    [SerializeField]
    private Collider _collider;

    private GameManager _gameManager;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        _spawnPoint = gameObject.GetComponentInChildren<Transform>();
        _collider = _spawnPoint.GetComponent<Collider>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private new void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    private void OnMouseUpAsButton()
    {
        if (_gameManager == null) FindObjectOfType<GameManager>();
        _gameManager.TransferPlayer(this);
    }

    private new void OnMouseExit()
    {
        base.OnMouseExit();
    }
}
