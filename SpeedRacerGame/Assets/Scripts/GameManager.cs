using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Map Stats")]
    [Range(1,8)]
    public int maxLaps;
    public float mapTimer;
    public bool raceStarted = false;
    private float timeRaceStarted;

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

    // Update is called once per frame
    public void StartRace()
    {
        if(!raceStarted)
        {
            timeRaceStarted = Time.time;
            raceStarted = true;
        }
    }

    public string GetTimeInFormat()
    {
        float minutes, seconds, milliseconds;
        minutes = Mathf.Floor(mapTimer / 60);
        seconds = Mathf.RoundToInt(mapTimer % 60);
        //milliseconds = Mathf.RoundToInt((mapTimer * 1000) % 1000);

        string x = $"{minutes}:{seconds}";
        return x;
    }
}
