using UnityEngine;
using UnityEditor;

public static class FruitPrefabGenerator
{
    [MenuItem("Suika/Generate Fruit Prefabs")]
    public static void GenerateFruitPrefabs()
    {
        const string prefabDir  = "Assets/Prefabs/Fruits";
        const string basePath   = "Assets/Prefabs/Fruit.prefab";
        const string dbPath     = "Assets/Data/FruitDatabase.asset";

        var basePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(basePath);
        if (basePrefab == null) { Debug.LogError("[Suika] Base prefab not found: " + basePath); return; }

        var db = AssetDatabase.LoadAssetAtPath<FruitDatabase>(dbPath);
        if (db == null) { Debug.LogError("[Suika] FruitDatabase not found: " + dbPath); return; }

        var mat = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>("Assets/Data/FruitPhysics.physicsMaterial2D");

        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        if (!AssetDatabase.IsValidFolder(prefabDir))
            AssetDatabase.CreateFolder("Assets/Prefabs", "Fruits");

        foreach (var fruitData in db.fruits)
        {
            string path = $"{prefabDir}/{fruitData.fruitName}.prefab";

            var go = (GameObject)PrefabUtility.InstantiatePrefab(basePrefab);
            go.name = fruitData.fruitName;

            var fruit = go.GetComponent<Fruit>();
            fruit.data = fruitData;

            var sr = go.GetComponent<SpriteRenderer>();
            sr.sprite = fruitData.sprite;
            sr.color  = fruitData.sprite != null ? Color.white : fruitData.color;

            var col = go.GetComponent<CircleCollider2D>();
            col.radius         = 0.5f;   // Fruit.Init()이 localScale로 크기 결정
            col.sharedMaterial = mat;

            var saved = PrefabUtility.SaveAsPrefabAsset(go, path);
            Object.DestroyImmediate(go);

            // FruitData.prefab 필드에 생성된 프리팹 연결
            fruitData.prefab = saved;
            EditorUtility.SetDirty(fruitData);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Suika] Fruit prefabs generated in " + prefabDir);
    }
}
