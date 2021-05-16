using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] List<Transform>  spawnPoints = new List<Transform>();
    [SerializeField] private float spawnRadius;

    public List<GameItem> itemsSpawned = new List<GameItem>();
    // Start is called before the first frame update
    void Start()
    {
        Main.instance.EventManager.SubscribeToEvent(GameEvent.StartNewWave, SpawnPickups);
        Main.instance.EventManager.SubscribeToEvent(GameEvent.KilledAllEnemiesSpawned, ClearPickUps);
    }

    void ClearPickUps()
    {
        foreach (var item in itemsSpawned)
        {
            Debug.Log("rompo");
            Destroy(item.gameObject);
        }
        
        itemsSpawned.Clear();
    }
    
    void SpawnPickups()
    {
        
        BubleItem pickable = Resources.Load<BubleItem>("Buble");

        var random = (Random.insideUnitCircle * spawnRadius);

        var newVector = new Vector3(random.x, 0, random.y);

        foreach (var spawnPoint in spawnPoints)
        {
            Vector3 realSpawnPos = new Vector3(newVector.x, spawnPoint.localPosition.y, newVector.z);
            var currentPickUp = Instantiate(pickable, spawnPoint.position, spawnPoint.rotation);
            currentPickUp.transform.position = realSpawnPos + spawnPoint.position;
        
            itemsSpawned.Add(currentPickUp);
        }
    }
}
