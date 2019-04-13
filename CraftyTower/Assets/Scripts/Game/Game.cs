using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Game : PersistableObject {

    public static Game Instance { get; private set; }

    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;

    [SerializeField]
    PersistentStorage Storage;

    public SpawnController Spawner { get; set; }

	List<Enemy> enemies;

    private void OnEnable()
    {
        Instance = this;
    }

    void Update () {
		if (Input.GetKeyDown(saveKey)) {
			Storage.Save(this);
		}
		else if (Input.GetKeyDown(loadKey)) {
			BeginNewGame();
			Storage.Load(this);
		}
        Spawner.Spawn();
    }

    void BeginNewGame () {
		for (int i = 0; i < enemies.Count; i++) {
            //enemyFactory.Reclaim(enemies[i]);
		}
		enemies.Clear();
	}

	public override void Save (GameDataWriter writer) {
		writer.Write(enemies.Count);
		for (int i = 0; i < enemies.Count; i++) {
			writer.Write(enemies[i].EnemyId);
			writer.Write(enemies[i].MaterialId);
			enemies[i].Save(writer);
		}
	}

	public override void Load (GameDataReader reader) {
		int count = reader.ReadInt();
		for (int i = 0; i < count; i++) {
            int enemyId = reader.ReadInt();
            int materialId = reader.ReadInt();
			//Enemy instance = enemyFactory.Get(enemyId, materialId);
			//instance.Load(reader);
			//enemies.Add(instance);
		}
	}
}