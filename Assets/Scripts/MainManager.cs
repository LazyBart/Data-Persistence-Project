using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
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
        DisplayBestPlayer();
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
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        MenuManager.SavedData data = new MenuManager.SavedData();
        data.playerName = MenuManager.Instance.playerName;
        data.score = m_Points;  // Vytvorime promennou typu SavedData a naplnime ji daty z prave skoncene hry
        MenuManager.Instance.container.dataList.Add(data);  // Pridame je do vysledneho seznamu
        MenuManager.Instance.container.dataList.Sort((x, y) => y.score.CompareTo(x.score));  //  Ten setridime podle skore sestupne
        if (MenuManager.Instance.container.dataList.Count > 5)  // Je-li v seznamu vice nez 5 zaznamu..
        {
            MenuManager.Instance.container.dataList.RemoveAt(5);  // ..tak posledni zaznam vymazeme
        }
        MenuManager.Instance.bestScore = MenuManager.Instance.container.dataList[0].score;  // Aktualizujeme hodnotu rekordu a jmeno jeho drzitele
        MenuManager.Instance.bestPlayerName = MenuManager.Instance.container.dataList[0].playerName;
        DisplayBestPlayer();  // A zobrazime jej na obrazovce
        MenuManager.Instance.SaveScore();  // Nakonec seznam ulozime na disk
    }

    private void DisplayBestPlayer()
    {
        BestScoreText.text = "Best Score: " + MenuManager.Instance.bestPlayerName + " : " + MenuManager.Instance.bestScore;
    }
}
