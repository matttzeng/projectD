using ProjectD;
using UnityEngine;
using UnityEngine.UI;
using ProjectDInternal;

public class WaveSpawner : MonoBehaviour
{
    public Transform[] enemyPrefab;
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
    public bool loopGame = false;
    public CharacterData PlayerData;
    public Text PotionText;
    public static int potionCount = 0;
    public GameObject potionButton;

    
    //public int addDetection;

    public static int waveNumber = 0;

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

        //�Ʋ��ץ�  �Cwave�����m
        PlayerData.GetComponentInChildren<Animator>().transform.position = PlayerData.GetComponent<Transform>().transform.position;
      


        WaveSelection(loopGame);

        int i = waveNumber;
        if(waveNumber >= 5)
            i = 5 + Mathf.FloorToInt(waveNumber / 5);
        Debug.Log("�ͩǼ�: " + i);

        for (int j =0;j<i; j++)
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

        if (loopGame == false)
        {
            //�C�i�W�[�Ǫ�����j��
            GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("enemy");
            foreach (var enemy in enemies2)
            {
                //�C10���j�׼W�[10%
                if (waveNumber % 10 == 0)
                {
                    enemy.GetComponent<CharacterData>().Stats.baseStats.health = Mathf.FloorToInt(enemy.GetComponent<CharacterData>().Stats.baseStats.health * 1.1f);
                    enemy.GetComponent<CharacterData>().Stats.baseStats.attack = Mathf.FloorToInt(enemy.GetComponent<CharacterData>().Stats.baseStats.attack * 1.1f);
                    enemy.GetComponent<CharacterData>().Stats.baseStats.defense = Mathf.FloorToInt(enemy.GetComponent<CharacterData>().Stats.baseStats.defense * 1.1f);
                }

                enemy.GetComponent<CharacterData>().Stats.baseStats.health += (waveNumber - 1) * addHealth;
                enemy.GetComponent<CharacterData>().Stats.baseStats.attack += (waveNumber - 1) * addAttack;
                enemy.GetComponent<CharacterData>().Stats.baseStats.defense += (waveNumber - 1) * addDefense;
                //enemy.AddComponent<SimpleEnemyController>().detectionRadius += (waveNumber - 1) * addDetection;
                //enemy.AddComponent<SimpleEnemyController>().Speed += (waveNumber - 1) * addSpeed;

            }
        }

    }
    void SpwanEnemy()
    {
        spawnPoint.position = new Vector3(0, spawnPoint.position.y, 0);
        spawnPoint.position =new Vector3(spawnPoint.position.x + Random.Range(-10f, 10f), spawnPoint.position.y, spawnPoint.position.z + Random.Range(-10f, 10f));


        int i = Random.Range(0, 4);
        Instantiate(enemyPrefab[i],spawnPoint.position,spawnPoint.rotation);


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
        potionCount = 0;
        //SpawnWave();
        Debug.Log("���s�ͩ�, waveNumber =" + waveNumber);
    }
    //�C��������P�wLoop�P�_, Loop�ɱҰʨ���L��
    public void WaveSelection(bool loopGame)
    {
        PlayerData.Stats.unbeatable = false;
        //��������Loop, �i�U�@���}�lLoop
        if (waveNumber % 5 == 0 && loopGame == true)
        {
            waveNumber++;
            if (potionCount > 0)
                potionCount--;
            PotionCount();
            PlayerData.Stats.unbeatable = true;
        }
            
        else if (loopGame == false)
        {
            waveNumber++;
            if (potionCount > 0)
                potionCount--;
            PotionCount();
        }            

        else if (loopGame == true)
        {
            PlayerData.Stats.unbeatable = true;
            //return;
        }
    }
    //�v¡�Ĥ��N�o�^�X
    public void PotionCount()
    {        
        if (potionCount <= 0)
        {
            PotionText.text = "";
            potionButton.SetActive(true);
        }
           
        
        else
            PotionText.text = potionCount.ToString();
    }

    public void LoopGame()
    {
        if (loopGame == false)
            loopGame = true;
        else if (loopGame == true)
            loopGame = false;
    }
}
