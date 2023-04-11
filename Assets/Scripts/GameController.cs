using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI timeLeftLabel;
    [SerializeField] private float roundDuration = 30f;
    [SerializeField] private float quadUpdateInterval = 1f;
    [Header("Views")]
    [SerializeField] private GameObject menuViewGO;
    [SerializeField] private GameObject scoresViewGO;
    [Header("Refs")]
    [SerializeField] private QuadsManager quadsManager;
    [SerializeField] private TextMeshProUGUI scoreTextLabelGame;
    [SerializeField] private TextMeshProUGUI scoreTextLabelScore;

    private QuadsManager.QuadState _targetColor;
    private int _score;
    private Coroutine _currentRoundCoroutine;

    public event Action OnRoundFinished;
    
    private void Start()
    {
        ShowMenuView();
        quadsManager.UpdateScore += OnUpdateScore;
    }

    private void StartNewRound()
    {
        OnRoundFinished += ShowScoresView;
        
        if (_currentRoundCoroutine != null)
        {
            StopCoroutine(_currentRoundCoroutine);
        }

        _score = 0;
        UpdateScoreText();

        quadsManager.ResetQuads();
        _currentRoundCoroutine = StartCoroutine(PlayRound());
    }

    private IEnumerator PlayRound()
    {
        var elapsedTime = 0f;

        while (elapsedTime < roundDuration)
        {
            yield return new WaitForSeconds(quadUpdateInterval);

            quadsManager.UpdateQuads();

            elapsedTime += quadUpdateInterval;
            var timeLeft = roundDuration - elapsedTime;
            SetTimeLeft(timeLeft);
        }

        _currentRoundCoroutine = null;
        OnRoundFinished?.Invoke();
    }

    #region views

    public void OnPlayClicked()
    {
        ShowGameView();
        StartNewRound();
    }

    public void OnMenuClicked()
    {
        ShowMenuView();
    }
    
    private void ShowScoresView()
    {
        scoresViewGO.SetActive(true);
    }

    private void ShowMenuView()
    {
        scoresViewGO.SetActive(false);
        menuViewGO.SetActive(true);
    }
    
    private void ShowGameView()
    {
        scoresViewGO.SetActive(false);
        menuViewGO.SetActive(false);
    }
    
    #endregion
    
    private void UpdateScoreText()
    {
        scoreTextLabelGame.text = $"Score: {_score}".ToUpper();
        scoreTextLabelScore.text  = $"Your score: {_score}".ToUpper();
    }

    private void SetTimeLeft(float timeLeft)
    {
        timeLeftLabel.text = $"Time Left: {Mathf.RoundToInt(timeLeft)}".ToUpper();
    }
    
    private void OnUpdateScore(bool isMatch)
    {
        if (isMatch)
        {
            _score++;
        }
        else
        {
            _score--;
        }
        
        _score = Mathf.Max(_score, 0);
        
        scoreTextLabelGame.text = $"Score: {_score}".ToUpper();
        scoreTextLabelScore.text = $"Score: {_score}".ToUpper();
    }
}
