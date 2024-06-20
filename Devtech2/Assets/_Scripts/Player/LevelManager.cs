using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameObject player;

    [SerializeField] private int tilesAvaibleToPaint;
    [SerializeField] private int tilesNeededToWin;

    [SerializeField] private GameObject[] enemies;

    [SerializeField] private float enemyTimer = 10f;

    [SerializeField] private TextMeshProUGUI _objectiveTilesText;
    [SerializeField] private TextMeshProUGUI _currentTilesText;
    [SerializeField] private TextMeshProUGUI _avaibleTilesText;
    [SerializeField] private TextMeshProUGUI _timerUI;

    private bool playerTurn = true;
    private bool endedGame = false;
    private float timer = 0;

    private void Awake()
    {
        player = GameObject.Find("Player");
        player.GetComponent<TileColoring>().SetMaxColors(tilesAvaibleToPaint);
        _objectiveTilesText.text = "OBJECTIVE: " + tilesNeededToWin;
        ChangeUITimer(enemyTimer);
    }
    void Update()
    {
        if(GameManager.VirusTiles == tilesAvaibleToPaint && playerTurn)
        {
            playerTurn = false;
            StartEnemyTurn();
        }

        if(!playerTurn && !endedGame)
        {
            timer += Time.deltaTime;
            ChangeUITimer(enemyTimer - timer);
            if(timer >= enemyTimer)
            {
                endedGame = true;
                DestroyEnemies();
                if (GameManager.VirusTiles >= tilesNeededToWin)
                    Debug.Log("win");

            }
        }

        if(playerTurn)
            _avaibleTilesText.text = "AVAIBLE " + (tilesAvaibleToPaint - GameManager.VirusTiles);
        _currentTilesText.text = "CURRENT: " + GameManager.VirusTiles;




    }

    private void DestroyEnemies()
    {
       for(int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
    }

    private void ChangeUITimer(float timer)
    {
            string seconds = ((int)(timer % 60)).ToString("00");
            string minutes = ((int)(timer / 60)).ToString("00");
            _timerUI.text = minutes + ":" + seconds;
    }

    private void StartEnemyTurn()
    {
        player.SetActive(false);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].gameObject.SetActive(true);
        }
    }
}
