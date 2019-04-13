using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour {

    public SpawnController spawner;

    void Start()
    {
        Game.Instance.Spawner = spawner;
    }
}
