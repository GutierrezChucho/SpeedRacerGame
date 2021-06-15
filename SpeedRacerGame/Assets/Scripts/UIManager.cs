using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //UI relevant to each player (Laps, Lap Times, Item)
    public class PlayerUI
    {
        private GameObject gameObject;
        PlayerStats player;

        private TextMeshProUGUI lap;
        private TextMeshProUGUI time;
        private Image item;

        //
        public PlayerUI(GameObject g, PlayerStats p)
        {
            gameObject = g;
            player = p;

            time = g.GetComponentsInChildren<TextMeshProUGUI>()[0];
            lap = g.GetComponentsInChildren<TextMeshProUGUI>()[1];
            item = g.GetComponentsInChildren<Image>()[1];
        }

        public void Set()
        {
            time.text = player.lapTimes.Count != 0 ? player.GetLapTimeInFormat(player.lapTimes[player.lapTimes.Count-1]) : "";
            lap.text = $"Lap: {player.currentLap}";

            //Boilerplate for sprites for items. To be tweaked.
            Color ItemSprite = Color.clear;

            if (player.heldItem != null)
            {
                switch (player.heldItem.itemID)
                {
                    default:
                        ItemSprite = Color.clear;
                        break;
                    case 0:
                        ItemSprite = Color.white;
                        break;
                    case 1:
                        ItemSprite = Color.red;
                        break;
                    case 2:
                        ItemSprite = Color.yellow;
                        break;
                }
            }

            item.color = ItemSprite;
        }
    }

    public PlayerUI p1, p2;

    public GameObject[] UI;

    public TextMeshProUGUI mainTimer;

    // Start is called before the first frame update
    void Start()
    {
        p1 = new PlayerUI(UI[0], GameManager.Instance.players[0]);
        p2 = new PlayerUI(UI[1], GameManager.Instance.players[1]);
    }

    // Update is called once per frame
    void Update()
    {
        mainTimer.text = GetTimeInFormat(GameManager.Instance.mapTimer);
        p1.Set();
        p2.Set();
    }

    public string GetTimeInFormat(float time)
    {
        float minutes, seconds, milliseconds;
        minutes = Mathf.Floor(time / 60);
        seconds = Mathf.FloorToInt(time % 60);
        milliseconds = Mathf.FloorToInt((time * 1000) % 1000);

        string x = $"{minutes}:{seconds}:{milliseconds}";
        return x;
    }
}
