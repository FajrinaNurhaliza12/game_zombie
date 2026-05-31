using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    [Header("Gun Stats")]
    [SerializeField] private float damage = 25f;
    [SerializeField] private float range = 50f;
    [SerializeField] private float fireRate = 0.4f;
    [SerializeField] private int maxAmmo = 12;
    [SerializeField] private float reloadTime = 1.5f;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 30f;

    private Camera playerCamera;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    public void Init(Camera cam)
    {
        playerCamera = cam;
        currentAmmo = maxAmmo;
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
        Debug.Log("Ammo: " + currentAmmo + "/" + maxAmmo);
    }

    public void SetBullet(GameObject prefab, Transform spawnPoint, float speed)
    {
        bulletPrefab = prefab;
        bulletSpawnPoint = spawnPoint;
        bulletSpeed = speed;
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
                Shoot();
            else
                StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
            StartCoroutine(Reload());

        // Kembalikan posisi setelah recoil
        transform.localPosition = Vector3.Lerp(
            transform.localPosition, originalPos, 10f * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation, originalRot, 10f * Time.deltaTime);
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(
            new Rect(Screen.width / 2 - 10, Screen.height / 2 - 20, 20, 40),
            "+", style);
    }

    void Shoot()
    {
        nextFireTime = Time.time + fireRate;
        currentAmmo--;
        Debug.Log("Ammo: " + currentAmmo + "/" + maxAmmo);

        // Recoil
        transform.localPosition -= new Vector3(0, 0, 0.05f);
        transform.localRotation *= Quaternion.Euler(-5f, 0, 0);

        // Spawn peluru visual
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            Ray aimRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Vector3 targetPoint = Physics.Raycast(aimRay, out RaycastHit aimHit, range)
                ? aimHit.point
                : aimRay.GetPoint(range);

            Vector3 direction = (targetPoint - bulletSpawnPoint.position).normalized;
            Quaternion bulletRotation = Quaternion.LookRotation(direction);

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = direction * bulletSpeed;

            Destroy(bullet, 3f);
        }

        // Raycast untuk damage ke zombie
        int mask = ~LayerMask.GetMask("Gun");
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, range, mask))
        {
            Debug.Log("Kena: " + hit.transform.name);
            hit.transform.GetComponent<ZombieAI>()?.TakeDamage(damage);
        }

        if (currentAmmo <= 0)
            StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        // Animasi pistol turun
        float t = 0;
        Vector3 downPos = originalPos + new Vector3(0, -0.3f, 0);
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(originalPos, downPos, t / 0.3f);
            yield return null;
        }

        yield return new WaitForSeconds(reloadTime);

        // Animasi pistol naik
        t = 0;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(downPos, originalPos, t / 0.3f);
            yield return null;
        }

        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reload selesai! Ammo: " + currentAmmo + "/" + maxAmmo);
    }
}