using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

[RequireComponent(typeof(GameUI))]
public class GameController : MonoBehaviour
{

    public static GameController Instance { get; private set; }
    [SerializeField]
    private int knifeCount;
    
    [Header("Knife Spawning")]
    [SerializeField]
    private Vector2 knifeSpawnPosition;
    [SerializeField]
    private GameObject knifeObject;

    public GameUI GameUI { get; private set; }
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScore;

    private void Awake()
    {
        Instance = this;

        GameUI = GetComponent<GameUI>();
    }
   
    void Start()
    {
        GameUI.SetInitialDisplayedKnifeCount(knifeCount);
        SpawnKnife();
        scoreText.text = "S:" + Scoring.totalScore;
        UpdateHighScoreText();
    }
   
    void CheckHighScore()
    {
        if (Scoring.totalScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", Scoring.totalScore);
        }
    }
    void UpdateHighScoreText()
    {
        highScore.text = "H:" + PlayerPrefs.GetInt("HighScore", 0);
    }
    public void OnSuccessfulKnifeHit()
    {
        if (knifeCount > 0)
        {
            SpawnKnife();
            Scoring.totalScore += 1;
            CheckHighScore();
            UpdateHighScoreText();
            scoreText.text = "S:" + Scoring.totalScore;


        }
        else
        {
            StartGameOverSequence(true);
        }
       
    }

    private void SpawnKnife()
    {
        knifeCount--;
        Instantiate(knifeObject, knifeSpawnPosition, Quaternion.identity);
    }

    public void StartGameOverSequence(bool win)
    {
        StartCoroutine("GameOverSequenceCoroutine", win);
    }

    private IEnumerator GameOverSequenceCoroutine(bool win)
    {
        if (win)
        {
            yield return new WaitForSecondsRealtime(0.3f);
            RestartGame();
        }
        else
        {
            GameUI.ShowRestartButton();
            Scoring.totalScore = 0;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
