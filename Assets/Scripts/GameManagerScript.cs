using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    #region Fields
    [Tooltip("Script with game configuration")]
    [SerializeField]
    private GameConfigScript config;

    [Tooltip("Canvas with game GUI")]
    [SerializeField]
    private GameObject gameScreen;

    [Tooltip("Display of current gesture pattern")]
    [SerializeField]
    private Image display;

    [Tooltip("Current score text")]
    [SerializeField]
    private Text scoreText;

    [Tooltip("Time left text")]
    [SerializeField]
    private Text timeText;

    [Tooltip("Alert text")]
    [SerializeField]
    private Text alertText;

    [Tooltip("Canvas with start game GUI")]
    [SerializeField]
    private GameObject startScreen;

    [Tooltip("Start button text")]
    [SerializeField]
    private Text startButtonText;

    [Tooltip("Total score text")]
    [SerializeField]
    private Text totalScoreText;

    private int score = 0;
    private int level = 0;
    private float alertTime = 0f;

    private int index = 0;
    private float time = 0f;
    private bool gameInProcess = false;
    private static GameManagerScript instance = null;
    #endregion

    #region Properties
    public int Index
    {
        get { return index; }
    }
    public bool GameInProcess
    {
        get { return gameInProcess; }
    }
    public static GameManagerScript Instance
    {
        get { return instance; }
    }
    #endregion

    #region Methods
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start ()
    {
        totalScoreText.enabled = false;
        gameScreen.SetActive(false);
    }
	
	private void Update ()
    {
		if (GameInProcess)
        {
            time -= Time.deltaTime;

            if (time <= 0)
            {
                time = 0;
                GameOver();
            }

            timeText.text = "Time left: " + time.ToString("F2");

            if (alertTime > 0)
            {
                alertTime -= Time.deltaTime;

                if (alertTime <= 0)
                {
                    alertText.enabled = false;
                }
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
	}

    public void NextLevel()
    {
        if (GameInProcess)
        {
            if (Index < config.Patterns.Length - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
            level++;
            score++;
        }
        else
        {
            gameInProcess = true;
            startScreen.SetActive(false);
            gameScreen.SetActive(true);
            alertText.enabled = false;
            index = 0;
            level = 0;
            score = 0;
        }

        time = config.InitialTime - config.DeltaTime * level;

        if (time < config.MinTime)
        {
            time = config.MinTime;
        }

        scoreText.text = "Score: " + score;
        display.sprite = config.Patterns[Index].display;
    }

    public void Alert(string text)
    {
        alertText.enabled = true;
        alertText.text = text;
        alertTime = config.AlertTime;
    }

    private void GameOver()
    {
        gameInProcess = false;
        startScreen.SetActive(true);
        gameScreen.SetActive(false);
        totalScoreText.enabled = true;
        totalScoreText.text = "Total score: " + score;
        startButtonText.text = "Retry";
    }
    #endregion
}
