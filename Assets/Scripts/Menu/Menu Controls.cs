/*****************************************************************************
// File Name :         Menu Controls.cs
// Author :            Amber C. Cardamone
// Creation Date :     August 22nd, 2024
//
// Brief Description : Makes buttons usable by making the player able to restart and quit.
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    //[SerializeField] private SaveLoadJSON _saveLoadJSON;
    [SerializeField] private DataPersistenceManager _dataPersistenceManager;

    /// <summary>
    /// Restarts the game.
    /// </summary>
    public void Restart()
    {
        //_saveLoadJSON.SaveGame();
        _dataPersistenceManager.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    /// <summary>
    /// Quits the game.
    /// </summary>
    public void Quit()
    {
        _dataPersistenceManager.Save();
        //_saveLoadJSON.SaveGame();
        print("Quit Game");
        Application.Quit();
    }
}
