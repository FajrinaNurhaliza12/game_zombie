using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Settings")]
    [SerializeField] private int zombiesToWin = 10;

    private int coins = 0;
    private int zombiesKilled = 0;
    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void OnZombieKilled(int reward)
    {
        if (gameEnded) return;

        zombiesKilled++;

        Debug.Log("Zombie berhasil dikalahkan!");

        AddCoins(reward);

        Debug.Log("Zombie dikalahkan: " + zombiesKilled + "/" + zombiesToWin);

        if (zombiesKilled >= zombiesToWin)
        {
            WinGame();
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;

        Debug.Log("Coin berhasil diklaim! Total coin: " + coins);
    }

    void WinGame()
    {
        if (gameEnded) return;

        gameEnded = true;

        Debug.Log("MENANG!");

        EndGame();
    }

    void OnPlayerDeath()
    {
        if (gameEnded) return;

        gameEnded = true;

        Debug.Log("GAME OVER!");

        EndGame();
    }

    void EndGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}