using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif

// MenuUIHandler for Data-PersistenceProject

public class MenuUIHandler : MonoBehaviour
{
    public static MenuUIHandler Instance;

    public string playerName;
    public int highScore;
    public string highScoreName;
    public TextMeshProUGUI BestScoreText;

    public GameObject inputField;
    // public TextMeshProUGUI test;

    // Start is called before the first frame update

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadHighScore();
        BestScoreText.text = $"Best Score : {highScoreName} : {highScore}";
        // Debug.Log(highScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetPlayerName()
    {
        // playerName = test.text;  // Use this for TextMeshPro Input field
        playerName = inputField.GetComponent<Text>().text;  // Use this for standard Unity input field
        Debug.Log(playerName);
    }

    public void StartNew()
    {
        playerName = inputField.GetComponent<Text>().text;
        SceneManager.LoadScene(1);
        gameObject.SetActive(false);
    }

    public void Exit()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
# endif
    }

    public void ResetHighScore()
    {
        highScore = 0;
        playerName = "";
        SaveHighScore();
        Start();
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string name;
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data.highScore;
            highScoreName = data.name;
        }
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.highScore = highScore;
        data.name = playerName;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
}
