using UnityEngine;
using System.Collections;

public class Enemy : PersistableObject {

    MeshRenderer meshRenderer;
    static int colorPropertyId = Shader.PropertyToID("_Color");
    static MaterialPropertyBlock sharedPropertyBlock;

    public EnemySettings settings;

    [HideInInspector]
    public bool enemySettingsFoldout;

    private int enemyId = int.MinValue;
    public int EnemyId
    {
        get { return enemyId; }
        set {
            if (enemyId == int.MinValue && value != int.MinValue)
            {
                enemyId = value;
            }
        }
    }

    public int MaterialId { get; private set; }
    public void SetMaterial(Material material, int materialId)
    {
        meshRenderer.material = material;
        MaterialId = materialId;
    }

    private Color color;
    public void SetColor(Color color)
    {
        this.color = color;
        if (sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
        sharedPropertyBlock.SetColor(colorPropertyId, color);
        meshRenderer.SetPropertyBlock(sharedPropertyBlock);
    }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnEnemySettingsUpdated()
    {

    }

    public void OnColorSettingsUpdated()
    {

    }

    IEnumerator ChangeColorOnHit()
    {
        // Save color before hit - then change to hitColor (red here) and wait
        Color before = color;
        SetColor(Color.red);
        yield return new WaitForSeconds(0.10f);

        // Change color back to normal
        SetColor(before);
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(color);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        SetColor(reader.ReadColor());
    }
}
