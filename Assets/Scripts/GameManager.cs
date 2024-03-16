
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GAME_STATE {MENU, GAME, GAME_OVER};
    public static GAME_STATE gameState;
    // TODO: Eventually make a dont destroy thingy

    // Start is called before the first frame update
    void Start()
    {
        gameState = GAME_STATE.GAME;
    }

    public void LoadScene(string sceneName){
        gameState = GAME_STATE.GAME;
        SceneManager.LoadScene(sceneName);
    }

    public void Pause(){
        Time.timeScale = 0;
    }

    public void Resume(){
        Time.timeScale = 1;
    }
}
