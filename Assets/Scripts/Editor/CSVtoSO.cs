using UnityEditor;
using System.IO;
using UnityEngine;

using ProjectD;


public class CSVtoSO : MonoBehaviour
{
    
    private static string enemyCSVPath = "/Scripts/Editor/CSVs/EnemyCSV.csv";
    private static string weaponCSVPath = "/Scripts/Editor/CSVs/WeaponCSV.csv";
    
    //weapon test file
     private static string weaponTestCSVPath = "/Scripts/Editor/CSVs/WeaponTestCSV.csv";

    private static string anyWeaponPath = "/ItemDataBase/anyWeapon.asset";




    //save scriptableObj to json 
    [MenuItem("Utilities/Generate TestWeapon")]
    public static void WeaponSaveToFile()
    {
       Weapon w=  (Weapon)AssetDatabase.LoadAssetAtPath("Assets/ItemDataBase/anyWeapon.asset", typeof(Weapon));
       string json = JsonUtility.ToJson(w);
       File.WriteAllText("Assets/"+weaponTestCSVPath, json);
     }
    [MenuItem("Utilities/Read TestWeapon")]

    public static void WeaponCSVtoJson()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + weaponCSVPath);


        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length != 3)
            {
                Debug.Log("怪物資料不正確");
                continue;
            }

            //把各項數值對應起來

            Weapon weapon = ScriptableObject.CreateInstance<Weapon>();
            weapon.ItemName = splitData[0];
            weapon.Description = splitData[1];
         






            AssetDatabase.CreateAsset(weapon, $"Assets/Creator Kit - Beginner Code/Prefabs/Weapons/{weapon.ItemName}.asset");

        }

        AssetDatabase.SaveAssets();
}
    public static void WeaponReadToFile()
    {
       

        Weapon weapon = ScriptableObject.CreateInstance<Weapon>();

        string jsonString =  File.ReadAllText("Assets/Scripts/Editor/CSVs/WeaponTestCSV_fix.csv");
        JsonUtility.FromJsonOverwrite(jsonString, weapon);

        AssetDatabase.CreateAsset(weapon, $"Assets/Creator Kit - Beginner Code/Prefabs/Weapons/{weapon.ItemName}.asset");
        AssetDatabase.SaveAssets();
    

    }













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

    [MenuItem("Utilities/Generate Weapon")]
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
       

















            AssetDatabase.CreateAsset(weapon, $"Assets/Creator Kit - Beginner Code/Prefabs/Weapons/{weapon.ItemName}.asset");

        }

        AssetDatabase.SaveAssets();
    }




    }
  
