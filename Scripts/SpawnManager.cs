using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [Header("Zombie Spawn")]
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform[] zombieSpawnPoints;
    [SerializeField] private int totalZombies = 12;
    [SerializeField] private float spawnInterval = 5f;

    [Header("Item Spawn")]
    [SerializeField] private GameObject medkitPrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform[] itemSpawnPoints;
    [SerializeField] private float itemInterval = 8f;
    [SerializeField] private int maxItems = 4;

    private int spawned = 0;
    private int itemCount = 0;

    void Start()
    {
        Debug.Log("Spawn Manager dimulai!");

        StartCoroutine(SpawnZombies());

        StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnZombies()
    {
        while (spawned < totalZombies)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (zombieSpawnPoints.Length == 0)
            {
                Debug.LogWarning("Zombie Spawn Point belum diisi!");
                continue;
            }

            int i = Random.Range(0, zombieSpawnPoints.Length);

            GameObject zombie = Instantiate(
                zombiePrefab,
                zombieSpawnPoints[i].position,
                zombieSpawnPoints[i].rotation
            );

            spawned++;

            Debug.Log(
                "Zombie berhasil di-spawn! Total zombie: "
                + spawned + "/" + totalZombies
            );
        }

        Debug.Log("Semua zombie selesai di-spawn!");
    }

    IEnumerator SpawnItems()
    {
        // Spawn item awal
        SpawnOneItem();
        SpawnOneItem();

        while (true)
        {
            yield return new WaitForSeconds(itemInterval);

            if (itemCount < maxItems)
            {
                SpawnOneItem();
            }
        }
    }

    void SpawnOneItem()
    {
        if (itemSpawnPoints.Length == 0)
        {
            Debug.LogWarning("Item Spawn Point belum diisi!");
            return;
        }

        int i = Random.Range(0, itemSpawnPoints.Length);

        Vector3 pos =
            itemSpawnPoints[i].position + Vector3.up * 0.5f;

        // 40% medkit, 60% coin
        GameObject prefab =
            Random.value < 0.4f
            ? medkitPrefab
            : coinPrefab;

        if (prefab == null)
        {
            Debug.LogWarning("Prefab item belum diisi!");
            return;
        }

        GameObject obj = Instantiate(
            prefab,
            pos,
            Quaternion.identity
        );

        itemCount++;

        Debug.Log(
            "Item berhasil di-spawn! Total item aktif: "
            + itemCount
        );

        // Kurangi counter saat item dihapus
        ItemTracker tracker =
            obj.AddComponent<ItemTracker>();

        tracker.Init(() =>
        {
            itemCount--;

            Debug.Log(
                "Item diambil/dihapus. Sisa item aktif: "
                + itemCount
            );
        });
    }
}

public class ItemTracker : MonoBehaviour
{
    private System.Action onDestroyCallback;

    public void Init(System.Action cb)
    {
        onDestroyCallback = cb;
    }

    void OnDestroy()
    {
        onDestroyCallback?.Invoke();
    }
}