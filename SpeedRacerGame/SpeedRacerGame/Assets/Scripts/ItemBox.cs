using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public ItemDatabase itemTable;
    public Item item;

    // Start is called before the first frame update
    void Start()
    {
        //Set item to a random from the table
        int rIndex = Random.Range(0, itemTable.itemList.Count);
        item = itemTable.itemList[rIndex];
    }
}
