using System.Collections.Generic;
using ProjectD;
using UnityEngine;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectD
{
    /// <summary>
    /// Special Item than can be equipped. They can have a minimum stats value needed to equip them, and you can add
    /// EquippedEffect which will be executed when the object is equipped and unequipped, allowing to code special
    /// behaviour when the player equipped those object, like raising stats.
    /// </summary>
    [CreateAssetMenu(fileName = "EquipmentItem", menuName = "Beginner Code/Equipment Item", order = -999)]
    public class EquipmentItem : Item
    {
        public enum EquipmentSlot
        {
            Head,
            Torso,
            Legs,
            Feet,
            Accessory,
            Skill
        }
    
        public abstract class EquippedEffect : ScriptableObject
        {
            public string Description;
            //return true if could be used, false otherwise.
            public abstract void Equipped(CharacterData user);
            public abstract void Removed(CharacterData user);
  
            public virtual string GetDescription()
            {
                return Description;
            }
        }

        public EquipmentSlot Slot;
        public StatSystem.StatModifier Modifier;

        [Header("Minimum Stats")]
        public int MinimumStrength;
        public int MinimumAgility;
        public int MinimumDefense;

        public List<EquippedEffect> EquippedEffects;
    
        public override bool UsedBy(CharacterData user)
        {
            var userStat = user.Stats.stats;

            if (userStat.agility < MinimumAgility
                || userStat.strength < MinimumStrength
                || userStat.defense < MinimumDefense)
            {
                return false;
            }

            user.Equipment.Equip(this);
        
            return true;
        }

        //武器的desc
        public override string GetDescription1()
        {
            string desc = base.GetDescription1() + "\n";
            
            //foreach (var effect in EquippedEffects)
                //desc += "\n" + effect.GetDescription1();
            
            bool requireStrength = MinimumStrength > 0;
            bool requireDefense = MinimumDefense > 0;
            bool requireAgility = MinimumAgility > 0;

            if (requireStrength || requireAgility || requireDefense)
            {
                desc += "\nRequire : \n";

                if (requireStrength)
                    desc += $"Strength : {MinimumStrength}";

                if (requireAgility)
                {
                    if (requireStrength) desc += " & ";
                    desc += $"Defense : {MinimumDefense}";
                }
            
                if (requireDefense)
                {
                    if (requireStrength || requireAgility) desc += " & ";
                    desc += $"Agility : {MinimumAgility}";
                }
            }

            //string desc = base.GetDescription1() + "\n";

            string unit = Modifier.ModifierMode == StatSystem.StatModifier.Mode.Percentage ? "%" : "";

            if (Modifier.Stats.itemQuality == 0)
                desc += $"Quality : White\n";
            if (Modifier.Stats.itemQuality == 1)
                desc += $"Quality : Green\n";
            if (Modifier.Stats.itemQuality == 2)
                desc += $"Quality : Blue\n";
            if (Modifier.Stats.itemQuality == 3)
                desc += $"Quality : Yellow\n";
            if (Modifier.Stats.itemQuality == 4)
                desc += $"Quality : Purple\n";
            if (Modifier.Stats.itemQuality == 5)
                desc += $"Quality : Orange\n";
            if (Modifier.Stats.itemQuality == 6)
                desc += $"Quality : Red\n";
            /*
            
            if (Modifier.Stats.strength != 0)
                desc += $"Str : {Modifier.Stats.strength:+0;-#}{unit}\n"; //format specifier to force the + sign to appear
            if (Modifier.Stats.agility != 0)
                desc += $"Agi : {Modifier.Stats.agility:+0;-#}{unit}\n";
            */
            if (Modifier.Stats.defense != 0)
                desc += $"Def : {Modifier.Stats.defense:+0;-#}{unit}\n";
            if (Modifier.Stats.health != 0)
                desc += $"HP : {Modifier.Stats.health:+0;-#}{unit}\n";
            if (Modifier.Stats.attack != 0)
                desc += $"Atk : {Modifier.Stats.attack:+0;-#}{unit}\n";
            if (Modifier.Stats.skill != 0)
                desc += $"Skill : {Modifier.Stats.skill:+0;-#}{unit}\n";
            if (Modifier.Stats.skillRange != 0)
                desc += $"SkillRange : {Modifier.Stats.skillRange:+0;-#}{unit}\n";
            if (Modifier.Stats.skillSpeed != 0)
                desc += $"SkillSpeed : {Modifier.Stats.skillSpeed:+0;-#}{unit}\n";
            if (Modifier.Stats.moveSpeed != 0)

                desc += $"MoveSpeed : {Modifier.Stats.moveSpeed:+0;-#}{unit}\n";
            if (Modifier.Stats.attackSpeed != 0)
                desc += $"AtkSpeed : {Modifier.Stats.attackSpeed:+0;-#}{unit}\n";
            if (Modifier.Stats.attackRange != 0)
                desc += $"AtkRange : {Modifier.Stats.attackRange:+0;-#}{unit}\n";
          
            if (Modifier.Stats.crit != 0)
                desc += $"Crit : {Modifier.Stats.crit:+0;-#}{unit}\n";



            //return desc;

            return desc;
        }


        public void EquippedBy(CharacterData user)
        {
            /*foreach (var effect in EquippedEffects)
            {
                effect.Equipped(user);
            }*/
            //QualityCalculate(Modifier);
            //Debug.Log(itemQuality);
            //Modifier.Stats.itemQuality = itemQuality;
            //Debug.Log(Modifier.Stats.itemQuality);
            user.Stats.AddModifier(Modifier);
            //Debug.Log(Modifier.Stats.itemQuality);
        }
    
        public void UnequippedBy(CharacterData user)
        {
            /*foreach (var effect in EquippedEffects)
                effect.Removed(user);*/
            user.Stats.RemoveModifier(Modifier);
        }

        /*public void QualatyCalculate(StatSystem.StatModifier Modifier)
        {
            float i = (itemQuality + 1) / 3;
            Modifier.Stats.health = (int)Math.Floor(Modifier.Stats.health / i);
            Modifier.Stats.strength = (int)Math.Floor(Modifier.Stats.strength / i);
            Modifier.Stats.defense = (int)Math.Floor(Modifier.Stats.defense / i);
            Modifier.Stats.agility = (int)Math.Floor(Modifier.Stats.agility / i);
            Modifier.Stats.attack = (int)Math.Floor(Modifier.Stats.attack / i);
            Modifier.Stats.skill = (int)Math.Floor(Modifier.Stats.skill / i);
            Modifier.Stats.moveSpeed = (int)Math.Floor(Modifier.Stats.moveSpeed / i);
            Modifier.Stats.attackSpeed = (int)Math.Floor(Modifier.Stats.attackSpeed / i);
            Modifier.Stats.attackRange = (int)Math.Floor(Modifier.Stats.attackRange / i);
            Modifier.Stats.crit = (int)Math.Floor(Modifier.Stats.crit / i);
            Modifier.Stats.skillRange = (int)Math.Floor(Modifier.Stats.skillRange / i);
            Modifier.Stats.skillSpeed = (int)Math.Floor(Modifier.Stats.skillSpeed / i);
        }*/
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EquipmentItem))]
public class EquipmentItemEditor : Editor
{
    EquipmentItem m_Target;
    
