using UnityEngine;

public class RandomWeaponGenerator : MonoBehaviour
{
    

    string[] itemName = new string[] { "����Z��", "�Ŧ�Z��", "�զ�Z��" };

    public string GetRandomItem()
    {
        //pick item name
        int itemIndex = Random.Range(0, itemName.Length);
        string weaponName = itemName[itemIndex];

        return weaponName;
    }
}
