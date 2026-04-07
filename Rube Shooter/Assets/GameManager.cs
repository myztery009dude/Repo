using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public HighScores highScores;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoretext;

    public GameObject highScorePanel;
    public TextMeshProUGUI highScoresText;

    public Button newgameButton;
    public Button highScoresButton;

    public TargetHealth[] targets;
    public GameObject player;
    public Camera worldCamera;

    public float startTimerAmount = 3;
    private float startTimer;

    public float targetActivateTimerAmount = 1;
    private float targetActivateTimer;

    public float gameTimerAmount = 60;
    private float gameTimer;

    private int score = 0;
    private int level = 1;

    public enum GameState
    {
        Start,
        Playing,
        GameOver,
    };

    private GameState gameState;
    public GameState State { get { return gameState; } }

    private void Awake()
    {
        gameState = GameState.GameOver;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        player.SetActive(false);
        worldCamera.gameObject.SetActive(true);

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].GameManager = this;
            targets[i].gameObject.SetActive(false);
        }

        startTimer = startTimerAmount;
        messageText.text = "Press Enter to Start";
        timerText.text = "";
        scoretext.text = "";

        highScorePanel.gameObject.SetActive(false);
        newgameButton.gameObject.SetActive(true);
        highScoresButton.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        switch (gameState)
        {
            case GameState.Start:
                GameStateStart();
                break;
            case GameState.Playing:
                GameStatePlaying();
                break;
            case GameState.GameOver:
                GameStateGameOver();
                break;
        }
    }

    private void GameStateStart()
    {
        startTimer -= Time.deltaTime;
        messageText.text = "Get Ready " + (int)(startTimer + 1);

        if (startTimer < 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            messageText.text = "";
            gameState = GameState.Playing;
            gameTimer = gameTimerAmount;
            startTimer = startTimerAmount;
            score = 0;

            player.SetActive(true);
            worldCamera.gameObject.SetActive(false);
        }
    }

    private void GameStatePlaying()
    {
        gameTimer -= Time.deltaTime;

        int seconds = Mathf.RoundToInt(gameTimer);
        timerText.text = "Time: " + seconds;

        scoretext.text = "Score: " + score + " | Level: " + level;

        if (gameTimer <= 0)
        {
            if (level == 1)
            {
                level = 2;
                messageText.text = "LEVEL 2!";

                gameTimer = gameTimerAmount;
                targetActivateTimerAmount = 0.5f;

                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].gameObject.SetActive(false);
                }

                gameState = GameState.Start;
                return;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                messageText.text = "Game Over! Score: " + score;

                gameState = GameState.GameOver;
                player.SetActive(false);
                worldCamera.gameObject.SetActive(true);

                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].gameObject.SetActive(false);
                }

                highScores.AddScore(score);
                highScores.SaveScoresToFile();

                newgameButton.gameObject.SetActive(true);
                highScoresButton.gameObject.SetActive(true);
            }
        }

        targetActivateTimer -= Time.deltaTime;

        if (targetActivateTimer <= 0)
        {
            ActivateRandomTarget();
            targetActivateTimer = targetActivateTimerAmount;
        }
    }

    private void GameStateGameOver()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            level = 1;
            gameState = GameState.Start;
            timerText.text = "";
            scoretext.text = "";
        }
    }

    private void ActivateRandomTarget()
    {
        int randomindex = Random.Range(0, targets.Length);
        targets[randomindex].gameObject.SetActive(true);
    }

    public void AddScore(int points)
    {
        score += points;
        scoretext.text = "Score: " + score + " | Level: " + level;
    }

    public void OnNewGame()
    {
        level = 1;
        gameState = GameState.Start;
    }

    public void OnHighScores()
    {
        messageText.text = "";

        highScoresButton.gameObject.SetActive(false);
        highScorePanel.gameObject.SetActive(true);

        string text = "";
        for (int i = 0; i < highScores.scores.Length; i++)
        {
            text += highScores.scores[i] + "\n";
        }
        highScoresText.text = text;
    }
}