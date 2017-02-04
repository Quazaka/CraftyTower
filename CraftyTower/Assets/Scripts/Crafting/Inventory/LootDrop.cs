using UnityEngine;
using UnityEngine.UI;
using System;
using CraftyTower.Items;

[RequireComponent(typeof(SpriteRenderer), typeof(Image))]
public class LootDrop : MonoBehaviour {

    public delegate void Inventory(LootDrop loot);
    public static event Inventory OnLoot;

    private ItemTypes _type;
    private Sprite _itemSprite;
    private int _count = 0;

    // spriteRenderer is used in world space (when loot is dropped - instantiated)
    private SpriteRenderer spriteRend;
    // Image is used in the UI - makes dragging it around much easier
    private Image inventoryImage;

    private bool wasLooted;

    #region Getters
    public ItemTypes Type
    {
        get { return _type; }
    }
        
    public Sprite ItemSprite
    {
        get { return _itemSprite; }
        set { _itemSprite = value; }
    }

    public int ItemCount
    {
        get { return _count; }
        set { _count = value; }
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        inventoryImage = GetComponent<Image>();
        SetSprite();
    }

    // Update is called once per frame
    void Update()
    {
        if (!wasLooted)
        {
            transform.Rotate(Vector3.up, 40 * Time.deltaTime);
        }       
    }

    void OnMouseUp()
    {
        if (OnLoot != null)
        {
            OnLoot(this);
            wasLooted = true;
            transform.rotation = Quaternion.identity;
            SetSprite();
        }  
    }

    // Changes how to show the loot based on whether it is looted or not
    public void SetSprite()
    {
        if (!wasLooted)
        {
            spriteRend.sprite = _itemSprite;
        }
        else
        {
            inventoryImage.sprite = _itemSprite;
        }
    }
}
