using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public static UserInput Instance { get; private set; }

    public Player Input;

    private void Awake()
    {
        Input = new Player();
        Input.Enable();
    }

    private void OnEnable()
    {
        Input.Enable();
    }

    private void OnDisable()
    {
        Input.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetBasicScreenToWorld()
    {
        return Camera.main.ScreenToWorldPoint(Input.Basic.MouseMovement.ReadValue<Vector2>());
    }

    public Vector2 GetUIScreenToWorld()
    {
        return Camera.main.ScreenToWorldPoint(Input.UI.MousePosition.ReadValue<Vector2>());
    }

}
