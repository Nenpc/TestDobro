using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    [SerializeField] private int _nextLevel;
    [SerializeField] private int _currentLevel;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private GameObject _player;
    [SerializeField] private CanvasGroup _exitBackgroundImageCanvasGroup;
    [SerializeField] private CanvasGroup _caughtBackgroundImageCanvasGroup;
    [SerializeField] private Text _result;
    [SerializeField] private Text _bestResult;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _restartLevelButton;
    [SerializeField] private Progress _progress;

    private float _timer;
    private bool _isEnded;

    private void Awake()
    {
        _isEnded = false;
        EnemyPatrol.CatchPlayerAction += Lose;
        _progress.EndTimeAction += Lose;
        _nextLevelButton.onClick.AddListener(NextLevel);
        _restartLevelButton.onClick.AddListener(RestartLevel);
    }

    private void OnDestroy()
    {
        EnemyPatrol.CatchPlayerAction -= Lose;
        _progress.EndTimeAction -= Lose;
        _nextLevelButton.onClick.RemoveListener(NextLevel);
        _restartLevelButton.onClick.RemoveListener(RestartLevel);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player)
        {
            var result = _progress.GetResult();
            _result.text = "Result:" + result;
            var bestResult = _progress.GetBestResult(_currentLevel);
            if (bestResult != -1)
            {
                _bestResult.gameObject.SetActive(true);
                _bestResult.text = "Best result:" + bestResult;
            }
            else
            {
                _bestResult.gameObject.SetActive(false);
            }
        
            if (result < bestResult || bestResult == -1)
                _progress.SaveBestResult(_currentLevel, result);
            
            StartCoroutine(EndLevel(_exitBackgroundImageCanvasGroup));
        }
    }

    private void Lose()
    {
        StartCoroutine(EndLevel(_caughtBackgroundImageCanvasGroup));
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(_currentLevel);
    }

    private void NextLevel()
    {

        SceneManager.LoadScene(_nextLevel);
    }

    private IEnumerator EndLevel(CanvasGroup imageCanvasGroup)
    {
        if (_isEnded)
            yield break;

        _isEnded = true;
        
        while (imageCanvasGroup.alpha != 1)
        {
            _timer += Time.deltaTime;
            imageCanvasGroup.alpha = _timer / _fadeDuration;
            yield return null;
        }
    }
}
