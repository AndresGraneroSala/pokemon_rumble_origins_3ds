using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    static public GameManager instance;
    [SerializeField] private GameObject menuEndGame;
    private int playerSpeed = 1;

    public int PlayerSpeed {
        get { return playerSpeed; }
    }

    private void Awake() {
        instance = this;
        ResumeGame();
    }

    public void StopPlayer() {
        playerSpeed = 0;
    }
    
    public void MovePlayer() {
        playerSpeed = 1;
    }

    private void Start() {
        menuEndGame.SetActive(false);
    }

    public void EndGame() {
        menuEndGame.SetActive(true);
        PauseGame();
    }

    public void PauseGame() {
        Time.timeScale = 0;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
    }
    
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}