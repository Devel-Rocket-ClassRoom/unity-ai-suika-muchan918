using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { Playing, GameOver }

    public static GameManager Instance { get; private set; }
    public static GameState State { get; private set; } = GameState.Playing;

    // Phase 7 UI가 구독
    public static event System.Action OnGameOverEvent;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        State = GameState.Playing;
    }

    void OnEnable()  => GameOverDetector.OnGameOver += HandleGameOver;
    void OnDisable() => GameOverDetector.OnGameOver -= HandleGameOver;

    void Update()
    {
        if (State == GameState.GameOver && Input.GetKeyDown(KeyCode.R))
            RestartGame();
    }

    private void HandleGameOver()
    {
        if (State == GameState.GameOver) return;
        State = GameState.GameOver;

        // 모든 과일 물리 정지
        foreach (var rb in FindObjectsByType<Rigidbody2D>(FindObjectsSortMode.None))
            rb.isKinematic = true;

        // DropController 비활성화
        var dc = FindFirstObjectByType<DropController>();
        if (dc != null) dc.enabled = false;

        Debug.Log("[GameManager] Game Over");
        OnGameOverEvent?.Invoke();
    }

    public void RestartGame()
    {
        State = GameState.Playing;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
