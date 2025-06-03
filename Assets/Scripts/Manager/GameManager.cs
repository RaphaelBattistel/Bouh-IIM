using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    

    private List<BlocController> blocs = new();

    public Canvas win;
    [SerializeField] private Canvas pause;
    [SerializeField] private Canvas settings;

    [SerializeField] private bool gameIsPaused;
    //public TextMeshProUGUI WinText;

    private void Awake()
    {
        
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        
        blocs.AddRange(FindObjectsOfType<BlocController>());
    }
    private void Start()
    {
        win.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
    }

    public void ToggleStopBloc()
    {
        foreach (var bc in blocs)
        {
            bc.IsMoved = !bc.IsMoved;
        }
    }

    public void Win()
    {
        win.gameObject.SetActive(true);
    }

    public void Paused()
    {
        pause.gameObject.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void Resume()
    {
        pause.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        gameIsPaused = false;
    }

    public void Settings()
    {
        settings.gameObject.SetActive(true);
    }
}