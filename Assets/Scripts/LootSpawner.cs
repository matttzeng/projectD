using System;
using System.Collections;
using System.Collections.Generic;
using ProjectD;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectD
{
    /// <summary>
    /// This class handle creating loot. It got a list of events and each events have a list of items with associated
    /// weight. When the spawn is triggered through the SpawnLoot function, it will spawn one item per events, with the
    /// item being picked randomly per event 
    /// </summary>
    public class LootSpawner : MonoBehaviour
    {
        


        [System.Serializable]

        
        public class SpawnEvent
        {
            public LootEntry[] Entries;
        }
    
        [System.Serializable]
        public class LootEntry
        {
            public int Weight = 1;
            public Item Item;
        }

        class InternalPurcentageEntry
        {
            public LootEntry Entry;
            public float Percentage;
        }
    
        public SpawnEvent[] Events;
        public AudioClip SpawnedClip;
        public bool dropRandomWeapon = false;


        /// <summary>
        /// Call this to trigger the spawning of the loot. Will spawn one item per event, picking the item randomly
        /// per event using the defined weight. Every call will pick randomly again (but most of the time, the caller
        /// will destroy the LootSpawner too as you spawn loot from something only once)
        /// </summary>
        public void SpawnLoot()
        {
            Vector3 position = transform.position;
            SFXManager.PlaySound(SFXManager.Use.WorldSound, new SFXManager.PlayData()
            {
                Clip = SpawnedClip,
                Position = position
            });

            //we go over all the events.
            if (dropRandomWeapon == false)
            {


                for (int i = 0; i < Events.Length; ++i)
                {
                    SpawnEvent Event = Events[i];

                    //first iterate over all object to make a total weight value.
                    int totalWeight = 0;
                    foreach (var entry in Event.Entries)
                    {
                        totalWeight += entry.Weight;
                    }

                    //if we don't have any weight just exit
                    if (totalWeight == 0)
                        continue;

                    //then go back again on all the object to build a lookup table based on percentage.
                    List<InternalPurcentageEntry> lookupTable = new List<InternalPurcentageEntry>();
                    float previousPercent = 0.0f;
                    foreach (var entry in Event.Entries)
                    {
                        float percent = entry.Weight / (float)totalWeight;
                        InternalPurcentageEntry percentageEntry = new InternalPurcentageEntry();
                        percentageEntry.Entry = entry;
                        percentageEntry.Percentage = previousPercent + percent;

                        previousPercent = percentageEntry.Percentage;

                        lookupTable.Add(percentageEntry);
                    }

                    float rng = Random.value;
                    for (int k = 0; k < lookupTable.Count; ++k)
                    {
                        if (rng <= lookupTable[k].Percentage)
                        {
                           
                           



                                    
                            GameObject obj = new GameObject(lookupTable[k].Entry.Item.ItemName);

                            Debug.Log("生成物件:" + obj.name);
                            var l = obj.AddComponent<Loot>();

                            //生成一個新的weapon.asset
                            //l.Item = WeaponEditor.CreateWeapon();
                            //string weaponName = lookupTable[k].Entry.Item.name + UnityEngine.Random.Range(0, 10000);


                            

                            //WeaponEditor.CreateWeapon(weaponName);

                            //l.Item的欄位放進這個weapon asset
                            /*
                            l.Item = lookupTable[k].Entry.Item;// AssetDatabase.LoadAssetAtPath("Assets/ItemDataBase/"+weaponName+".asset",WeaponEditor);
                            Debug.Log("創造" + l.Item.name);

                            //先隨機數值
                            var s = ((StatChangeEquipEffect)((Weapon)l.Item).EquippedEffects[0]).Modifier.Stats;
                            s.health =        UnityEngine.Random.Range(0, s.health);
                            s.strength =      UnityEngine.Random.Range(0, s.strength);
                            s.defense =       UnityEngine.Random.Range(0, s.defense);
                            s.agility =       UnityEngine.Random.Range(0, s.agility);
                            s.attack =        UnityEngine.Random.Range(0, s.attack);
                            s.skill =         UnityEngine.Random.Range(0, s.skill);
                            s.moveSpeed =     UnityEngine.Random.Range(0, s.moveSpeed);
                            s.attackSpeed =   UnityEngine.Random.Range(0, s.attackSpeed);
                            s.attackRange =   UnityEngine.Random.Range(0, s.attackRange);
                            s.crit =          UnityEngine.Random.Range(0, s.crit);
                            s.skillRange =    UnityEngine.Random.Range(0, s.skillRange);
                            s.skillSpeed =    UnityEngine.Random.Range(0, s.skillSpeed);
                            
                            */
                            l.Item = lookupTable[k].Entry.Item;
                            //掉出這個weapon asset
                            l.Spawn(position);

                                

                            








                                
                            //GameObject obj = Instantiate(lookupTable[k].Entry.Item.WorldObjectPrefab);




                                
                            break;


                            

                            
                        }
                    }
                }
            }
            else
            {
                var weaponGenerator = GetComponent<RandomWeaponGenerator>();
                
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(LootSpawner))]
public class LootSpawnerEditor : Editor
{
    SerializedProperty m_SpawnSoundProp;
    SerializedProperty m_SpawnEventProp;
    SerializedProperty m_dropRandomWeapon;
    

    bool[] m_FoldoutInfos;

    int toDelete = -1;

    void OnEnable()
    {
        m_SpawnSoundProp = serializedObject.FindProperty("SpawnedClip");
        m_SpawnEventProp = serializedObject.FindProperty("Events");
        m_dropRandomWeapon = serializedObject.FindProperty("dropRandomWeapon");
        m_FoldoutInfos = new bool[m_SpawnEventProp.arraySize];
     
        Undo.undoRedoPerformed += RecomputeFoldout;
    }

    void OnDisable()
    {
        Undo.undoRedoPerformed -= RecomputeFoldout;
    }

    void RecomputeFoldout()
    {
        serializedObject.Update();

        var newFoldout = new bool[m_SpawnEventProp.arraySize];
        Array.Copy(m_FoldoutInfos, newFoldout, Mathf.Min(m_FoldoutInfos.Length, newFoldout.Length));
        m_FoldoutInfos = newFoldout;
    }

    public override void OnInspectorGUI()
    {
        
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_dropRandomWeapon);
        EditorGUILayout.PropertyField(m_SpawnSoundProp);
        
        
        for (int i = 0; i < m_SpawnEventProp.arraySize; ++i)
        {
            var i1 = i;
            m_FoldoutInfos[i] = EditorGUILayout.BeginFoldoutHeaderGroup(m_FoldoutInfos[i], $"Slot {i}", null, (rect) => { ShowHeaderContextMenu(rect, i1); });

            if (m_FoldoutInfos[i])
            {
                var entriesArrayProp = m_SpawnEventProp.GetArrayElementAtIndex(i).FindPropertyRelative("Entries");

                int localToDelete = -1;
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Item");
                GUILayout.Label("Weight");
                GUILayout.Space(16);
                EditorGUILayout.EndHorizontal();

                for (int j = 0; j < entriesArrayProp.arraySize; ++j)
                {
                    var entryProp = entriesArrayProp.GetArrayElementAtIndex(j);

                    var itemProp = entryProp.FindPropertyRelative("Item");
                    var weightProp = entryProp.FindPropertyRelative("Weight");

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(itemProp, GUIContent.none);
                    EditorGUILayout.PropertyField(weightProp, GUIContent.none);
                    if (GUILayout.Button("-", GUILayout.Width(16)))
                    {
                        localToDelete = j;
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (localToDelete != -1)
                {
                    entriesArrayProp.DeleteArrayElementAtIndex(localToDelete);
                }

                if (GUILayout.Button("Add New Entry", GUILayout.Width(100)))
                {
                    entriesArrayProp.InsertArrayElementAtIndex(entriesArrayProp.arraySize);
                }
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        if (toDelete != -1)
        {
            m_SpawnEventProp.DeleteArrayElementAtIndex(toDelete);
            ArrayUtility.RemoveAt(ref m_FoldoutInfos, toDelete);
            toDelete = -1;
        }

        if (GUILayout.Button("Add new Slot"))
        {
            m_SpawnEventProp.InsertArrayElementAtIndex(m_SpawnEventProp.arraySize);
            serializedObject.ApplyModifiedProperties();

            //insert will copy the last element, which can lead to having to empty a large spawn event to start new
            //so we manually "empty" the new event
            var newElem = m_SpawnEventProp.GetArrayElementAtIndex(m_SpawnEventProp.arraySize - 1);
            var entries = newElem.FindPropertyRelative("Entries");

            entries.ClearArray();

            ArrayUtility.Add(ref m_FoldoutInfos, false);
        }

        serializedObject.ApplyModifiedProperties();
    }
    
    void ShowHeaderContextMenu(Rect position, int index)
    {
        var menu = new GenericMenu();
        menu.AddItem(new GUIContent("Remove"), false, () => { toDelete = index;});
        menu.DropDown(position);
    }
}

#endif