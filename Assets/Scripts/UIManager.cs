using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite panelSprite;
    [SerializeField] private Sprite buttonSprite;
    [SerializeField] private Sprite backgroundSprite;

    private TextMeshProUGUI _scoreTxt;
    private TextMeshProUGUI _highScoreTxt;
    private GameObject      _gameOverPanel;
    private TextMeshProUGUI _finalScoreTxt;
    private TextMeshProUGUI _newRecordTxt;

    void Awake()
    {
        BuildUI();
    }

    void OnEnable()
    {
        ScoreManager.OnScoreChanged    += UpdateScore;
        ScoreManager.OnHighScoreChanged += UpdateHighScore;
        GameManager.OnGameOverEvent    += ShowGameOver;
    }

    void OnDisable()
    {
        ScoreManager.OnScoreChanged    -= UpdateScore;
        ScoreManager.OnHighScoreChanged -= UpdateHighScore;
        GameManager.OnGameOverEvent    -= ShowGameOver;
    }

    // ── UI 생성 ──────────────────────────────────────────────
    private void BuildUI()
    {
        var canvas = CreateCanvas();
        AddBackground(canvas);
        AddScorePanel(canvas);
        AddGameOverPanel(canvas);

        _gameOverPanel.SetActive(false);
        UpdateScore(ScoreManager.CurrentScore);
        UpdateHighScore(ScoreManager.HighScore);
    }

    private Canvas CreateCanvas()
    {
        var go = new GameObject("Canvas");
        go.transform.SetParent(transform);
        var c = go.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        c.sortingOrder = 100;
        go.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        go.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1080, 1920);
        go.AddComponent<GraphicRaycaster>();
        return c;
    }

    private void AddBackground(Canvas canvas)
    {
        if (backgroundSprite == null) return;
        var go = new GameObject("Background");
        go.transform.SetParent(canvas.transform, false);
        var img = go.AddComponent<Image>();
        img.sprite = backgroundSprite;
        img.type = Image.Type.Simple;
        img.SetNativeSize();
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;
        go.transform.SetAsFirstSibling();
    }

    private void AddScorePanel(Canvas canvas)
    {
        // 패널
        var panel = MakeImage("ScorePanel", canvas.transform, panelSprite);
        var prt = panel.GetComponent<RectTransform>();
        prt.anchorMin = new Vector2(0.5f, 1f);
        prt.anchorMax = new Vector2(0.5f, 1f);
        prt.pivot     = new Vector2(0.5f, 1f);
        prt.sizeDelta = new Vector2(400f, 140f);
        prt.anchoredPosition = new Vector2(0f, -20f);

        // 최고점수
        _highScoreTxt = MakeText("HighScore", panel.transform, "최고: 0", 28);
        var hrt = _highScoreTxt.GetComponent<RectTransform>();
        hrt.anchorMin = hrt.anchorMax = new Vector2(0.5f, 0.75f);
        hrt.anchoredPosition = Vector2.zero;
        hrt.sizeDelta = new Vector2(360f, 40f);

        // 현재점수
        _scoreTxt = MakeText("Score", panel.transform, "0", 52);
        _scoreTxt.fontStyle = FontStyles.Bold;
        var srt = _scoreTxt.GetComponent<RectTransform>();
        srt.anchorMin = srt.anchorMax = new Vector2(0.5f, 0.3f);
        srt.anchoredPosition = Vector2.zero;
        srt.sizeDelta = new Vector2(360f, 60f);
    }

    private void AddGameOverPanel(Canvas canvas)
    {
        _gameOverPanel = MakeImage("GameOverPanel", canvas.transform, panelSprite).gameObject;
        var prt = _gameOverPanel.GetComponent<RectTransform>();
        prt.anchorMin = new Vector2(0.5f, 0.5f);
        prt.anchorMax = new Vector2(0.5f, 0.5f);
        prt.pivot     = new Vector2(0.5f, 0.5f);
        prt.sizeDelta = new Vector2(500f, 480f);
        prt.anchoredPosition = Vector2.zero;

        var t = _gameOverPanel.transform;

        MakeText("Title", t, "GAME OVER", 56).GetComponent<RectTransform>()
            .SetAnchored(0.5f, 0.78f, new Vector2(440f, 70f));

        _finalScoreTxt = MakeText("FinalScore", t, "0점", 44);
        _finalScoreTxt.GetComponent<RectTransform>()
            .SetAnchored(0.5f, 0.58f, new Vector2(440f, 60f));

        _newRecordTxt = MakeText("NewRecord", t, "🎉 신기록!", 32);
        _newRecordTxt.color = new Color(1f, 0.85f, 0f);
        _newRecordTxt.GetComponent<RectTransform>()
            .SetAnchored(0.5f, 0.45f, new Vector2(440f, 44f));
        _newRecordTxt.gameObject.SetActive(false);

        MakeText("HighScoreLabel", t, "", 28)
            .GetComponent<RectTransform>()
            .SetAnchored(0.5f, 0.35f, new Vector2(440f, 38f));
        UpdateHighScoreLabel();

        var btn = MakeButton("RestartBtn", t, buttonSprite, "다시 시작");
        btn.GetComponent<RectTransform>().SetAnchored(0.5f, 0.15f, new Vector2(280f, 70f));
        btn.GetComponent<Button>().onClick.AddListener(() =>
            FindFirstObjectByType<GameManager>()?.RestartGame());
    }

    // ── 이벤트 핸들러 ────────────────────────────────────────
    private void UpdateScore(int score)
    {
        if (_scoreTxt != null) _scoreTxt.text = score.ToString();
    }

    private void UpdateHighScore(int hs)
    {
        if (_highScoreTxt != null) _highScoreTxt.text = $"최고: {hs}";
        UpdateHighScoreLabel();
    }

    private void UpdateHighScoreLabel()
    {
        if (_gameOverPanel == null) return;
        var lbl = _gameOverPanel.transform.Find("HighScoreLabel")?.GetComponent<TextMeshProUGUI>();
        if (lbl != null) lbl.text = $"최고 점수: {ScoreManager.HighScore}";
    }

    private void ShowGameOver()
    {
        if (_gameOverPanel == null) return;
        _gameOverPanel.SetActive(true);

        if (_finalScoreTxt != null)
            _finalScoreTxt.text = $"{ScoreManager.CurrentScore}점";

        bool isNew = ScoreManager.CurrentScore >= ScoreManager.HighScore
                     && ScoreManager.CurrentScore > 0;
        if (_newRecordTxt != null)
            _newRecordTxt.gameObject.SetActive(isNew);

        UpdateHighScoreLabel();
    }

    // ── 헬퍼 ─────────────────────────────────────────────────
    private static Image MakeImage(string name, Transform parent, Sprite sprite)
    {
        var go  = new GameObject(name);
        go.transform.SetParent(parent, false);
        var img = go.AddComponent<Image>();
        if (sprite != null)
        {
            img.sprite = sprite;
            img.type   = Image.Type.Sliced;
        }
        else
        {
            img.color = new Color(0f, 0f, 0f, 0.75f);
        }
        return img;
    }

    private static TextMeshProUGUI MakeText(string name, Transform parent, string text, int size)
    {
        var go  = new GameObject(name);
        go.transform.SetParent(parent, false);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text      = text;
        tmp.fontSize  = size;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color     = Color.white;
        return tmp;
    }

    private static GameObject MakeButton(string name, Transform parent, Sprite sprite, string label)
    {
        var img = MakeImage(name, parent, sprite);
        var btn = img.gameObject.AddComponent<Button>();
        btn.targetGraphic = img;
        var lbl = MakeText("Label", img.transform, label, 32);
        var rt  = lbl.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;
        return img.gameObject;
    }
}

// RectTransform 편의 확장
public static class RectTransformExt
{
    public static void SetAnchored(this RectTransform rt, float ax, float ay, Vector2 size)
    {
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(ax, ay);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = size;
    }
}
