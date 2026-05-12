using System.Collections;
using UnityEngine;

public class DropController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FruitDatabase fruitDatabase;

    [Header("Container Bounds")]
    [SerializeField] private float minX     = -3.0f;
    [SerializeField] private float maxX     =  3.0f;
    [SerializeField] private float dropY    =  9.5f;

    [Header("Feel")]
    [SerializeField] private float slideSpeed    = 14f;   // 미리보기 슬라이드 속도
    [SerializeField] private float dropCooldown  = 0.7f;
    [SerializeField] private float momentumScale = 0.12f; // 마우스 속도 → 수평 관성 배율

    [Header("Next Preview")]
    [SerializeField] private Transform nextPreviewAnchor;  // 다음 과일 표시 위치

    private FruitData _currentData;
    private FruitData _nextData;

    private GameObject   _previewGo;
    private SpriteRenderer _previewSr;
    private LineRenderer   _dropLine;

    private GameObject   _nextPreviewGo;

    private float _currentX;
    private float _targetX;
    private float _prevTargetX;
    private float _mouseVelocityX;

    private bool _canDrop = true;

    void Start()
    {
        PickNextData();
        ShowCurrentPreview();
    }

    void Update()
    {
        UpdateMouseTracking();
        UpdatePreviewPosition();

        if (_canDrop && Input.GetMouseButtonDown(0))
            StartCoroutine(DoDrop());
    }

    // ── 마우스 추적 ─────────────────────────────────────────
    private void UpdateMouseTracking()
    {
        Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _prevTargetX      = _targetX;
        _targetX          = Mathf.Clamp(world.x, minX, maxX);
        _mouseVelocityX   = (_targetX - _prevTargetX) / Mathf.Max(Time.deltaTime, 0.001f);
    }

    private void UpdatePreviewPosition()
    {
        if (_previewGo == null) return;
        _currentX = Mathf.Lerp(_currentX, _targetX, Time.deltaTime * slideSpeed);
        _previewGo.transform.position = new Vector3(_currentX, dropY, 0f);
        UpdateDropLine();
    }

    // ── 드롭 ────────────────────────────────────────────────
    private IEnumerator DoDrop()
    {
        _canDrop = false;

        float dropX = _currentX;
        float hVel  = _mouseVelocityX * momentumScale;

        DestroyPreview();

        var go = Instantiate(_currentData.prefab, new Vector3(dropX, dropY, 0f), Quaternion.identity);
        go.GetComponent<Fruit>().Init(_currentData);
        go.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(hVel, 0f);
        GameOverDetector.IgnoreUntilTime = Time.time + 1f;

        yield return new WaitForSeconds(dropCooldown);

        PickNextData();
        ShowCurrentPreview();
        _canDrop = true;
    }

    // ── 과일 선택 ────────────────────────────────────────────
    private void PickNextData()
    {
        var pool = fruitDatabase.GetDroppableFruits();
        _currentData = _nextData ?? pool[Random.Range(0, pool.Length)];
        _nextData    = pool[Random.Range(0, pool.Length)];
        RefreshNextPreview();
    }

    // ── 현재 과일 미리보기 ────────────────────────────────────
    private void ShowCurrentPreview()
    {
        DestroyPreview();

        _previewGo = new GameObject("DropPreview");
        _previewSr = _previewGo.AddComponent<SpriteRenderer>();
        _previewSr.sortingOrder = 10;
        _previewSr.sprite = _currentData.sprite;

        Color c = _currentData.sprite != null ? Color.white : _currentData.color;
        _previewSr.color = new Color(c.r, c.g, c.b, 0.65f);

        float diameter = _currentData.radius * 2f;
        _previewGo.transform.localScale = new Vector3(diameter, diameter, 1f);
        _currentX = 0f;
        _previewGo.transform.position = new Vector3(_currentX, dropY, 0f);

        // 드롭 라인
        _dropLine = _previewGo.AddComponent<LineRenderer>();
        _dropLine.positionCount  = 2;
        _dropLine.startWidth     = 0.04f;
        _dropLine.endWidth       = 0.04f;
        _dropLine.useWorldSpace  = true;
        _dropLine.material       = new Material(Shader.Find("Sprites/Default"));
        _dropLine.startColor     = new Color(1f, 1f, 1f, 0.3f);
        _dropLine.endColor       = new Color(1f, 1f, 1f, 0f);
        UpdateDropLine();
    }

    private void UpdateDropLine()
    {
        if (_dropLine == null) return;
        _dropLine.SetPosition(0, new Vector3(_currentX, dropY - _currentData.radius, 0f));
        _dropLine.SetPosition(1, new Vector3(_currentX, 0f, 0f));
    }

    private void DestroyPreview()
    {
        if (_previewGo != null) Destroy(_previewGo);
        _previewGo = null;
        _previewSr = null;
        _dropLine  = null;
    }

    // ── 다음 과일 미리보기 ────────────────────────────────────
    private void RefreshNextPreview()
    {
        if (_nextPreviewGo != null) Destroy(_nextPreviewGo);
        if (nextPreviewAnchor == null) return;

        _nextPreviewGo = new GameObject("NextPreview");
        _nextPreviewGo.transform.SetParent(nextPreviewAnchor, false);

        var sr = _nextPreviewGo.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 10;
        sr.sprite = _nextData.sprite;
        Color c = _nextData.sprite != null ? Color.white : _nextData.color;
        sr.color = c;

        float diameter = _nextData.radius * 2f;
        _nextPreviewGo.transform.localScale = new Vector3(diameter, diameter, 1f);
    }
}
