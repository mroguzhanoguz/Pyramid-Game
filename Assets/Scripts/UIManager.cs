using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject completedPanel;
    [SerializeField] private GameObject failedPanel;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        ResetUI();
        AppointStateEvents();
    }

    void ResetUI()
    {
        startPanel.SetActive(true);
        inGamePanel.SetActive(false);
        completedPanel.SetActive(false);
        failedPanel.SetActive(false);
    }
    void AppointStateEvents()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.OnInitialized += Initialize;
        gameManager.OnPlayStarted += OnPlayStarted;
        gameManager.OnCompleted += OnCompleted;
        gameManager.OnFailed += OnFailed;
    }
    void OnPlayStarted()
    {
        startPanel.SetActive(false);
        inGamePanel.SetActive(true);
    }
    void OnCompleted()
    {
        inGamePanel.SetActive(false);
        completedPanel.SetActive(true);
    }
    void OnFailed()
    {
        inGamePanel.SetActive(false);
        failedPanel.SetActive(true);
    }

    public void UIButton_Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}