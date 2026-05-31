using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float healAmount = 25f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(0, 90f * Time.deltaTime, 0);

        transform.position = new Vector3(
            startPos.x,
            startPos.y + Mathf.Sin(Time.time * 2f) * 0.2f,
            startPos.z
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);

            Debug.Log("Medkit berhasil diklaim! HP bertambah +" + healAmount);
        }

        Destroy(gameObject);
    }
}