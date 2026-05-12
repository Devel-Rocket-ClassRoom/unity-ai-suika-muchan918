using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Fruit : MonoBehaviour
{
    public FruitData data;

    [HideInInspector] public bool isMerging;

    private SpriteRenderer _sr;
    private CircleCollider2D _col;

    void Awake()
    {
        _sr  = GetComponentInChildren<SpriteRenderer>();
        _col = GetComponent<CircleCollider2D>();
    }

    public void Init(FruitData fruitData)
    {
        data = fruitData;
        _sr.sprite = data.sprite;
        _sr.color  = data.sprite != null ? Color.white : data.color;

        // 콜라이더 반지름과 Visual 크기는 프리팹에 저장된 값을 그대로 사용
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (isMerging) return;

        var other = col.gameObject.GetComponent<Fruit>();
        if (other == null || other.isMerging) return;
        if (other.data == null || other.data.level != data.level) return;

        isMerging       = true;
        other.isMerging = true;

        MergeRequested?.Invoke(this, other);
    }

    public static event System.Action<Fruit, Fruit> MergeRequested;
}
