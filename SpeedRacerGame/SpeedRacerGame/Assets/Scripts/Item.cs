using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Assets/New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int itemID;
    [TextArea]
    public string itemDescription;
    public Sprite sprite;
    [Tooltip("If left empty, item is assumed to affect player directly")]
    public GameObject prefab;

    public void UseItem(GameObject player)
    {
        //Separate use cases depending on whether it spawns prefab or not
        if(prefab == null)
        {
            player.GetComponent<PlayerControls>().SetSpeedModifer(1.2f);
        }
        else
        {
            Instantiate(prefab, player.GetComponent<PlayerControls>().itemSpawn.transform.position, Quaternion.identity);
        }
    }

}
