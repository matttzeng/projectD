using System.Collections;
using System.Collections.Generic;
using ProjectD;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectD
{
    /// <summary>
    /// Base clase of all items in the game. This is an abstract class and need to be inherited to specify behaviour.
    /// The project offer 3 type of items : UsableItem, Equipment and Weapon
    /// </summary>
    public abstract class Item : ScriptableObject
    {
        public string ItemName;
        public Sprite ItemSprite;
        public string Description;
        public GameObject WorldObjectPrefab;

        /// <summary>
        /// Called by the inventory system when the object is "used" (double clicked)
        /// </summary>
        /// <param name="user">The CharacterDate that used that item</param>
        /// <returns>If it was actually used (allow the inventory to know if it can remove the object or not)</returns>
        public virtual bool UsedBy(CharacterData user)
        {
            return false;
        }

        public virtual string GetDescription()
        {
            return Description;
        }
    }
}

#if UNITY_EDITOR
public class ItemEditor
{
    SerializedObject m_Target;

    SerializedProperty m_NameProperty;
    SerializedProperty m_IconProperty;
    SerializedProperty m_DescriptionProperty;
    SerializedProperty m_WorldObjectPrefabProperty;
    
    public void Init(SerializedObject target)
    {
        m_Target = target;

        m_NameProperty = m_Target.FindProperty(nameof(Item.ItemName));
        m_IconProperty = m_Target.FindProperty(nameof(Item.ItemSprite));
        m_DescriptionProperty = m_Target.FindProperty(nameof(Item.Description));
        m_WorldObjectPrefabProperty = m_Target.FindProperty(nameof(Item.WorldObjectPrefab));
    }

    public void GUI()
    {
        EditorGUILayout.PropertyField(m_IconProperty);
        EditorGUILayout.PropertyField(m_NameProperty);
        EditorGUILayout.PropertyField(m_DescriptionProperty, GUILayout.MinHeight(128));
        EditorGUILayout.PropertyField(m_WorldObjectPrefabProperty);
    }
}
#endif

/*
namespace ProjectD
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    /// <summary>
    /// Base clase of all items in the game. This is an abstract class and need to be inherited to specify behaviour.
    /// The project offer 3 type of items : UsableItem, Equipment and Weapon
    /// </summary>
    public abstract class Item : ScriptableObject
    {
        public string ItemName;
        public Sprite ItemSprite;
        public string Description;
        public GameObject WorldObjectPrefab;

        /// <summary>
        /// Called by the inventory system when the object is "used" (double clicked)
        /// </summary>
        /// <param name="user">The CharacterDate that used that item</param>
        /// <returns>If it was actually used (allow the inventory to know if it can remove the object or not)</returns>

        new public string name = "New Item";
        public Sprite icon = null;
        public bool isDefaultItem = false;

        public virtual void Use()
        {
            //use the item
            //somthing might happen

            Debug.Log("Using " + name);
        }



        public virtual bool UsedBy(CharacterData user)
        {
            return false;
        }

        public virtual string GetDescription()
        {
            return Description;
        }
    }
}

#if UNITY_EDITOR
public class ItemEditor
{
    SerializedObject m_Target;

    SerializedProperty m_NameProperty;
    SerializedProperty m_IconProperty;
    SerializedProperty m_DescriptionProperty;
    SerializedProperty m_WorldObjectPrefabProperty;

    public void Init(SerializedObject target)
    {
        m_Target = target;

        m_NameProperty = m_Target.FindProperty(nameof(Item.ItemName));
        m_IconProperty = m_Target.FindProperty(nameof(Item.ItemSprite));
        m_DescriptionProperty = m_Target.FindProperty(nameof(Item.Description));
        m_WorldObjectPrefabProperty = m_Target.FindProperty(nameof(Item.WorldObjectPrefab));
    }

    public void GUI()
    {
        EditorGUILayout.PropertyField(m_IconProperty);
        EditorGUILayout.PropertyField(m_NameProperty);
        EditorGUILayout.PropertyField(m_DescriptionProperty, GUILayout.MinHeight(128));
        EditorGUILayout.PropertyField(m_WorldObjectPrefabProperty);
    }
}
#endif
*/