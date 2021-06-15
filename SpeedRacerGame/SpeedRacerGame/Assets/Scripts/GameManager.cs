using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Map Stats")]
    [Range(1,8)]
    [Tooltip("How many laps must the player do to finish the track")]
    public int maxLaps;
    public float mapTimer;
    public bool raceStarted = false;
    private float timeRaceStarted;

    public PlayerStats[] players;
    public GameObject[] checkpoints;
    public GameObject[] respawnPoints;

    #region Singleton Pattern
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion


    // Start is called before the first frame update
    void Update()
    {
        mapTimer = Time.time - timeRaceStarted;
    }

    //Initiates the start of the race
    public void StartRace()
    {
        if(!raceStarted)
        {
            timeRaceStarted = Time.time;
            raceStarted = true;
        }
    }
}
