using ProjectD;
using UnityEngine;
using ProjectDInternal;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform miniBossPrefab;
    public Transform bigBossPrefab;
    public  Transform spawnPoint;
    public GameObject[] enemies;
    public float timeBetweenWaves = 5f;
    private float countdown = 2f;
    public int addHealth;
    public int addAttack;
    public int addDefense;
    public int addSpeed;
    //public int addDetection;

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

        if (waveNumber %5 == 0)
        {
            SpwanMiniBoss();
        }

        if (waveNumber %10 == 0)
        {
            SpwanBigBoss();
        }

        //每波增加怪物素質強度
        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("enemy");
        foreach(var enemy in enemies2)
        {
            enemy.GetComponent<CharacterData>().Stats.baseStats.health += (waveNumber - 1) * addHealth;
            enemy.GetComponent<CharacterData>().Stats.baseStats.attack += (waveNumber - 1) * addAttack;
            enemy.GetComponent<CharacterData>().Stats.baseStats.defense += (waveNumber - 1) * addDefense;
            //enemy.AddComponent<SimpleEnemyController>().detectionRadius += (waveNumber - 1) * addDetection;
            enemy.AddComponent<SimpleEnemyController>().Speed += (waveNumber - 1) * addSpeed;
            
        }

        waveNumber++;

    }
    void SpwanEnemy()
    {
        spawnPoint.position = new Vector3(0, spawnPoint.position.y, 0);
        spawnPoint.position =new Vector3(spawnPoint.position.x + Random.Range(-10f, 10f), spawnPoint.position.y, spawnPoint.position.z + Random.Range(-10f, 10f));
        
        
   
        Instantiate(enemyPrefab,spawnPoint.position,spawnPoint.rotation);


    }

    void SpwanMiniBoss()
    {
        spawnPoint.position = new Vector3(0, spawnPoint.position.y, 0);
        spawnPoint.position = new Vector3(spawnPoint.position.x + Random.Range(-10f, 10f), spawnPoint.position.y, spawnPoint.position.z + Random.Range(-10f, 10f));



        Instantiate(miniBossPrefab, spawnPoint.position, spawnPoint.rotation);

    }

    void SpwanBigBoss()
    {
        spawnPoint.position = new Vector3(0, spawnPoint.position.y, 0);
        spawnPoint.position = new Vector3(spawnPoint.position.x + Random.Range(-10f, 10f), spawnPoint.position.y, spawnPoint.position.z + Random.Range(-10f, 10f));



        Instantiate(bigBossPrefab, spawnPoint.position, spawnPoint.rotation);

    }

    public void RestartSpwan()
    {
        for(int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i].gameObject);
        }
      
        waveNumber = 0;
        SpawnWave();
        Debug.Log("重新生怪, waveNumber =" + waveNumber);
    }
}
