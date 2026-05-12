using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private Text scoreValueText;

    private int _score;
    public int Score => _score;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void OnEnable()  => MergeManager.OnScoreAdded += AddScore;
    void OnDisable() => MergeManager.OnScoreAdded -= AddScore;

    private void AddScore(int points)
    {
        _score += points;
        if (scoreValueText != null)
            scoreValueText.text = _score.ToString();
    }
}
