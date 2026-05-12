using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static event System.Action<int> OnScoreAdded;

    private const int WatermelonLevel = 10;
    private const int WatermelonBonusScore = 100;

    [SerializeField] private FruitDatabase fruitDatabase;

    void OnEnable()  => Fruit.MergeRequested += HandleMerge;
    void OnDisable() => Fruit.MergeRequested -= HandleMerge;

    private void HandleMerge(Fruit a, Fruit b)
    {
        Vector2 midPoint = (a.transform.position + b.transform.position) / 2f;
        int level = a.data.level;

        Destroy(a.gameObject);
        Destroy(b.gameObject);

        if (level >= WatermelonLevel)
        {
            // 수박 + 수박: 둘 다 소멸, 보너스 점수
            OnScoreAdded?.Invoke(WatermelonBonusScore);
            return;
        }

        var nextData = fruitDatabase.GetFruitByLevel(level + 1);
        if (nextData == null || nextData.prefab == null)
        {
            Debug.LogWarning($"[MergeManager] No prefab for level {level + 1}");
            return;
        }

        var next = Instantiate(nextData.prefab, midPoint, Quaternion.identity);
        next.GetComponent<Fruit>().Init(nextData);

        OnScoreAdded?.Invoke(nextData.mergeScore);
    }
}
