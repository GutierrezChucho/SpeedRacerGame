using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpointer : MonoBehaviour
{
    public Transform Spawnpoint;
   
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Restart")
        {
            transform.position = Spawnpoint.transform.position;
        }
    }
}
