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
        //public ItemEntryUI TorsoSlot;
        //public ItemEntryUI LegsSlot;
        //public ItemEntryUI FeetSlot;
        //public ItemEntryUI AccessorySlot;
        public ItemEntryUI WeaponSlot;
        //public ItemEntryUI SkillSlot;

        public void Init(InventoryUI owner)
        {
            //HeadSlot.Owner = owner;
            //TorsoSlot.Owner = owner;
            //LegsSlot.Owner = owner;
            //FeetSlot.Owner = owner;
            //AccessorySlot.Owner = owner;
            WeaponSlot.Owner = owner;
            //SkillSlot.Owner = owner;
        }

        public void UpdateEquipment(EquipmentSystem equipment, StatSystem system)
        {
            var head = equipment.GetItem(EquipmentItem.EquipmentSlot.Head);
            var torso = equipment.GetItem(EquipmentItem.EquipmentSlot.Torso);
            var legs = equipment.GetItem(EquipmentItem.EquipmentSlot.Legs);
            var feet = equipment.GetItem(EquipmentItem.EquipmentSlot.Feet);
            var accessory = equipment.GetItem(EquipmentItem.EquipmentSlot.Accessory);
            var weapon = equipment.Weapon;
            //var skill = equipment.Skill;

            //HeadSlot.SetupEquipment(head);
            //TorsoSlot.SetupEquipment(torso);
            //LegsSlot.SetupEquipment(legs);
            //FeetSlot.SetupEquipment(feet);
            //AccessorySlot.SetupEquipment(accessory);
            WeaponSlot.SetupEquipment(weapon);
            //SkillSlot.SetupEquipment(skill);

            //穿裝備時即時改變text值, 不受暫停影響
            GetComponentInParent<UISystem>().UpdateStatsText();
        }
    }
}