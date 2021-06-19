using System.Collections.Generic;
using UnityEngine;

namespace ProjectD 
{
    /// <summary>
    /// This handles the inventory of our character. The inventory has a maximum of 32 slot, each slot can hold one
    /// TYPE of object, but those can be stacked without limit (e.g. 1 slot used by health potions, but contains 20
    /// health potions)
    /// </summary>
    public class InventorySystem
    {
      
        /// <summary>
        /// One entry in the inventory. Hold the type of Item and how many there is in that slot.
        /// </summary>
        public class InventoryEntry
        {
            public int Count;
            public Item Item;
        }

        private List<Item> _itemDELs= new List<Item>();


        //Only 30 slots in inventory
        public InventoryEntry[] Entries = new InventoryEntry[30];

        CharacterData m_Owner;
        
        public void Init(CharacterData owner)
        {
            m_Owner = owner;
        }
        
        /// <summary>
        /// Add an item to the inventory. This will look if this item already exist in one of the slot and increment the
        /// stack counter there instead of using another slot.
        /// </summary>
        /// <param name="item">The item to add to the inventory</param>
        public void AddItem(Item item)
        {
            
            bool found = false;
            int firstEmpty = -1;
            for (int i = 0; i < 30; ++i)
            {
                if (Entries[i] == null)
                {
                    
                    if (firstEmpty == -1)
                        firstEmpty = i;
                }
                else if (Entries[i].Item == item)
                {
                    
                    Entries[i].Count += 1;
                    found = true;
                }
            }

            if (!found && firstEmpty != -1)
            {
                
               
                InventoryEntry entry = new InventoryEntry();
                entry.Item = item;
                entry.Count = 1;
                
                
                Entries[firstEmpty] = entry;
                
            }
        }

        /// <summary>
        /// This will *try* to use the item. If the item return true when used, this will decrement the stack count and
        /// if the stack count reach 0 this will free the slot. If it return false, it will just ignore that call.
        /// (e.g. a potion will return false if the user is at full health, not consuming the potion in that case)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool UseItem(InventoryEntry item)
        {
            //true mean it get consumed and so would be removed from inventory.
            //(note "consumed" is a loose sense here, e.g. armor get consumed to be removed from inventory and added to
            //equipement by their subclass, and de-equiping will re-add the equipement to the inventory 
            if (item.Item.UsedBy(m_Owner))
            {
                item.Count -= 1;

                if (item.Count <= 0)
                {
                    //maybe store the index in the InventoryEntry to avoid having to find it again here
                    for (int i = 0; i < 30; ++i)
                    {
                        if (Entries[i] == item)
                        {
                            Entries[i] = null;
                            break;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        //quality是物品稀有度  目前沒有分  都是0
        public void EquipmentCombine(EquipmentItem[] itemCombined)
        {
            //刪除三個同品質  
            int itemCount = 0;
            int qualityCalculate = 0;
            //List<InventoryEntry> itemDEL = new List<InventoryEntry>();
            int[] Array = new int[3];
          
        
           

           


            for(int j = 0; j < 30; j++)
            {
                
                if (itemCount<3)
                {
                   
                    //Debug.Log("檢查點3" + Entries[j].Item.name+"  "+Entries[j].Item.itemQuality );
                  
                    if (Entries[j] != null /*&& Entries[j].Item.itemQuality == quality*/)
                    {

                        Debug.Log("檢查點4");
                        //itemDEL.Add(Entries[j]);
                        Array[itemCount] = j;
                        //AddToList(itemDEL.Item);
                        //UseItem(itemDEL);
                        
                        if ((Entries[j].Item as Weapon).Modifier.Stats.itemQuality == 0)
                            qualityCalculate += Random.Range(0, 15);
                        else if ((Entries[j].Item as Weapon).Modifier.Stats.itemQuality == 1)
                            qualityCalculate += Random.Range(0, 30);
                        else if ((Entries[j].Item as Weapon).Modifier.Stats.itemQuality == 2)
                            qualityCalculate += Random.Range(31, 50);

                        //Entries[j] = null;
                        itemCount++;
                        
                     

                    }
                    if (itemCount == 3 )
                    {
                        //Debug.Log("物品數量不足");
                        //break;
                        /*UnityEngine.Object.Destroy(itemDEL.IndexOf(InventoryEntry));
                        itemDEL[1] = null;
                        itemDEL[2] = null;
                        itemDEL.Clear();*/
                        Entries[Array[0]] = null;
                        Entries[Array[1]] = null;
                        Entries[Array[2]] = null;

                        Weapon itemCombine;
                        itemCombine = itemCombined[Random.Range(0, itemCombined.Length)].Clone() as Weapon;
                        itemCombine.ItemName = itemCombine.name;
                        itemCombine.Modifier.Stats.randomvalue = true;

                        if (qualityCalculate > 30 && qualityCalculate <= 80)
                            itemCombine.Modifier.Stats.itemQuality = 1;
                        else if (qualityCalculate > 80)
                            itemCombine.Modifier.Stats.itemQuality = 2;

                        Debug.Log("合成計算:" + qualityCalculate);
                        Debug.Log("合成武器:" + itemCombine.name + ", 稀有度: " + itemCombine.Modifier.Stats.itemQuality);
                        AddItem(itemCombine);
                        itemCount = 0;
                    }

                }             
            }

            //增加一個高1品質五品  (目前沒有分  增加同一品質物品
            //從刪掉的表格中隨機找一個物品生成

            /*if(itemCount == 3) 
            {
                Weapon itemCombine;
                itemCombine = itemCombined[Random.Range(0, itemCombined.Length)].Clone() as Weapon;
                itemCombine.ItemName = itemCombine.name;
                itemCombine.Modifier.Stats.randomvalue = true;

                if (qualityCalculate > 3 && qualityCalculate <= 20)
                    itemCombine.Modifier.Stats.itemQuality = 1;
                else if (qualityCalculate > 20)
                    itemCombine.Modifier.Stats.itemQuality = 2;

                Debug.Log("合成計算:" + qualityCalculate);
                Debug.Log("合成武器:" + itemCombine.name + ", 稀有度: "+itemCombine.Modifier.Stats.itemQuality);
                AddItem(itemCombine);
                itemCount = 0;
            }*/
           







        }

      
        //合成刪掉的物品的表格
        public void AddToList(Item item)
        {
            _itemDELs.Add(item);
        }
    }
    
}