using UnityEngine;
using TMPro;
using DG.Tweening;
using Lean.Touch;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] TextMeshProUGUI levelCompleteText;
    [SerializeField] private Button nextButton;
    [SerializeField] TextMeshProUGUI loseText;
    [SerializeField] TextMeshProUGUI tryAgainText;
    [SerializeField] private Button restartButton;
    [SerializeField] private ParticleSystem[] winEffects;

    void Start()
    {
        EventManager.current.onStartGame += OnStartGame;
        EventManager.current.onFinishGame += OnFinishGame;
        EventManager.current.onWinGame += OnWinGame;
        EventManager.current.onLoseGame += OnLoseGame;
    }
    
    void OnDestroy()
    {
        EventManager.current.onStartGame -= OnStartGame;
        EventManager.current.onFinishGame -= OnFinishGame;
        EventManager.current.onWinGame -= OnWinGame;
        EventManager.current.onLoseGame -= OnLoseGame;
    }
    
    void OnStartGame()
    {
        
    }
    
    void OnFinishGame()
    {
        
    }
    
    void OnWinGame()
    {
        winText.transform.gameObject.SetActive(true);
        winText.transform.localPosition = new Vector3(transform.localPosition.x - 500, transform.position.y, transform.position.z);
        winText.transform.DOMoveX(500, 1f).OnComplete(() =>
        {
            winText.DOFade(0f, 1f).OnComplete(() =>
            {
                nextButton.transform.gameObject.SetActive(true);
                levelCompleteText.transform.gameObject.SetActive(true);
                levelCompleteText.DOFade(100f, 1f).OnComplete(() =>
                {
                    EventManager.current.OnFinishGame();
                });
            });
        });
        for (int i = 0; i < winEffects.Length; i++)
        {
            winEffects[i].transform.gameObject.SetActive(true);
            winEffects[i].Play();
        }
    }
    
    void OnLoseGame()
    {
        loseText.transform.gameObject.SetActive(true);
        loseText.transform.localPosition = new Vector3(transform.localPosition.x - 500, transform.position.y, transform.position.z);
        loseText.transform.DOMoveX(500, 1f).OnComplete(() =>
        {
            loseText.DOFade(0f, 1f).OnComplete(() =>
            {
                restartButton.transform.gameObject.SetActive(true);
                tryAgainText.transform.gameObject.SetActive(true);
                tryAgainText.DOFade(100f, 1f).OnComplete(() =>
                {
                    EventManager.current.OnFinishGame();
                });
            });
        });
    }

    public void OnPressNextButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void OnPressRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
