using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        EventManager.current.onStartGame += OnStartGame;
        EventManager.current.onFinishGame += OnFinishGame;
        EventManager.current.onWinGame += OnWinGame;
        EventManager.current.onLoseGame += OnLoseGame;
        EventManager.current.OnStartGame();
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
        // Debug.Log("Game is START!");
        Invoke(nameof(LateStart), 1f); // Game starts 1 second late to wait for all classes to load correctly
    }

    void OnFinishGame()
    {
        // Debug.Log("Game is OVER!");
    }
    
    void OnWinGame()
    {
        
    }
    
    void OnLoseGame()
    {
        
    }
    
    void LateStart()
    {
        
    }
    
    public void OnSwipe(Vector2 screenDelta)
    {
        float x = screenDelta.x;
        float y = screenDelta.y;
        
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            // Left or Right
            if (x < 0)
                EventManager.current.OnSwipe("Left");
            else
            {
                EventManager.current.OnSwipe("Right");
            }
        }
        else
        {
            //Up or Down
            if (y < 0)
            {
                EventManager.current.OnSwipe("Down");
            }
            else
            {
                EventManager.current.OnSwipe("Up");
            }
        }
        
    }

    
}
