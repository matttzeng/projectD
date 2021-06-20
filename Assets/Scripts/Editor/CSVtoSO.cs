using UnityEditor;
using System.IO;
using UnityEngine;
using ProjectD;


public class CSVtoSO : MonoBehaviour
{
    
    private static string enemyCSVPath = "/Scripts/Editor/CSVs/EnemyCSV.csv";
    private static string weaponCSVPath = "/Scripts/Editor/CSVs/WeaponCSV.csv";



    [MenuItem("Utilities/Generate Enemies")]
    public static void GenerateEnemies()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + enemyCSVPath);

      
        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');




            if (splitData.Length != 3 )
            {

                Debug.Log("怪物資料不正確");
                continue;
            }


            Enemy enemy = ScriptableObject.CreateInstance<Enemy>();
            enemy.enemyName = splitData[0];
            enemy.HP = int.Parse(splitData[1]);
            enemy.Attack = int.Parse(splitData[2]);

            AssetDatabase.CreateAsset(enemy, $"Assets/Creator Kit - Beginner Code/Prefabs/Enemies/{enemy.enemyName}.asset");

        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Enemies")]
    public static void GenerateWeapon()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + weaponCSVPath);



        foreach (string s in allLines)
        {
         
            
                string[] splitData = s.Split(',');




                if (splitData.Length != 3)
                {

                    Debug.Log("武器資料不正確");
                    return;
                }
            
            

            Weapon weapon = ScriptableObject.CreateInstance<Weapon>();

            weapon.ItemSprite = Resources.Load < Sprite>(splitData[0] );
            weapon.ItemName = splitData[1];
            weapon.Description = splitData[2];
            //weapon.WorldObjectPrefab = FindObjectOfType<PrefabType>();

















            AssetDatabase.CreateAsset(weapon, $"Assets/Creator Kit - Beginner Code/Prefabs/Weapons/{weapon.ItemName}.asset");

        }

        AssetDatabase.SaveAssets();
    }

    
}
