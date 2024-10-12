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
    public Container container;  // Datovy typ pro serializaci
    public TMP_InputField inputField;
    public TextMeshProUGUI highScore;
    public Button startButton;
    public Button quitButton;
    public Button hallOfFameButton;
    public static MenuManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            container = new Container();
            container.dataList = new List<SavedData>();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            // Najdeme tlaèítka a InputField pøi návratu do menu
            inputField = GameObject.Find("Input Name").GetComponent<TMP_InputField>();
            highScore = GameObject.Find("HighestScore").GetComponent<TextMeshProUGUI>();
            startButton = GameObject.Find("ButtonStart").GetComponent<Button>();
            quitButton = GameObject.Find("ButtonQuit").GetComponent<Button>();
            hallOfFameButton = GameObject.Find("ButtonHoF").GetComponent<Button>();


            if (inputField != null)
            {
                inputField.onEndEdit.AddListener(OnEndEdit);
            }
            if (startButton != null)
            {
                startButton.onClick.AddListener(StartGame);
            }
            if (quitButton != null)
            {
                quitButton.onClick.AddListener(ExitGame);
            }
            if(hallOfFameButton != null)
            {
                hallOfFameButton.onClick.AddListener(HallOfFame);
            }

            LoadScore();
            highScore.text = "Best Score: " + bestPlayerName + " : " + bestScore;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Datova trida pro serializaci ukladanych dat
    [System.Serializable]
    public class SavedData
    {
        public string playerName;
        public int score;
    }

    [System.Serializable]
    public class Container
    {
        public List<SavedData> dataList;
    }

    // Ulozeni rekordniho skore - hrac plus body
    public void SaveScore()
    {
        string json = JsonUtility.ToJson(container);
        File.WriteAllText(Application.persistentDataPath + "/savedtopscores.json", json);
    }

    // Nacteni ulozeneho skore
    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savedtopscores.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            container = JsonUtility.FromJson<Container>(json);
            bestScore = container.dataList[0].score;
            bestPlayerName = container.dataList[0].playerName;
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
    private void HallOfFame()
    {
        SceneManager.LoadScene(2);
    }
}
