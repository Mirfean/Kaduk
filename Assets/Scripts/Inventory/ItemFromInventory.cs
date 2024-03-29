using TMPro;
using UnityEngine;
using UnityEngine.UI;


enum Rotation
{
    r0 = 0, // Anchors min(0,1) max(0,1) pivot(0,1)
    r90 = 1, // Anchors min(1,1) max(1,1) pivot(1,1)
    r180 = 2, // Anchors min(1,0) max(1,0) pivot(1,0)
    r270 = 3 // Anchors min(0,0) max(0,0) pivot(0,0)
}

/// <summary>
/// Created item in inventory
/// </summary>
public class ItemFromInventory : MonoBehaviour
{
    [SerializeField]
    public string ItemDescription;

    public string ItemName;

    public ItemData ItemInvData;

    Rotation _rotation = Rotation.r0;

    public bool[,] SpaceFill;

    public int OnGridPositionX;

    public int OnGridPositionY;

    [SerializeField]
    internal InventoryGrid _grid;

    public int HEIGHT
    {
        get
        {
            if (_rotation == Rotation.r0 || _rotation == Rotation.r180) return ItemInvData.Height;
            else return ItemInvData.Width;
        }

    }

    public int WIDTH
    {
        get
        {
            if (_rotation == Rotation.r0 || _rotation == Rotation.r180) return ItemInvData.Width;
            else return ItemInvData.Height;
        }
    }

    public bool IsWeapon;

    public TextMeshProUGUI AmountText;

    public bool IsStackable;

    public int Amount;

    public void Awake()
    {
        Debug.Log("I'm alive!");
    }

    public void OnMouseEnter()
    {
        InventoryManager.OnMouseAboveItem(this);
    }

    public void OnMouseDown()
    {
        ClickHoverManager.OnHoverOpen(transform.parent.GetComponent<InventoryGrid>().stashType);
    }

    public void OnMouse()
    {
        ClickHoverManager.OnHoverOpen(transform.parent.GetComponent<InventoryGrid>().stashType);
    }

    public ItemData itemData
    {
        get { return ItemInvData; }
        set
        {
            ItemInvData = value;

            GetComponent<Image>().sprite = ItemInvData.ItemIcon;

            ItemName = ItemInvData.ItemName;

            ItemDescription = ItemInvData.Description;

            Vector2 size = new Vector2(WIDTH * InventoryGrid.TileSizeWidth, HEIGHT * InventoryGrid.TileSizeHeight);

            GetComponent<RectTransform>().sizeDelta = size;

            SpaceFill = ItemInvData.Fill;
        }
    }

    internal void Set(ItemData itemdata)
    {
        this.itemData = itemdata;
    }

    internal void rotate()
    {
        SpaceFill = rotateFill();

        if (_rotation == Rotation.r270) _rotation = Rotation.r0;
        else _rotation += 1;

        this.gameObject.transform.Rotate(0f, 0f, 90f);

        ChangePivot();

    }

    private bool[,] rotateFill()
    {
        bool[,] newFill = new bool[HEIGHT, WIDTH];
        for (int i = 0; i < WIDTH; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                Debug.Log($"for new[{j},{i}] value of old[{WIDTH - 1 - i},{ HEIGHT - (HEIGHT - j)}]");
                newFill[j, i] = SpaceFill[WIDTH - 1 - i, HEIGHT - (HEIGHT - j)];
            }
        }
        return newFill;
    }

    public void ChangePivot()
    {
        Vector2 newValue = new Vector2();

        switch (_rotation)
        {
            case Rotation.r0:
                {
                    newValue = new Vector2(0, 1);
                    break;
                }
            case Rotation.r90:
                {
                    newValue = new Vector2(1, 1);
                    break;
                }

            case Rotation.r180:
                {
                    newValue = new Vector2(1, 0);
                    break;
                }

            case Rotation.r270:
                {
                    newValue = new Vector2(0, 0);

                    break;
                }

        }

        gameObject.GetComponent<RectTransform>().pivot = newValue;
        gameObject.GetComponent<RectTransform>().anchorMin = newValue;
        gameObject.GetComponent<RectTransform>().anchorMax = newValue;
        gameObject.GetComponent<RectTransform>().anchoredPosition = newValue;
    }

    public int ReloadAmount(int needed)
    {
        if (Amount > needed)
            {
                Amount =- needed;
                return needed;
            }
        else
            {
                Debug.Log("Not enought ammo!");
                return Amount;
            }
    }
}
