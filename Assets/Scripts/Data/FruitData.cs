using UnityEngine;

[CreateAssetMenu(fileName = "FruitData", menuName = "Suika/FruitData")]
public class FruitData : ScriptableObject
{
    public int level;
    public string fruitName;
    public Sprite sprite;
    public Color color = Color.white;
    public float radius;
    public int mergeScore;
    public bool isDroppable;
    public GameObject prefab;
}
