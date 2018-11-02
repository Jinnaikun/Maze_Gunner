using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public float enemySpawnTime; //Time it takes for one enemy to spawn
	public GameObject [] prefabArray; //Array holding different enemies
	public GameObject [] locationArray; //Array holding the different spawn points
    


     void Start()
    {
        InvokeRepeating("SpawnRandom", enemySpawnTime, enemySpawnTime);
    }


 
    //Spawns random enemies at random enemy spawners
	void SpawnRandom()
	{
        int prefabToSpawn;
		int locationToSpawnAt;

		prefabToSpawn = Random.Range(0,prefabArray.Length);
		locationToSpawnAt = Random.Range (0, locationArray.Length);

       

		Instantiate(prefabArray[prefabToSpawn], locationArray[locationToSpawnAt].transform);


	}
}
