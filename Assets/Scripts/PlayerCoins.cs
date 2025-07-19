using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    public int coins = 0;

    public void AddCoins(int amount)
    {
        coins += amount;
        // Aquí puedes actualizar la UI o reproducir un sonido
        Debug.Log($"Monedas: {coins}");
    }
}
