using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// MainManager for Data-PersistenceProject

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text BestScoreText;

    private bool m_Started = false;
    private int m_Points;
    private string m_Name;
    private string m_HighScoreName;
    private int m_HighScore;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        m_HighScoreName = MenuUIHandler.Instance.highScoreName;
        m_HighScore = MenuUIHandler.Instance.highScore;
        m_Name = MenuUIHandler.Instance.playerName;
        ScoreText.text = $"Score : {m_Name} : {m_Points}";
        BestScore();

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void BestScore()
    {
        BestScoreText.text = $"Best Score : {m_HighScoreName} : {m_HighScore}";
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Name} : {m_Points}";
    }

    public void GameOver()
    {
        if (m_Points > m_HighScore)
        {
            m_HighScore = m_Points;
            m_HighScoreName = m_Name;
            SaveHighScore();
            MenuUIHandler.Instance.LoadHighScore();
            BestScore();
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string name;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.highScore = m_HighScore;
        data.name = m_Name;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
}
