using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HallOfFame : MonoBehaviour
{
    public TextMeshProUGUI topNames;
    public TextMeshProUGUI topScores;
    
    private void Start()
    {
        topNames = GameObject.Find("Names").GetComponent<TextMeshProUGUI>();
        topScores = GameObject.Find("Scores").GetComponent<TextMeshProUGUI>();
        string names = string.Join("\n", MenuManager.Instance.container.dataList.Select(item => item.playerName));
        string scores = string.Join("\n", MenuManager.Instance.container.dataList.Select(item => item.score));
        DisplayTopPlayers(names, scores);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void DisplayTopPlayers(string names, string scores)
    {
        topNames.text = names;
        topScores.text = scores;
    }
}
