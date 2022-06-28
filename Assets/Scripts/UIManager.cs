using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    [SerializeField] Transform win;
    [SerializeField] Transform lose;
    
    private Button nextButton;
    private Button restartButton;

    [SerializeField] GameObject tutorial;
    [SerializeField] GameObject golds;
    [SerializeField] GameObject stackElements;
    [SerializeField] TextMeshProUGUI collectStackText;
    [SerializeField] TextMeshProUGUI totalStackText;
    [SerializeField] GameObject levels;

    private Vector3 stackTextDefaultPosition = Vector3.zero;
    
    void Start()
    {
        EventManager.current.onStartGame += OnStartGame;
        EventManager.current.onFinishGame += OnFinishGame;
        EventManager.current.onWinGame += OnWinGame;
        EventManager.current.onLoseGame += OnLoseGame;
        EventManager.current.onCollectStack += OnCollectStack;
        
        stackTextDefaultPosition = collectStackText.transform.localPosition;
        nextButton = win.GetChild(2).GetComponent<Button>();
        restartButton = lose.GetChild(2).GetComponent<Button>();
        UpdateLevelBar();
        UpdateGolds();
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
        tutorial.SetActive(false);
        golds.SetActive(false);
        stackElements.SetActive(true);
    }
    
    void OnFinishGame()
    {
        
    }
    
    void OnWinGame()
    {
        win.gameObject.SetActive(true);
        TextMeshProUGUI celebrationText = win.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI levelCompleteText = win.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        levelCompleteText.alpha = 0;
        
        celebrationText.transform.localPosition = new Vector3(transform.localPosition.x - 500, transform.position.y, transform.position.z);
        celebrationText.transform.DOMoveX(500, 1f).OnComplete(() =>
        {
            celebrationText.DOFade(0f, 1f).OnComplete(() =>
            {
                nextButton.transform.gameObject.SetActive(true);
                levelCompleteText.DOFade(100f, 1f).OnComplete(() =>
                {
                    EventManager.current.OnFinishGame();
                });
            });
        });
    }
    
    void OnLoseGame()
    {
        lose.gameObject.SetActive(true);
        TextMeshProUGUI consolationText = lose.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI tryAgainText = lose.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        tryAgainText.alpha = 0;
        
        consolationText.transform.gameObject.SetActive(true);
        consolationText.transform.localPosition = new Vector3(transform.localPosition.x - 500, transform.position.y, transform.position.z);
        consolationText.transform.DOMoveX(500, 1f).OnComplete(() =>
        {
            consolationText.DOFade(0f, 1f).OnComplete(() =>
            {
                restartButton.transform.gameObject.SetActive(true);

                tryAgainText.DOFade(100f, 1f).OnComplete(() =>
                {
                    EventManager.current.OnFinishGame();
                });
            });
        });
    }
    
    void OnCollectStack()
    {
        totalStackText.text = (int.Parse(totalStackText.text) + 1).ToString();
        
        DOTween.Kill("PlusOneMove");
        DOTween.Kill("PlusOneFade");
        collectStackText.gameObject.SetActive(true);
        collectStackText.transform.DOLocalMoveY(70f, 1f).OnComplete(()=>collectStackText.gameObject.SetActive(false)).SetId("PlusOneMove").SetRelative(true);
        collectStackText.DOFade(0f, 1f).SetId("PlusOneFade").OnKill(() =>
        {
            collectStackText.alpha = 1;
            collectStackText.transform.localPosition = stackTextDefaultPosition;
            collectStackText.gameObject.SetActive(false);
        }).OnComplete(() =>
        {
            collectStackText.alpha = 1;
            collectStackText.transform.localPosition = stackTextDefaultPosition;
            collectStackText.gameObject.SetActive(false);
        });
    }

    void UpdateLevelBar()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex + 1;
        if (currentLevel % 5 == 1)
        {
            for (int i = 0; i < levels.transform.childCount; i++)
            {
                levels.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentLevel + i).ToString();
            }

            levels.transform.GetChild(currentLevel - 1).GetComponent<Image>().color = Color.white;
        }
        
    }
    
    void UpdateGolds()
    {
        TextMeshProUGUI goldText = golds.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI bestText = golds.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        goldText.text = Random.Range(40, 120) + "G";
        bestText.text = "BEST:" + Random.Range(400, 600);
    }

    public void OnPressNextButton()
    {
        DOTween.KillAll();
        if (SceneManager.GetActiveScene().buildIndex + 1 <= SceneManager.sceneCount)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCount + 1));
        }
    }
    
    public void OnPressRestartButton()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
