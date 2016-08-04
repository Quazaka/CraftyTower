namespace CraftyTower.Spawner
{
    /// <summary>
    /// Different ways of spawning enemies
    /// </summary>
    public enum SpawnMode
    {
        /// <summary>
        /// Spawn enemies until limit is hit, then stop.
        /// </summary>
        Single,
        /// <summary>
        /// Spawn a wave of enemies. When all enemies are dead, spawn a new wave
        /// </summary>
        Wave,
        /// <summary>
        /// Spawn a wave enemies over time. A new wave will spawn after the specified time
        /// regardless of whether all enemies are dead or not
        /// </summary>
        TimedWave,
    }

    /// <summary>
    /// Types of enemies that can be spawned (Sort by alphabethic because we use Resources.LoadAll)
    /// </summary>
    public enum EnemyTypes
    {
        Boss = 0,
        Fast,
        Normal
    }
}

