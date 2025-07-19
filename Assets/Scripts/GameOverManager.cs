using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;
    private bool isGameOver = false;

    public void ShowGameOverMenu()
    {
        if (gameOverMenu != null)
            gameOverMenu.SetActive(true);
        isGameOver = true;
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
