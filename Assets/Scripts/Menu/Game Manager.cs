/*****************************************************************************
// File Name :         Game Manager.cs
// Author :            Amber C. Cardamone
// Creation Date :     August 22nd, 2024
//
// Brief Description : Keeps track of the player's score, and loads the lose screen.
*****************************************************************************/
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour//, IDataPersistence
{
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private TMP_Text _healthText;
    private int currentScore;
    [SerializeField] private TMP_Text _currentScoreText;
    [SerializeField] private TMP_Text _currentScoreTextEndScreen;
    //[SerializeField] private int _highScore;
    [SerializeField] private TMP_Text _highScoreText;
    [SerializeField] private TMP_Text _highScoreTextEndScreen;
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _gameInfo;
    [SerializeField] private PlayerControls _playerControls;
    //[SerializeField] private SaveSystem _saveSystem;
    [SerializeField] private AudioClip _lifeLostSound;
    //[SerializeField] private SaveLoadJSON _saveLoadJSON;
    //[SerializeField] private FileDataHandler _fileDataHandler;
    [SerializeField] private DataPersistenceManager _dataPersistenceManager;


    //public int HighScore { get => _highScore; set => _highScore = value; }

    /*
    /// <summary>
    /// Allows the highscore data saved in JSON to be used in the GameManager;
    /// </summary>
    /// <param name="data"></param>
    public void LoadData(GameData data)
    {
        this.highScore = data.HighScore;
    }

    /// <summary>
    /// Allows the highscore data found in the Game Manager to be saved through JSON
    /// </summary>
    /// <param name="data"></param>
    public void SaveData(ref GameData data)
    {
        data.HighScore = this.highScore;
    }
    */

    /// <summary>
    /// Sets the player's health and score back to their base stats, and changed the text back to their bases.
    /// </summary>
    void Start()
    {
        print("High Score: " + _dataPersistenceManager.GameData.HighScore);
        //_highScoreText.text = PlayerPrefs.GetInt("High Score: ", 0).ToString();
        //_saveSystem = FindObjectOfType<SaveSystem>();
        //_saveLoadJSON = FindObjectOfType<SaveLoadJSON>();
        //_saveLoadJSON.LoadGame();
        _dataPersistenceManager.Load();
        UpdateText();
        _gameInfo.SetActive(true);
        _deathScreen.SetActive(false);
        _health = _maxHealth;
        //currentScore = 1000;
    }

    /// <summary>
    /// Makes the player gain points after defeating an enemy.
    /// </summary>
    public void PlayerAddScore()
    {
        //_saveSystem.PlayerAddScore();
        currentScore += 1000;
        _currentScoreText.text = ("Score: " + currentScore);
        if (currentScore >= _dataPersistenceManager.GameData.HighScore)
        {
            _dataPersistenceManager.GameData.HighScore = currentScore;
            print("High Score: " + _dataPersistenceManager.GameData.HighScore);
            //_dataPersistenceManager.GameData.HighScore = _highScore;
            //PlayerPrefs.SetInt("HighScore", currentScore);
            UpdateText();
        }
    }
    
    /// <summary>
    /// Makes the player lose health after getting hit by an enemy.
    /// </summary>
    public void PlayerLoseHealth()
    {
        AudioSource.PlayClipAtPoint(_lifeLostSound, transform.position);
        _health -= 1;
        _healthText.text = ("Health: " + _health);
        if (_health <= 0)
        {
            _dataPersistenceManager.Save();
            //dataHandler.Save(GameData);
            _deathScreen.SetActive(true);
            _gameInfo.SetActive(false);
            UpdateText();
            _highScoreTextEndScreen.text = ("High Score: " + _dataPersistenceManager.GameData.HighScore);
            //_highScoreTextEndScreen.text = _highScore.ToString();// PlayerPrefs.GetInt("High Score: " + HighScore).ToString();
            _currentScoreTextEndScreen.text = ("Score: " + currentScore);
            _playerControls.Die();
        }
    }

    private void UpdateText()
    {
        _currentScoreText.text = ("Score: " + currentScore);
        _healthText.text = ("Health: " + _health);
        _highScoreText.text = ("High Score: " + _dataPersistenceManager.GameData.HighScore);
    }
}
