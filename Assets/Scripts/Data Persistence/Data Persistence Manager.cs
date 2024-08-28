/*****************************************************************************
// File Name :         Data Persistence Manager.cs
// Author :            Amber C. Cardamone
// Creation Date :     August 23rd, 2024
//
// Brief Description : Saves and Loads the High Score information.
*****************************************************************************/

// Gives easier syntax for finding IDataPersistence game objects
using System.IO;
using UnityEngine;
public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    public GameData GameData;
    //private List<IDataPersistence> dataPersistenceObjects;

    //private FileDataHandler dataHandler;

    public DataPersistenceManager instance { get; private set; }

    // Used as the directory to where the data will be saved
    private string dataDirPath;
    // This will be the name of the file the data saves to
    private string dataFileName = "SaveData.Json";


    /// <summary>
    /// Loads the saved data on launch;
    /// </summary>
    private void Start()
    {
        dataDirPath = Application.persistentDataPath;
        Load();
    }

    /// <summary>
    /// Sets up the game to start with a new data system save.
    /// </summary>
    public void NewGame()
    {
        // Makes our current Game Data become a New Game Data
        this.GameData = new GameData();
    }

    

    /// <summary>
    /// Returns a game data object.
    /// </summary>
    /// <returns></returns>
    public GameData Load()
    {
        // uses Path.Combine to account for different kind of operating systems having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        // The loadedData will be the variable that will be loaded into
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            print("Found File");
            // loads the serialized data from the file
            string dataToLoad = File.ReadAllText(fullPath);
            print ("data to load = " + dataToLoad);
            // Reaches into the file to find the path

            // Loads in the serialized Data
            //dataToLoad = reader.ReadToEnd();

            // Deserealizes the data from JSon back into the C# object
            loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            GameData.HighScore = loadedData.HighScore;
            print("loaded data = " + loadedData);
            print("Game Data High Score = " + GameData.HighScore);
        }
        return loadedData;
    }

    /// <summary>
    /// Takes in a game data object and puts it into a JSon save file.
    /// </summary>
    /// <returns></returns>
    public void Save()
    {
        // uses Path.Combine to account for different kind of operating systems having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        // Create the directory path in case it doesn't exist on our computer
        //Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        // Serialize the C# game data object into JSon
        string dataToStore = JsonUtility.ToJson(GameData, true);
        Debug.Log(fullPath);
        File.WriteAllText(fullPath, dataToStore);
        // Write the serialized data to the file
        /*using (FileStream stream = new FileStream(fullPath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                // Puts in the data that will be added to the file
                writer.Write(dataToStore);
            }
        }*/


    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
