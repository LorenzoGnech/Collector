using UnityEngine;

[CreateAssetMenu(fileName = "Spawner.asset", menuName = "Collector/Spawners/Spawner", order = 0)]
public class SpawnerData : ScriptableObject {
    public GameObject itemToSpawn;
    public int minSpawn;
    public int maxSpawn;
}
