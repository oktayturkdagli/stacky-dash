using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isFirstSwipe = true;
    
    void Start()
    {
        EventManager.current.onFinishGame += OnFinishGame;
        EventManager.current.onWinGame += OnWinGame;
        EventManager.current.onLoseGame += OnLoseGame;
    }
    
    void OnDestroy()
    {
        EventManager.current.onFinishGame -= OnFinishGame;
        EventManager.current.onWinGame -= OnWinGame;
        EventManager.current.onLoseGame -= OnLoseGame;
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

    public void OnSwipe(Vector2 screenDelta)
    {
        if (isFirstSwipe)
        {
            isFirstSwipe = false;
            EventManager.current.OnStartGame();
            return;
        }
        
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
