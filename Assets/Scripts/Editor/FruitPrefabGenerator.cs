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

            go.GetComponent<Fruit>().data = fruitData;

            // 콜라이더: radius 직접 설정, 부모 scale은 (1,1,1) 유지
            var col = go.GetComponent<CircleCollider2D>();
            col.radius         = fruitData.radius;
            col.sharedMaterial = mat;

            // Visual 자식의 SpriteRenderer와 크기 설정
            var visual = go.transform.Find("Visual");
            if (visual != null)
            {
                var sr = visual.GetComponent<SpriteRenderer>();
                sr.sprite = fruitData.sprite;
                sr.color  = fruitData.sprite != null ? Color.white : fruitData.color;

                // 초기 비주얼 크기 = 콜라이더 지름 (이후 Inspector에서 독립 조작 가능)
                float diameter = fruitData.radius * 2f;
                visual.localScale = new Vector3(diameter, diameter, 1f);
            }

            var saved = PrefabUtility.SaveAsPrefabAsset(go, path);
            Object.DestroyImmediate(go);

            fruitData.prefab = saved;
            EditorUtility.SetDirty(fruitData);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Suika] Fruit prefabs generated in " + prefabDir);
    }
}
