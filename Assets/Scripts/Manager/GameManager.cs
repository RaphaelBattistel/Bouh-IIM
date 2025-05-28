using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    

    private List<BlocController> blocs = new();

    public TextMeshProUGUI WinText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        blocs.AddRange(FindObjectsOfType<BlocController>());
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
        WinText.gameObject.SetActive(true);
    }
}