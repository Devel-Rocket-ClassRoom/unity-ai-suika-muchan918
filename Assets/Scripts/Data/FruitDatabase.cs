using UnityEngine;

[CreateAssetMenu(fileName = "FruitDatabase", menuName = "Suika/FruitDatabase")]
public class FruitDatabase : ScriptableObject
{
    public FruitData[] fruits;

    public FruitData GetFruitByLevel(int level)
    {
        if (level < 0 || level >= fruits.Length) return null;
        return fruits[level];
    }

    public FruitData[] GetDroppableFruits()
    {
        return System.Array.FindAll(fruits, f => f.isDroppable);
    }
}
