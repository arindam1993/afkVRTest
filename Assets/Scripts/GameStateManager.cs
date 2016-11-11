using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {

	public static GameStateManager Instance { get; private set; }

    public static string highScoreKey = "AFKVR_HIGH_SCORE";
    public static string highPlayereKey = "AFKVR_HIGH_PLAYER";
    public EnemySpawner spawner;
    public VRPlayer player;
    public GazeKeyboard keyboard;
    public GazeMainMenu startScreen;
    public int HighScore { get; private set; }
    public string HighPlayer { get; private set; }

    public Text HealthUI;
    public Text HighScoreNameUI;
    public Text HighScoreUI;
    public Text CurrScoreUI;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        keyboard.Hide();
        player.Reset();

        HealthUI.enabled = false;
        HighScoreNameUI.enabled = false;
        HighScoreUI.enabled = false;
        CurrScoreUI.enabled = false;
    }

    public void GameStart()
    {
        HighScore = PlayerPrefs.GetInt(highScoreKey, 0);
        HighPlayer = PlayerPrefs.GetString(highPlayereKey, "Nobody");
        player.Reset();
        spawner.StartSpawning();
        keyboard.Hide();
        startScreen.Hide();

        HealthUI.enabled = true;
        HighScoreNameUI.enabled = true;
        HighScoreUI.enabled = true;
        CurrScoreUI.enabled = true;

        HealthUI.text = player.Health+"" ;
        HighScoreNameUI.text = HighPlayer + "";
        HighScoreUI.text = HighScore + "";
        CurrScoreUI.text = player.Score + "";
    }


    public void GameEnd()
    {
        HealthUI.enabled = false;
        HighScoreNameUI.enabled = false;
        HighScoreUI.enabled = false;
        CurrScoreUI.enabled = false;

        spawner.StopSpawning();
        spawner.DeactivateAll();
        Debug.Log("GAME ENDED");
        if(player.Score > HighScore)
        {
            keyboard.Score = player.Score;
            keyboard.Show();
            keyboard.OnNameSaved.AddListener(saveScore);
        }else
        {
            startScreen.Show();
        }
    }

    void saveScore(string name, int score)
    {
        PlayerPrefs.SetString(highPlayereKey, name);
        PlayerPrefs.SetInt(highScoreKey, score);
        PlayerPrefs.Save();
        Debug.Log("Saving Score:" + name + ", " + score);
        keyboard.Hide();
        startScreen.Show();
    }


}
