using System.Collections;
using System.Collections.Generic;
using ProjectD;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectDInternal
{
    /// <summary>
    /// Keep reference and update the Equipment entry (the 6 icons around the character in the Inventory)
    /// </summary>
    public class EquipmentUI : MonoBehaviour
    {
        //public ItemEntryUI HeadSlot;
        public ItemEntryUI TorsoSlot;
        //public ItemEntryUI LegsSlot;
        //public ItemEntryUI FeetSlot;
        //public ItemEntryUI AccessorySlot;
        public ItemEntryUI WeaponSlot;
        public ItemEntryUI[] SkillSlot;

        public void Init(InventoryUI owner)
        {
            //HeadSlot.Owner = owner;
            TorsoSlot.Owner = owner;
            //LegsSlot.Owner = owner;
            //FeetSlot.Owner = owner;
            //AccessorySlot.Owner = owner;
            WeaponSlot.Owner = owner;

            //初始化裝備UI裡的技能欄
            for(int i = 0; i < SkillSlot.Length; i++)
            {
                SkillSlot[i].Owner = owner;
                Debug.Log("初始技能欄");
            }
        }
        //安裝腳色技能於技能欄, 暫定3個技能
        public void SetSkill(Skill[] skill)
        {
            SkillSlot[0].SetupEquipment(skill[0]);
            SkillSlot[1].SetupEquipment(skill[1]);
            SkillSlot[2].SetupEquipment(skill[2]);

        }
        
        public void UpdateEquipment(EquipmentSystem equipment, StatSystem system)
        {
            //var head = equipment.GetItem(EquipmentItem.EquipmentSlot.Head);
            var torso = equipment.GetItem(EquipmentItem.EquipmentSlot.Torso);
            //var legs = equipment.GetItem(EquipmentItem.EquipmentSlot.Legs);
            //var feet = equipment.GetItem(EquipmentItem.EquipmentSlot.Feet);
            //var accessory = equipment.GetItem(EquipmentItem.EquipmentSlot.Accessory);
            var weapon = equipment.Weapon;

            //HeadSlot.SetupEquipment(head);
            TorsoSlot.SetupEquipment(torso);
            //LegsSlot.SetupEquipment(legs);
            //FeetSlot.SetupEquipment(feet);
            //AccessorySlot.SetupEquipment(accessory);
            WeaponSlot.SetupEquipment(weapon);

            //穿裝備時即時改變text值, 不受暫停影響
            GetComponentInParent<UISystem>().UpdateStatsText();
        }
    }
}