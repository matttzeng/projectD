using System;
using System.Collections;
using System.Collections.Generic;
using ProjectD;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ProjectDInternal 
{
    public class ItemEntryUI : MonoBehaviour, /*IPointerClickHandler,*/ IPointerEnterHandler, IPointerExitHandler,
     /*IBeginDragHandler, IDragHandler,*/ IEndDragHandler, IPointerDownHandler, IPointerUpHandler
  
    {    
        public Image IconeImage;
        public Image OutlineColor;
        public Text ItemCount;
        public Button identifyButton;
        public int InventoryEntry { get; set; } = -1;
        public EquipmentItem EquipmentItem { get; private set; }
    
        public InventoryUI Owner { get; set; }
        public int Index { get; set; }
        public float downTime;
        public float upTime;

        /*public void OnPointerClick(PointerEventData eventData)
        {
            //if(identifyButton.onClick)
            if (eventData.clickCount % 1 == 0)
            {
                if (InventoryEntry != -1)
                {
                    if (Owner.Character.Inventory.Entries[InventoryEntry] != null)
                        Owner.ObjectDoubleClicked(Owner.Character.Inventory.Entries[InventoryEntry]);
                }
                else
                {
                    Owner.EquipmentDoubleClicked(EquipmentItem);
                }
            }
        }*/

        public void OnPointerDown(PointerEventData eventData)
        {
            downTime = Time.unscaledTime;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            upTime = Time.unscaledTime;
            //Debug.Log(upTime - downTime);
            //if(identifyButton.onClick)
            //if (eventData.clickCount % 1 == 0)
            if (upTime - downTime < 0.5f)
            {
                if (InventoryEntry != -1)
                {
                    if (Owner.Character.Inventory.Entries[InventoryEntry] != null)
                        Owner.ObjectDoubleClicked(Owner.Character.Inventory.Entries[InventoryEntry]);
                }
                else
                {
                    Owner.EquipmentDoubleClicked(EquipmentItem);
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Owner.ObjectHoveredEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Owner.ObjectHoverExited(this);
        }

        public void UpdateEntry()
        {
            var entry = Owner.Character.Inventory.Entries[InventoryEntry];
            bool isEnabled = entry != null;
            
            gameObject.SetActive(isEnabled);
        
            if (isEnabled)
            {
                // Debug.Log("顯示道具圖片");
                IconeImage.sprite = entry.Item.ItemSprite;

                if ((entry.Item as Weapon).Modifier.Stats.itemQuality == 0)
                {
                    OutlineColor.color = Color.white;
                }
                if ((entry.Item as Weapon).Modifier.Stats.itemQuality == 1)
                {
                    OutlineColor.color =new Color(0,0.8f,0);
                }
                if ((entry.Item as Weapon).Modifier.Stats.itemQuality == 2)
                {
                    OutlineColor.color = new Color(0, 0.4f, 1.0f);
                }
                if ((entry.Item as Weapon).Modifier.Stats.itemQuality == 3)
                {
                    OutlineColor.color = Color.yellow;
                }
                if ((entry.Item as Weapon).Modifier.Stats.itemQuality == 4)
                {
                    OutlineColor.color = new Color(0.7f, 0.2f, 0.8f);
                }
                if ((entry.Item as Weapon).Modifier.Stats.itemQuality == 5)
                {
                    OutlineColor.color = new Color(0.9f, 0.6f, 0);
                }
                if ((entry.Item as Weapon).Modifier.Stats.itemQuality == 6)
                {
                    OutlineColor.color = new Color(0.8f, 0.2f, 0.2f);
                }





                if (entry.Count > 1)
                {
                    ItemCount.gameObject.SetActive(true);
                    ItemCount.text = entry.Count.ToString();
                }
                else
                {
                    ItemCount.gameObject.SetActive(false);
                }
            }
        }

        public void SetupEquipment(EquipmentItem itm)
        {
            EquipmentItem = itm;

            enabled = itm != null;
            IconeImage.enabled = enabled;
            if (enabled)
                IconeImage.sprite = itm.ItemSprite;
           
            if (itm.Slot == (EquipmentItem.EquipmentSlot)666)
            {
                if ((EquipmentItem as Weapon).Modifier.Stats.itemQuality == 0)
                {
                    OutlineColor.color = Color.white;
                }
                if ((EquipmentItem as Weapon).Modifier.Stats.itemQuality == 1)
                {
                    OutlineColor.color = new Color(0, 0.8f, 0);
                }
                if ((EquipmentItem as Weapon).Modifier.Stats.itemQuality == 2)
                {
                    OutlineColor.color =new Color(0, 0.4f, 1.0f);
                }
                if ((EquipmentItem as Weapon).Modifier.Stats.itemQuality == 3)
                {
                    OutlineColor.color = Color.yellow;
                }
                if ((EquipmentItem as Weapon).Modifier.Stats.itemQuality == 4)
                {
                    OutlineColor.color =new Color(0.7f, 0.2f, 0.8f);
                }
                if ((EquipmentItem as Weapon).Modifier.Stats.itemQuality == 5)
                {
                    OutlineColor.color = new Color(0.9f, 0.6f, 0);
                }
                if ((EquipmentItem as Weapon).Modifier.Stats.itemQuality == 6)
                {
                    OutlineColor.color = new Color(0.8f, 0.2f, 0.2f);
                }
            }
        }
        /*
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(EquipmentItem != null)
                return;
        
            Owner.CurrentlyDragged = new InventoryUI.DragData();
            Owner.CurrentlyDragged.DraggedEntry = this;
            Owner.CurrentlyDragged.OriginalParent = (RectTransform)transform.parent;
        
            transform.SetParent(Owner.DragCanvas.transform, true);
        }
        */
    
        /*
        public void OnDrag(PointerEventData eventData)
        {
            if(EquipmentItem != null)
                return;
        
            transform.localPosition = transform.localPosition + UnscaleEventDelta(eventData.delta);
        }
    
        */
        Vector3 UnscaleEventDelta(Vector3 vec)
        {
            Vector2 referenceResolution = Owner.DragCanvasScaler.referenceResolution;
            Vector2 currentResolution = new Vector2(Screen.width, Screen.height);
 
            float widthRatio = currentResolution.x / referenceResolution.x;
            float heightRatio = currentResolution.y / referenceResolution.y;
            float ratio = Mathf.Lerp(widthRatio, heightRatio,  Owner.DragCanvasScaler.matchWidthOrHeight);
 
            return vec / ratio;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(EquipmentItem != null)
                return;
        
           // Owner.HandledDroppedEntry(eventData.position);
        
            RectTransform t = transform as RectTransform;
        
            transform.SetParent(Owner.CurrentlyDragged.OriginalParent, true);

            t.offsetMax = -Vector2.one * 4;
            t.offsetMin = Vector2.one * 4;
        }
    }
}