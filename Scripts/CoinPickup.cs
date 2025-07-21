using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Coin picked up!");
            PlayerCoins playerCoins = other.GetComponent<PlayerCoins>();
            if (playerCoins != null)
            {
                playerCoins.AddCoins(coinValue);
            }
            Destroy(gameObject);
        }
    }
}
