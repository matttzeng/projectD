
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public  Transform spawnPoint;
    public GameObject[] enemies;
    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    public static int waveNumber = 1;

    private void Update()
    {

        enemies = GameObject.FindGameObjectsWithTag("enemy");


        if (enemies.Length == 0)
        {
            if (countdown <= 0f)
            {
                SpawnWave();
                countdown = timeBetweenWaves;
            }

            countdown -= Time.deltaTime;
        }
    }
    void SpawnWave()
    {
        for (int i =0;i<waveNumber; i++)
        {
            SpwanEnemy();
        }

        waveNumber++;

    }
    void SpwanEnemy()
    {
        spawnPoint.position = new Vector3(0, spawnPoint.position.y, 0);
        spawnPoint.position =new Vector3(spawnPoint.position.x + Random.Range(-10f, 10f), spawnPoint.position.y, spawnPoint.position.z + Random.Range(-10f, 10f));
        
        
   
        Instantiate(enemyPrefab,spawnPoint.position,spawnPoint.rotation);

    }





}
