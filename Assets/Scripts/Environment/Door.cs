using UnityEngine;

public class Door : OutlineObject
{
    [SerializeField]
    private Door _destination;
    public Door Destination { get { return _destination; } }

    [SerializeField] public Room ThisRoom;

    [SerializeField]
    private Transform _spawnPoint;

    public Transform SpawnPoint { get { return _spawnPoint; } }

    [SerializeField]
    private Collider _collider;

    private GameManager _gameManager;

    public bool Closed;

    public string KeyID;


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
        GameManager.HandleDoorAction(this);
    }

    private new void OnMouseExit()
    {
        base.OnMouseExit();
    }



}
