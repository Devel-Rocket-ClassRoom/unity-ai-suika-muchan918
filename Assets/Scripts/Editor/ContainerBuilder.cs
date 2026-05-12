using UnityEngine;
using UnityEditor;

public static class ContainerBuilder
{
    // Container dimensions (world units)
    public const float Width  = 6.5f;
    public const float Height = 9.0f;
    public const float WallThickness = 0.1f;

    [MenuItem("Suika/Build Container")]
    public static void BuildContainer()
    {
        var existing = GameObject.Find("Container");
        if (existing != null)
        {
            Undo.DestroyObjectImmediate(existing);
        }

        var mat = GetOrCreatePhysicsMaterial();

        var root = new GameObject("Container");
        Undo.RegisterCreatedObjectUndo(root, "Build Container");

        CreateWall(root, "BottomWall", mat,
            new Vector2(-Width / 2f, 0f),
            new Vector2( Width / 2f, 0f));

        CreateWall(root, "LeftWall", mat,
            new Vector2(-Width / 2f, 0f),
            new Vector2(-Width / 2f, Height));

        CreateWall(root, "RightWall", mat,
            new Vector2(Width / 2f, 0f),
            new Vector2(Width / 2f, Height));

        AddLineRenderer(root);

        Selection.activeGameObject = root;
        Debug.Log("[Suika] Container built. Menu: Suika > Build Container");
    }

    private static void CreateWall(GameObject parent, string wallName,
        PhysicsMaterial2D mat, Vector2 start, Vector2 end)
    {
        var go = new GameObject(wallName);
        go.transform.SetParent(parent.transform, false);

        var col = go.AddComponent<EdgeCollider2D>();
        col.points = new[] { start, end };
        col.sharedMaterial = mat;
    }

    private static void AddLineRenderer(GameObject parent)
    {
        var go = new GameObject("Visual");
        go.transform.SetParent(parent.transform, false);

        var lr = go.AddComponent<LineRenderer>();
        lr.positionCount = 4;
        lr.SetPositions(new Vector3[]
        {
            new(-Width / 2f, Height, 0f),   // top-left
            new(-Width / 2f, 0f,     0f),   // bottom-left
            new( Width / 2f, 0f,     0f),   // bottom-right
            new( Width / 2f, Height, 0f),   // top-right
        });

        lr.startWidth = 0.08f;
        lr.endWidth   = 0.08f;
        lr.loop       = false;
        lr.useWorldSpace = false;

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = new Color(0.6f, 0.4f, 0.2f);
        lr.endColor   = new Color(0.6f, 0.4f, 0.2f);
    }

    private static PhysicsMaterial2D GetOrCreatePhysicsMaterial()
    {
        const string path = "Assets/Data/FruitPhysics.physicsMaterial2D";
        var mat = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>(path);
        if (mat != null) return mat;

        mat = new PhysicsMaterial2D("FruitPhysics")
        {
            friction   = 0.4f,
            bounciness = 0.1f,
        };
        AssetDatabase.CreateAsset(mat, path);
        AssetDatabase.SaveAssets();
        return mat;
    }
}
