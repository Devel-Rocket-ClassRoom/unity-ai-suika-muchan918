using UnityEngine;

[CreateAssetMenu(fileName = "FruitData", menuName = "Suika/FruitData")]
public class FruitData : ScriptableObject
{
    public int level;
    public string fruitName;
    public Sprite sprite;       // placeholder: null until real art is assigned
    public Color color = Color.white;
    public float radius;
    public int mergeScore;
    public bool isDroppable;
}
