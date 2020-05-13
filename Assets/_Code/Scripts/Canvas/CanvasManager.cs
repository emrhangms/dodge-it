using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager ins;

    public Slider slider;
    public float gameSeconds;
    public bool started;
    public float startTime;
    public bool losed;

    public CanvasGroup StartPanel;
    public CanvasGroup InGamePanel;
    public CanvasGroup WinPanel;
    public GameObject WinPanelCamera;
    public CanvasGroup LosePanel;

    public Player player;

    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI winHighScoreTxt;
    public TextMeshProUGUI winScoreTxt;

    private void Awake() => ins = this;

    void Start()
    {
        Time.timeScale = 0;

        started = false;

        player = Player.ins;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !started)
            StartGame();

        if (started)
            UpdateScore();
    }

    public void StartGame()
    {
        Time.timeScale = 1;

        started = true;
        startTime = Time.time;
        StartPanel.DOFade(0, 0.3f);
        StartPanel.interactable = false;
        StartPanel.blocksRaycasts = false;

        InGamePanel.DOFade(1, 0.3f);
        InGamePanel.interactable = true;
        InGamePanel.blocksRaycasts = true;
    }

    public void UpdateScore()
    {
        if (losed) return;

        scoreTxt.text = player.score.ToString();
    }
/*
    public void WinGame()
    {
        InGamePanel.DOFade(0, 0.3f);
        InGamePanel.interactable = false;
        InGamePanel.blocksRaycasts = false;

        WinPanel.DOFade(1, 0.5f);
        WinPanel.interactable = true;
        WinPanel.blocksRaycasts = true;

        WinPanelCamera.SetActive(true);
    }
*/
    public void LoseGame()
    {
        InGamePanel.DOFade(0, 0.3f);
        InGamePanel.interactable = false;
        InGamePanel.blocksRaycasts = false;

        LosePanel.DOFade(1, 0.3f);
        LosePanel.interactable = true;
        LosePanel.blocksRaycasts = true;

        LosePanel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.3f);
        CameraController.ins.ShakeCamera(0.5f);
        losed = true;

        CheckHighScore();
    }

    public void CheckHighScore()
    {
        if (player.score > PlayerPrefs.GetInt("highscore"))
            PlayerPrefs.SetInt("highscore", player.score);

        winHighScoreTxt.text = "High Score : " + PlayerPrefs.GetInt("highscore").ToString();
        winScoreTxt.text = "Score : " + player.score.ToString();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("Main");
    }

}