    ItemEditor m_ItemEditor;

    List<string> m_AvailableEquipEffectType;
    SerializedProperty m_EquippedEffectListProperty;

    SerializedProperty m_SlotProperty;
    
    SerializedProperty m_MinimumStrengthProperty;
    SerializedProperty m_MinimumAgilityProperty;
    SerializedProperty m_MinimumDefenseProperty;

    SerializedProperty m_Modifier;

    void OnEnable()
    {
        m_Target = target as EquipmentItem;
        m_EquippedEffectListProperty = serializedObject.FindProperty(nameof(EquipmentItem.EquippedEffects));

        m_SlotProperty = serializedObject.FindProperty(nameof(EquipmentItem.Slot));
        
        m_MinimumStrengthProperty = serializedObject.FindProperty(nameof(EquipmentItem.MinimumStrength));
        m_MinimumAgilityProperty = serializedObject.FindProperty(nameof(EquipmentItem.MinimumAgility));
        m_MinimumDefenseProperty = serializedObject.FindProperty(nameof(EquipmentItem.MinimumDefense));

        m_Modifier = serializedObject.FindProperty(nameof(EquipmentItem.Modifier));
        
        m_ItemEditor = new ItemEditor();
        m_ItemEditor.Init(serializedObject);

        var lookup = typeof(EquipmentItem.EquippedEffect);
        m_AvailableEquipEffectType = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
            .Select(type => type.Name)
            .ToList();
    }

    public override void OnInspectorGUI()
    {
        m_ItemEditor.GUI();

        EditorGUILayout.PropertyField(m_SlotProperty);
        
        EditorGUILayout.PropertyField(m_MinimumStrengthProperty);
        EditorGUILayout.PropertyField(m_MinimumAgilityProperty);
        EditorGUILayout.PropertyField(m_MinimumDefenseProperty);

        EditorGUILayout.PropertyField(m_Modifier);
        
        int choice = EditorGUILayout.Popup("Add new Effect", -1, m_AvailableEquipEffectType.ToArray());

        if (choice != -1)
        {
            var newInstance = ScriptableObject.CreateInstance(m_AvailableEquipEffectType[choice]);
            
            AssetDatabase.AddObjectToAsset(newInstance, target);
            
            m_EquippedEffectListProperty.InsertArrayElementAtIndex(m_EquippedEffectListProperty.arraySize);
            m_EquippedEffectListProperty.GetArrayElementAtIndex(m_EquippedEffectListProperty.arraySize - 1).objectReferenceValue = newInstance;
        }

        Editor ed = null;
        int toDelete = -1;
        for (int i = 0; i < m_EquippedEffectListProperty.arraySize; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            var item = m_EquippedEffectListProperty.GetArrayElementAtIndex(i);           
            SerializedObject obj = new SerializedObject(item.objectReferenceValue);

            Editor.CreateCachedEditor(item.objectReferenceValue, null, ref ed);
            
            ed.OnInspectorGUI();
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("-", GUILayout.Width(32)))
            {
                toDelete = i;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (toDelete != -1)
        {
            var item = m_EquippedEffectListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
            DestroyImmediate(item, true);
            
            //need to do it twice, first time just nullify the entry, second actually remove it.
            m_EquippedEffectListProperty.DeleteArrayElementAtIndex(toDelete);
            m_EquippedEffectListProperty.DeleteArrayElementAtIndex(toDelete);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif