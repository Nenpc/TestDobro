using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{
    public event Action EndTimeAction;
    
    [SerializeField] private Text _timeText;
    [SerializeField] private int _maxTime;
    
    private int _currentTime;
    private DateTime _startTime;
    private void Awake()
    {
        _startTime = DateTime.Now;
        _currentTime = _maxTime;
        _timeText.text = _currentTime.ToString();
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (_currentTime > 0)
        {
            yield return new WaitForSeconds(1);
            _currentTime--;
            _timeText.text = _currentTime.ToString();
        }
        
        EndTimeAction?.Invoke();
    }

    public int GetResult()
    {
        return (int)(DateTime.Now - _startTime).TotalSeconds;
    }

    public int GetBestResult(int level)
    {
        if (PlayerPrefs.HasKey($"Level_{level}"))
        {
            return PlayerPrefs.GetInt($"Level_{level}");
        }
        else
        {
            return -1;
        }
    }

    public void SaveBestResult(int level, int result)
    {
        PlayerPrefs.SetInt($"Level_{level}", result);
    }
}