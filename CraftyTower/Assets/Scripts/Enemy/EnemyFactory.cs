using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class EnemyFactory : ScriptableObject {

    [SerializeField]
    Enemy[] prefabs;

    [SerializeField]
    Material[] materials;

    List<Enemy>[] pools;

    Scene poolScene;

    public Enemy Get(int enemyId = 0, int materialId = 0)
    {
        Enemy instance;
        if (pools == null)
        {
            CreatePools();
        }

        List<Enemy> pool = pools[enemyId];
        int lastIndex = pool.Count - 1;
        if (lastIndex >= 0)
        {
            instance = pool[lastIndex];
            instance.gameObject.SetActive(true);
            pool.RemoveAt(lastIndex);
        }
        else
        {
            instance = Instantiate(prefabs[enemyId]);
            instance.EnemyId = enemyId;
            SceneManager.MoveGameObjectToScene(instance.gameObject, poolScene);
        }
        instance.SetMaterial(materials[materialId], materialId);
        return instance;
    }

    public Enemy GetRandom()
    {
        return Get(
            Random.Range(0, prefabs.Length),
            Random.Range(0, materials.Length)
        );
    }

    void CreatePools()
    {
        pools = new List<Enemy>[prefabs.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<Enemy>();
        }
        poolScene = SceneManager.CreateScene(name);
    }

    public void Reclaim(Enemy enemyToRecycle)
    {
        if (pools == null)
        {
            CreatePools();
        }
        pools[enemyToRecycle.EnemyId].Add(enemyToRecycle);
        enemyToRecycle.gameObject.SetActive(false);
    }
}
