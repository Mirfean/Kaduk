using Assets.Scripts.Enums;
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

    public void HandleNewState(InteractionState state)
    {
        switch (state)
        {
            case InteractionState.DEFAULT:
                Instance.Input.UI.Disable();
                Instance.Input.Basic.Enable();
                break;

            case InteractionState.INVENTORY:
                Instance.Input.Basic.Disable();
                Instance.Input.UI.Enable();
                break;
        }
    }

    public Vector2 GetBasicMousePos()
    {
        if (Instance.Input.Basic.enabled)
            //Debug.Log(Instance.Input.Basic.MouseMovement.ReadValue<Vector2>());
            return Instance.Input.Basic.MouseMovement.ReadValue<Vector2>();
        Debug.Log("Basic Input not enabled!");
        return new Vector2();

    }

    public Vector2 GetUIMousePos()
    {
        if (Instance.Input.UI.enabled)
            return Instance.Input.UI.MousePosition.ReadValue<Vector2>();
        Debug.Log("UI not enabled!");
        return new Vector2();
    }

    public Vector2 GetBasicScreenToWorld()
    {
        //Debug.Log("Screen to world " + Camera.main.ScreenToWorldPoint(Instance.Input.Basic.MouseMovement.ReadValue<Vector2>()));
        return Camera.main.ScreenToWorldPoint(GetBasicMousePos());
    }

    public Vector2 GetUIScreenToWorld()
    {
        return Camera.main.ScreenToWorldPoint(Input.UI.MousePosition.ReadValue<Vector2>());
    }


}
