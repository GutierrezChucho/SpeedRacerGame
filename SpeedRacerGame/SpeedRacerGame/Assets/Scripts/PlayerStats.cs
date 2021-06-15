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
    public GameObject lastCheckpoint;

    [Header("Item")]
    public Item heldItem;

    // Changes take place in editor
    private void OnValidate()
    {
        mesh.sharedMaterial.color = playerColour;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "LapLine":
                //Don't trigger a lap if no checkpoints have been crossed (i.e start of game)
                if (lastCheckpoint != null)
                {
                    //Trigger a lap if the last checkpoint passed was the penultimate checkpoint
                    if (other.gameObject == GameManager.Instance.checkpoints[0] && lastCheckpoint == GameManager.Instance.checkpoints[GameManager.Instance.checkpoints.Length - 1])
                    {
                        //Begin a new lap
                        Lap();
                    }
                }
                lastCheckpoint = other.gameObject;
                break;
            case "ItemBox":
                if (heldItem == null)
                {
                    heldItem = other.GetComponent<ItemBox>().item;
                    Destroy(other.gameObject);
                }
                break;
            default:
                break;
        }
    }

    //To be called whenever the player crosses a lap line
    public void Lap()
    {
        if(!finished)
        {
            if (currentLap != 0)
            {
                lapTimes.Add(GameManager.Instance.mapTimer - lapTimes[currentLap - 1]);
            }
            else
            {
                lapTimes.Add(GameManager.Instance.mapTimer);
            }
            currentLap++;
        }

        finished = (currentLap == GameManager.Instance.maxLaps) ? true : false;
        if(finished && finalTime < 0.1f)
        {
            finalTime = Time.time;
        }
    }

    //Returns the time in min:sec:ms format
    public string GetLapTimeInFormat(float time)
    {
        float minutes, seconds, milliseconds;
        minutes = Mathf.Floor(time / 60);
        seconds = Mathf.FloorToInt(time % 60);
        milliseconds = Mathf.FloorToInt((time * 1000) % 1000);

        string x = $"{minutes}:{seconds}:{milliseconds}";
        return x;
    }
}
