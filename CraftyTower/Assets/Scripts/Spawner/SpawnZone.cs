using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CraftyTower.Spawner;

public abstract class SpawnZone : MonoBehaviour {

    public abstract Vector3 SpawnPoint { get; }
}
