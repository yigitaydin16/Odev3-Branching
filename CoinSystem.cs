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
        // AudioManager'� buluyoruz
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        // Oyuncuyu buluyoruz
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Tag kontrol� i�in CompareTag kullan
        {
            // Oyuncunun coin say�s�n� artt�r�yoruz
            player.AddCoin(1); // Coin'i burada ekliyoruz
            // Coin GameObject'ini yok ediyoruz
            Destroy(gameObject);

            // G�rsel efekt olu�turuyoruz
            Instantiate(effect, transform.position, Quaternion.identity);

            // Coin ses efektini �alarken ses seviyesini %50 olarak ayarl�yoruz
            audioManager.PlaySFX(audioManager.coin, 0.01f);
        }
    }
}
