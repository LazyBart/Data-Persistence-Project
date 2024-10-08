using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public string playerName; // jmeno aktualniho hrace
    public string bestPlayerName; // jmeno hrace drziciho rekord
    public int bestScore; // rekord
    public TMP_InputField inputField;
    public TextMeshProUGUI highScore;
    public static MenuManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadScore();
        highScore.text = "Best Score: " + bestPlayerName + " - " + bestScore;
        inputField.onEndEdit.AddListener(OnEndEdit); 
    }

    // Datova trida pro serializaci ukladanych dat
    [System.Serializable]
    class SavedData
    {
        public string playerName;
        public int bestScore;
    }

    // Ulozeni rekordniho skore - hrac plus body
    public void SaveScore()
    {
        SavedData data = new SavedData();
        data.bestScore = bestScore;
        data.playerName = bestPlayerName;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savedscore.json", json);
    }

    // Nacteni ulozeneho skore
    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savedscore.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SavedData data = JsonUtility.FromJson<SavedData>(json);
            bestScore = data.bestScore;
            bestPlayerName = data.playerName;
        }
    }

    // ukonceni editace jmena - bud klavesou Enter nebo kliknutim kamkoliv jinam
    void OnEndEdit (string name)
    {
        playerName = name;
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
