using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(0, 120f * Time.deltaTime, 0);

        transform.position = new Vector3(
            startPos.x,
            startPos.y + Mathf.Sin(Time.time * 2f) * 0.15f,
            startPos.z
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Coin berhasil diklaim! +" + coinValue);

        GameManager.Instance?.AddCoins(coinValue);

        Destroy(gameObject);
    }
}