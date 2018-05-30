using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {


    public Item TestItem;

    public List<Item> Backpack = new List<Item>(0);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Z)) AddItem(TestItem);
        if (Input.GetKeyDown(KeyCode.X)) RemoveItem(TestItem);
	}

    #region Inventory Management

    public void AddItem(Item ItemToAdd)
    {
        if (Backpack.Count < 20)
            Backpack.Add(ItemToAdd);
        else
            print("Not enough space.");
    }

    public void RemoveItem(Item ItemToRemove)
    {
        int IndexToRemove = GetIndexOfItem(ItemToRemove);
        if (IndexToRemove != -1)
        {
            Backpack.RemoveAt(IndexToRemove);
        }
    }

    public int GetIndexOfItem(Item ItemToCheck)
    {
        for (int i = 0; i < Backpack.Count; i++)
        {
            if (Backpack[i] == ItemToCheck) return i;
        }

        return -1;
    }

    public int GetIndexOfItem(string ItemToCheck)
    {
        for (int i = 0; i < Backpack.Count; i++)
        {
            if (Backpack[i].ItemName == ItemToCheck) return i;
        }

        return -1;
    }

    #endregion

}


