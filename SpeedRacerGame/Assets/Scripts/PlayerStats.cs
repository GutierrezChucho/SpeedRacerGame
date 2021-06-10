using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Racer Settings")]
    public int playerID;
    public Color playerColour;
    public MeshRenderer mesh;

    [Header("Race Stats")]
    public int currentLap = 0;
    public List<float> lapTimes;
    public bool finished;
    public float finalTime;
    public int currentPosition;

    [Header("Item")]
    public Item heldItem;

    // Start is called before the first frame update
    private void OnValidate()
    {
        mesh.sharedMaterial.color = playerColour;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LapLine")
        {
            print(Vector3.Distance(transform.position, other.transform.position));
            Lap();
        }
    }

    public void Lap()
    {
        if(!finished)
        {
            if (currentLap != 0)
            {
                lapTimes.Add(Time.time - lapTimes[currentLap - 1]);
            }
            else
            {
                lapTimes.Add(Time.time);
            }
            currentLap++;
        }

        finished = (currentLap == GameManager.Instance.maxLaps) ? true : false;
        if(finished && finalTime < 0.1f)
        {
            finalTime = Time.time;
        }
    }
}
