using UnityEngine;
using System.Collections.Generic;
using CraftyTower.Upgrades;
using System.Linq;

public abstract class BaseRoom : MonoBehaviour
{
    public abstract void AddStats();

    public abstract void RemoveStats();

    public abstract void GetStats();

}
