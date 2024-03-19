
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GAME_STATE {MENU, GAME, PAUSE, GAME_OVER};
    public static GAME_STATE gameState;
    // TODO: Eventually make a dont destroy thingy

    // Start is called before the first frame update
    void Start()
    {
        gameState = GAME_STATE.MENU;
        if(SceneManager.GetActiveScene().name.Equals("Game"))
            gameState = GAME_STATE.GAME;
    
    }

    public void LoadScene(string sceneName){
        if(sceneName.Equals("Game"))
            gameState = GAME_STATE.GAME;
        SceneManager.LoadScene(sceneName);
    }

    public void Pause(){
        gameState = GAME_STATE.PAUSE;
        Time.timeScale = 0;
    }

    public void Resume(){
        gameState = GAME_STATE.GAME;
        Time.timeScale = 1;
    }

    public void Quit(){
        Application.Quit();
    }
}
