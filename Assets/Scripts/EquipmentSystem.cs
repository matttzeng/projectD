using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectD
{
    /// <summary>
    /// Handles the equipment stored inside an instance of CharacterData. Will take care of unequipping the previous
    /// item when equipping a new one in the same slot.
    /// </summary>
    public class EquipmentSystem
    {
        public Weapon _Weapon { get; private set; }
        //public Skill Skill { get; private set; }

        public Action<EquipmentItem> OnEquiped { get; set; }
        public Action<EquipmentItem> OnUnequip { get; set; }

        CharacterData m_Owner;
        
        EquipmentItem m_HeadSlot;
        EquipmentItem m_TorsoSlot;
        EquipmentItem m_LegsSlot;
        EquipmentItem m_FeetSlot;
        EquipmentItem m_AccessorySlot;

        Weapon m_DefaultWeapon;
        //Skill m_DefaultSkill;

        public void Init(CharacterData owner)
        {
            m_Owner = owner;
        }
        
        public void InitWeapon(Weapon wep, CharacterData data)
        {
            m_DefaultWeapon = wep;
        }

        /*public void InitSkill(Skill wep, CharacterData data)
        {
            m_DefaultSkill = wep;
        }*/

        public EquipmentItem GetItem(EquipmentItem.EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentItem.EquipmentSlot.Head:
                    return m_HeadSlot;
                case EquipmentItem.EquipmentSlot.Torso:
                    return m_TorsoSlot;
                case EquipmentItem.EquipmentSlot.Legs:
                    return m_LegsSlot;
                case EquipmentItem.EquipmentSlot.Feet:
                    return m_FeetSlot;
                case EquipmentItem.EquipmentSlot.Accessory:
                    return m_AccessorySlot;
                //case EquipmentItem.EquipmentSlot.Skill:
                    //return m_Skill;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Equip the given item for the given user. This won't check about requirement, this should be done by the
        /// inventory before calling equip!
        /// </summary>
        /// <param name="item">Which item to equip</param>
        public void Equip(EquipmentItem item)
        {
            Unequip(item.Slot, true);

            OnEquiped?.Invoke(item);

            switch (item.Slot)
            {
                case EquipmentItem.EquipmentSlot.Head:
                {
                    m_HeadSlot = item;
                    m_HeadSlot.EquippedBy(m_Owner);
                }
                    break;
                case EquipmentItem.EquipmentSlot.Torso:
                {
                    m_TorsoSlot = item;
                    m_TorsoSlot.EquippedBy(m_Owner);
                }
                    break;
                case EquipmentItem.EquipmentSlot.Legs:
                {
                    m_LegsSlot = item;
                    m_LegsSlot.EquippedBy(m_Owner);
                }
                    break;
                case EquipmentItem.EquipmentSlot.Feet:
                {
                    m_FeetSlot = item;
                    m_FeetSlot.EquippedBy(m_Owner);
                }
                    break;
                case EquipmentItem.EquipmentSlot.Accessory:
                {
                    m_AccessorySlot = item;
                    m_AccessorySlot.EquippedBy(m_Owner);
                }
                    break;
                //special value for weapon
                case (EquipmentItem.EquipmentSlot)666:
                    _Weapon = item as Weapon;
                    //武器鑑定值為0時(未鑑定), 複製為新武器並鑑定
                    if (_Weapon.Identify == 0)
                    {
                        //string ss = JsonUtility.ToJson(item);
                        //Debug.Log(ss);
                        //var w = JsonUtility.FromJson<EquipmentItem>(ss);
                        //_Weapon = w as Weapon;

                        //string s = Weapon.name + UnityEngine.Random.Range(0, 10000);
                        //AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(Weapon), "Assets/InGameItem/" + s + ".asset");
                        //var o = AssetDatabase.LoadAssetAtPath("Assets/InGameItem/" + s + ".asset", typeof(Weapon)) as Weapon;

                        //Weapon = o;
                        _Weapon = _Weapon.Clone();
                        Debug.Log(_Weapon.name);
                        

                        _Weapon.Identify = 1;
                    }
                    _Weapon.EquippedBy(m_Owner);
                    break;
                /*case (EquipmentItem.EquipmentSlot)777:
                    Skill = item as Skill;
                    Debug.Log("安裝技能");
                    Skill.EquippedBy(m_Owner);
                    break;*/
                default:
                    break;
            }
        }

        /// <summary>
        /// Unequip the item in the given slot. isReplacement is used to tell the system if this unequip was called
        /// because we equipped something new in that slot or just unequip to empty slot. This is for the weapon : the
        /// weapon slot can't be empty, so if this is not a replacement, this will auto-requip the base weapon.
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="isReplacement"></param>
        public void Unequip(EquipmentItem.EquipmentSlot slot, bool isReplacement = false)
        {
            switch (slot)
            {
                case EquipmentItem.EquipmentSlot.Head:
                    if (m_HeadSlot != null)
                    {
                        m_HeadSlot.UnequippedBy(m_Owner);
                        m_Owner.Inventory.AddItem(m_HeadSlot);
                        OnUnequip?.Invoke(m_HeadSlot);
                        m_HeadSlot = null;
                    }
                    break;
                case EquipmentItem.EquipmentSlot.Torso:
                    if (m_TorsoSlot != null)
                    {
                        m_TorsoSlot.UnequippedBy(m_Owner);
                        m_Owner.Inventory.AddItem(m_TorsoSlot);
                        OnUnequip?.Invoke(m_TorsoSlot);
                        m_TorsoSlot = null;
                    }
                    break;
                case EquipmentItem.EquipmentSlot.Legs:
                    if (m_LegsSlot != null)
                    {
                        m_LegsSlot.UnequippedBy(m_Owner);
                        m_Owner.Inventory.AddItem(m_LegsSlot);
                        OnUnequip?.Invoke(m_LegsSlot);
                        m_LegsSlot = null;
                    }
                    break;
                case EquipmentItem.EquipmentSlot.Feet:
                    if (m_FeetSlot != null)
                    {
                        m_FeetSlot.UnequippedBy(m_Owner);
                        m_Owner.Inventory.AddItem(m_FeetSlot);
                        OnUnequip?.Invoke(m_FeetSlot);
                        m_FeetSlot = null;
                    }
                    break;
                case EquipmentItem.EquipmentSlot.Accessory:
                    if (m_AccessorySlot != null)
                    {
                        m_AccessorySlot.UnequippedBy(m_Owner);
                        m_Owner.Inventory.AddItem(m_AccessorySlot);
                        OnUnequip?.Invoke(m_AccessorySlot);
                        m_AccessorySlot = null;
                    }
                    break;
                case (EquipmentItem.EquipmentSlot)666:
                    if (_Weapon != null && 
                        (_Weapon != m_DefaultWeapon || isReplacement)) // the only way to unequip the default weapon is through replacing it
                    {
                        _Weapon.UnequippedBy(m_Owner);
                    
                        //the default weapon does not go back to the inventory
                        if(_Weapon != m_DefaultWeapon)
                            m_Owner.Inventory.AddItem(_Weapon);
                    
                        OnUnequip?.Invoke(_Weapon);
                        _Weapon = null;
                    
                        //reequip the default weapon if this is not an unequip to equip a new one
                        if(!isReplacement)
                            Equip(m_DefaultWeapon);
                    }
                    break;
                /*case (EquipmentItem.EquipmentSlot)777:
                    if (Skill != null &&
                        (Skill != m_DefaultSkill || isReplacement)) // the only way to unequip the default weapon is through replacing it
                    {
                        Skill.UnequippedBy(m_Owner);

                        //the default weapon does not go back to the inventory
                        if (Skill != m_DefaultSkill)
                            m_Owner.Inventory.AddItem(Skill);

                        OnUnequip?.Invoke(Skill);
                        Skill = null;

                        //reequip the default weapon if this is not an unequip to equip a new one
                        if (!isReplacement)
                            Equip(m_DefaultSkill);
                    }
                    break;*/
                /*case EquipmentItem.EquipmentSlot.Skill:
                    if (m_AccessorySlot != null)
                    {
                        m_AccessorySlot.UnequippedBy(m_Owner);
                        m_Owner.Inventory.AddItem(m_Skill);
                        OnUnequip?.Invoke(m_Skill);
                        m_AccessorySlot = null;
                    }
                    break;*/
                default:
                    break;
            }
        }
    }
}