using UnityEngine;
using System;
using CraftyTower.Items;

public class LootDrop : MonoBehaviour {

    public delegate void InventoryManager(LootDrop i);
    public static event InventoryManager OnLoot;

    private ItemTypes _type;
    [SerializeField]
    private Sprite _itemSprite;
    [SerializeField]
    private int _count = 0;

    private SpriteRenderer spriteRend;

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
        // set sprite
        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.sprite = _itemSprite;
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
        if(OnLoot != null)
        {
            OnLoot(this);
        }
        wasLooted = true;
        transform.rotation = Quaternion.identity;
    }
}
