using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public override void Interact()
    {
        base.Interact();
        PickUp();
    }
    public Item item;
   
    void PickUp()
    {
        Debug.Log("Pick up "+item.name);
        bool wasPickedUp = Inventory.instance.Add(item);

        if (wasPickedUp)
        {
        
            Destroy(gameObject);
        }
        
            

    }

}
