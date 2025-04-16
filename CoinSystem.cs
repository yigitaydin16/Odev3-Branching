using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    private PlayerController player;
    public GameObject effect;

    private AudioManager audioManager;

    private void Awake()
    {
        // AudioManager'ý buluyoruz
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        // Oyuncuyu buluyoruz
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Tag kontrolü için CompareTag kullan
        {
            // Oyuncunun coin sayýsýný arttýrýyoruz
            player.AddCoin(1); // Coin'i burada ekliyoruz
            // Coin GameObject'ini yok ediyoruz
            Destroy(gameObject);

            // Görsel efekt oluþturuyoruz
            Instantiate(effect, transform.position, Quaternion.identity);

            // Coin ses efektini çalarken ses seviyesini %50 olarak ayarlýyoruz
            audioManager.PlaySFX(audioManager.coin, 0.01f);
        }
    }
}
