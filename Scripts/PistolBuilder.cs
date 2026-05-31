using UnityEngine;

public class PistolBuilder : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0.22f, -0.2f, 0.38f);
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 30f;

    void Start()
    {
        GameObject gun = BuildPistol();

        // Buat BulletSpawnPoint di ujung laras
        GameObject spawnPoint = new GameObject("BulletSpawnPoint");
        spawnPoint.transform.SetParent(gun.transform);
        spawnPoint.transform.localPosition = new Vector3(0f, 0.01f, 0.2f);
        spawnPoint.transform.localRotation = Quaternion.identity;

        GunController gc = gun.AddComponent<GunController>();
        gc.Init(GetComponent<Camera>());
        gc.SetBullet(bulletPrefab, spawnPoint.transform, bulletSpeed);
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 28;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(
            new Rect(Screen.width / 2 - 10, Screen.height / 2 - 20, 20, 40),
            "+",
            style
        );
    }

    GameObject BuildPistol()
    {
        GameObject root = new GameObject("Pistol");
        root.transform.SetParent(transform);
        root.transform.localPosition = offset;
        root.transform.localRotation = Quaternion.identity;

        // Bagian-bagian pistol
        MakePart(
            "Body",
            root,
            new Vector3(0f, 0f, 0f),
            new Vector3(0.06f, 0.08f, 0.14f),
            new Color(0.15f, 0.15f, 0.15f) // abu gelap
        );
        MakePart(
            "Barrel",
            root,
            new Vector3(0f, 0.01f, 0.13f),
            new Vector3(0.03f, 0.03f, 0.1f),
            new Color(0.10f, 0.10f, 0.10f) // hitam
        );
        GameObject grip = MakePart(
            "Grip",
            root,
            new Vector3(0f, -0.09f, 0.01f),
            new Vector3(0.05f, 0.10f, 0.055f),
            new Color(0.08f, 0.08f, 0.08f) // hitam pekat
        );
        grip.transform.localRotation = Quaternion.Euler(12f, 0, 0);
        MakePart(
            "Sight",
            root,
            new Vector3(0f, 0.048f, 0.03f),
            new Vector3(0.008f, 0.012f, 0.015f),
            new Color(0.2f, 0.2f, 0.2f) // abu
        );

        return root;
    }

    GameObject MakePart(
        string partName,
        GameObject parent,
        Vector3 localPos,
        Vector3 scale,
        Color color)
    {
        GameObject p = GameObject.CreatePrimitive(PrimitiveType.Cube);
        p.name = partName;
        p.transform.SetParent(parent.transform);
        p.transform.localPosition = localPos;
        p.transform.localScale = scale;
        p.layer = LayerMask.NameToLayer("Gun");

        // Hapus collider — pistol tidak perlu collision
        Destroy(p.GetComponent<Collider>());

        // Material metalik
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        mat.SetFloat("_Metallic", 0.9f);
        mat.SetFloat("_Glossiness", 0.5f);
        p.GetComponent<Renderer>().material = mat;

        return p;
    }
}