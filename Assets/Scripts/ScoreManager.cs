using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";

    public static int CurrentScore { get; private set; }
    public static int HighScore    { get; private set; }

    public static event System.Action<int> OnScoreChanged;
    public static event System.Action<int> OnHighScoreChanged;

    void Awake()
    {
        CurrentScore = 0;
        HighScore    = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    void OnEnable()  => MergeManager.OnScoreAdded += AddScore;
    void OnDisable() => MergeManager.OnScoreAdded -= AddScore;

    private void AddScore(int amount)
    {
        CurrentScore += amount;
        OnScoreChanged?.Invoke(CurrentScore);

        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
            OnHighScoreChanged?.Invoke(HighScore);
        }
    }

    public static void Reset()
    {
        CurrentScore = 0;
        OnScoreChanged?.Invoke(0);
    }
}
