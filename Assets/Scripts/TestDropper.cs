using UnityEngine;

public class TestDropper : MonoBehaviour
{
    [SerializeField] private FruitDatabase fruitDatabase;
    [SerializeField] private float dropX = 0f;
    [SerializeField] private float dropY = 9.5f;
    [SerializeField] private float cooldown = 0.5f;

    private float _nextDropTime;

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        if (Time.time < _nextDropTime) return;

        Drop();
        _nextDropTime = Time.time + cooldown;
    }

    private void Drop()
    {
        var droppable = fruitDatabase.GetDroppableFruits();
        if (droppable == null || droppable.Length == 0) return;

        var data = droppable[Random.Range(0, droppable.Length)];
        if (data.prefab == null)
        {
            Debug.LogWarning($"[TestDropper] {data.fruitName} has no prefab assigned.");
            return;
        }

        var go = Instantiate(data.prefab, new Vector3(dropX, dropY, 0f), Quaternion.identity);
        go.GetComponent<Fruit>().Init(data);
    }
}
