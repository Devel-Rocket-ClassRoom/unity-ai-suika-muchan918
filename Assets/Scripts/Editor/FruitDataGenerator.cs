using UnityEngine;
using UnityEditor;

public static class FruitDataGenerator
{
    private static readonly (string name, Color color, float radius, int score, bool droppable)[] FruitTable =
    {
        ("Cherry",      new Color(0.80f, 0.05f, 0.05f), 0.15f,  2, true),
        ("Strawberry",  new Color(0.95f, 0.20f, 0.20f), 0.20f,  3, true),
        ("Grape",       new Color(0.55f, 0.10f, 0.75f), 0.27f,  4, true),
        ("Dekopon",     new Color(1.00f, 0.55f, 0.00f), 0.35f,  5, true),
        ("Persimmon",   new Color(0.90f, 0.27f, 0.00f), 0.43f,  6, true),
        ("Apple",       new Color(0.95f, 0.40f, 0.40f), 0.52f,  7, false),
        ("Pear",        new Color(0.80f, 0.80f, 0.10f), 0.62f,  8, false),
        ("Peach",       new Color(1.00f, 0.70f, 0.65f), 0.73f,  9, false),
        ("Pineapple",   new Color(1.00f, 0.90f, 0.00f), 0.85f, 10, false),
        ("Melon",       new Color(0.60f, 0.85f, 0.35f), 0.95f, 11, false),
        ("Watermelon",  new Color(0.05f, 0.50f, 0.15f), 1.10f,  0, false),
    };

    [MenuItem("Suika/Generate Fruit Assets")]
    public static void GenerateFruitAssets()
    {
        const string fruitDir = "Assets/Data/Fruits";
        const string dbPath   = "Assets/Data/FruitDatabase.asset";

        if (!AssetDatabase.IsValidFolder("Assets/Data"))
            AssetDatabase.CreateFolder("Assets", "Data");
        if (!AssetDatabase.IsValidFolder(fruitDir))
            AssetDatabase.CreateFolder("Assets/Data", "Fruits");

        var fruitAssets = new FruitData[FruitTable.Length];

        for (int i = 0; i < FruitTable.Length; i++)
        {
            var (fruitName, color, radius, score, droppable) = FruitTable[i];
            string path = $"{fruitDir}/{fruitName}.asset";

            var data = AssetDatabase.LoadAssetAtPath<FruitData>(path)
                       ?? ScriptableObject.CreateInstance<FruitData>();

            data.level       = i;
            data.fruitName   = fruitName;
            data.sprite      = LoadSprite(fruitName);
            data.color       = color;
            data.radius      = radius;
            data.mergeScore  = score;
            data.isDroppable = droppable;

            if (AssetDatabase.LoadAssetAtPath<FruitData>(path) == null)
                AssetDatabase.CreateAsset(data, path);
            else
                EditorUtility.SetDirty(data);

            fruitAssets[i] = data;
        }

        var db = AssetDatabase.LoadAssetAtPath<FruitDatabase>(dbPath)
                 ?? ScriptableObject.CreateInstance<FruitDatabase>();
        db.fruits = fruitAssets;

        if (AssetDatabase.LoadAssetAtPath<FruitDatabase>(dbPath) == null)
            AssetDatabase.CreateAsset(db, dbPath);
        else
            EditorUtility.SetDirty(db);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Suika] Fruit assets generated. Menu: Suika > Generate Fruit Assets");
    }

    private static Sprite LoadSprite(string fruitName)
    {
        // Try "FruitName 1.png" first, then "FruitName.png"
        string[] candidates = {
            $"Assets/Sprites/{fruitName} 1.png",
            $"Assets/Sprites/{fruitName}.png",
        };
        foreach (var path in candidates)
        {
            var s = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (s != null) return s;
        }
        return null;
    }
}
