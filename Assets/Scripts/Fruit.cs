using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Fruit : MonoBehaviour
{
    public FruitData data;

    [HideInInspector] public bool isMerging;

    private SpriteRenderer _sr;
    private CircleCollider2D _col;

    void Awake()
    {
        _sr  = GetComponent<SpriteRenderer>();
        _col = GetComponent<CircleCollider2D>();
    }

    public void Init(FruitData fruitData)
    {
        data = fruitData;
        _sr.sprite = data.sprite;
        _sr.color  = data.sprite != null ? Color.white : data.color;

        // 스프라이트는 1px/unit 기준 월드 크기 1.0 → localScale로 크기 맞춤
        float diameter = data.radius * 2f;
        transform.localScale = new Vector3(diameter, diameter, 1f);
        _col.radius = 0.5f;   // 로컬 반지름 0.5 × scale = data.radius
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (isMerging) return;

        var other = col.gameObject.GetComponent<Fruit>();
        if (other == null || other.isMerging) return;
        if (other.data == null || other.data.level != data.level) return;

        isMerging       = true;
        other.isMerging = true;

        // MergeManager(Phase 3)가 실제 스폰을 담당. 여기선 이벤트만 발행.
        MergeRequested?.Invoke(this, other);
    }

    // Phase 3의 MergeManager가 구독
    public static event System.Action<Fruit, Fruit> MergeRequested;
}
