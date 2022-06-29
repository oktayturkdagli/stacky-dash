using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    [SerializeField] SOUserData userData;
    
    [SerializeField] GameObject levels;
    [SerializeField] GameObject golds;
    [SerializeField] Transform stackElements;
    [SerializeField] GameObject tutorial;
    TextMeshProUGUI totalStackText;
    TextMeshProUGUI collectStackText;
    
    [SerializeField] Transform win;
    [SerializeField] Transform lose;
    private Button nextButton;
    private Button restartButton;

    void Start()
    {
        EventManager.current.onStartGame += OnStartGame;
        EventManager.current.onFinishGame += OnFinishGame;
        EventManager.current.onWinGame += OnWinGame;
        EventManager.current.onLoseGame += OnLoseGame;
        EventManager.current.onCollectStack += OnCollectStack;

        totalStackText = stackElements.GetChild(0).GetComponent<TextMeshProUGUI>();
        collectStackText = stackElements.GetChild(1).GetComponent<TextMeshProUGUI>();
        nextButton = win.GetChild(2).GetComponent<Button>();
        restartButton = lose.GetChild(2).GetComponent<Button>();
        
        userData.totalStack = 0;
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
        stackElements.gameObject.SetActive(true);
    }
    
    void OnFinishGame()
    {
        
    }
    
    void OnWinGame()
    {
        userData.currentLevel += 1;
        userData.totalGold += userData.totalStack;
        userData.totalStack = 0;
        if (userData.totalGold > userData.bestTotalGold)
            userData.bestTotalGold = userData.totalGold;

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
        userData.totalStack += 1;
        totalStackText.text = userData.totalStack.ToString();
        DOTween.Kill("PlusOneMove");
        DOTween.Kill("PlusOneFade");
        collectStackText.gameObject.SetActive(true);
        collectStackText.transform.localPosition = new Vector3(0,-150,0);
        collectStackText.transform.DOLocalMoveY(70f, 1f).OnComplete(()=>collectStackText.gameObject.SetActive(false)).SetId("PlusOneMove").SetRelative(true);
        collectStackText.DOFade(0f, 1f).SetId("PlusOneFade").OnKill(() =>
        {
            collectStackText.alpha = 1;
            collectStackText.gameObject.SetActive(false);
        }).OnComplete(() =>
        {
            collectStackText.alpha = 1;
            collectStackText.gameObject.SetActive(false);
        });
    }

    void UpdateLevelBar()
    {
        int currentLevel = userData.currentLevel;
        int startcount = 0;
        int offset = currentLevel % 5;
        switch (offset)
        {
            case 0:
                startcount = currentLevel - 4;
                break;
            case 1:
                startcount = currentLevel;
                break;
            case 2: case 3: case 4:
                startcount = currentLevel - (offset - 1);
                break;
        }

        for (int i = 0; i < 5; i++)
        {
            levels.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = (startcount + i).ToString();
            if (startcount + i == currentLevel)
            {
                levels.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            }
        }
    }
    
    void UpdateGolds()
    {
        TextMeshProUGUI goldText = golds.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI bestText = golds.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        goldText.text = userData.totalGold + "G";
        bestText.text = "BEST: " + userData.bestTotalGold;
    }

    public void OnPressNextButton()
    {
        int maxLevel = 0;
        if (userData.highestLevel <= SceneManager.sceneCountInBuildSettings)
            maxLevel = userData.highestLevel;
        else
            maxLevel = SceneManager.sceneCountInBuildSettings;
        
        DOTween.KillAll();
        if (userData.currentLevel < maxLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(Random.Range(0, maxLevel));
        }
    }
    
    public void OnPressRestartButton()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
