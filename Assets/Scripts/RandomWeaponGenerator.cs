using UnityEngine;

public class RandomWeaponGenerator : MonoBehaviour
{
    

    string[] itemName = new string[] { "黃色武器", "藍色武器", "白色武器" };

    public string GetRandomItem()
    {
        //pick item name
        int itemIndex = Random.Range(0, itemName.Length);
        string weaponName = itemName[itemIndex];

        return weaponName;
    }
}
