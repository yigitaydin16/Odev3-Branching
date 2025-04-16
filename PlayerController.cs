using System.Collections;
using UnityEngine;
using TMPro; // TextMeshPro kullanmak i�in gerekli

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    private Rigidbody2D rb;
    private bool facingRight;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform wallCheck;
    public LayerMask wallLayer;
    public float wallCheckDistance = 0.5f;
    public float groundCheckRadius = 0.5f;

    private bool isJumping;
    private bool isGrounded;
    private bool isTouchingWall;
    private Animator anim;

    public int coin; // Koin say�s�
    public TextMeshProUGUI coinText; // TextMeshProUGUI referans�
    private AudioManager audioManager;

    public GameOverScreen gameOverScreen; // GameOver ekran� referans�
    public WinScreen winScreen; // Win ekran� referans�

    private void Awake()
    {
        // AudioManager'� buluyoruz
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        facingRight = true;

        UpdateCoinText(); // Ba�lang��ta koin say�s�n� g�ncelle
    }

    private void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Y�n de�i�tir
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }

        // Zemin kontrol�
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Duvar kontrol�
        isTouchingWall = Physics2D.Raycast(wallCheck.position, facingRight ? Vector2.right : Vector2.left, wallCheckDistance, wallLayer);

        // Z�plama kontrol�
        if (Input.GetKeyDown(KeyCode.Z) && isGrounded && !isTouchingWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;

            audioManager.PlaySFX(audioManager.jump, 0.02f);
        }

        if (isGrounded && isJumping)
        {
            isJumping = false;
        }

        // Animasyon kontrol�
        anim.SetBool("hareket", moveInput != 0);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Enemy"))
        {
            // �l�m sesini �al
            if (collision.gameObject.CompareTag("Spike"))
            {
                audioManager.PlaySFX(audioManager.death, 0.008f);
            }
            else if (collision.gameObject.CompareTag("Enemy"))
            {
                audioManager.PlaySFX(audioManager.death2, 0.02f);
            }

            StartCoroutine(HandleDeath(0.2f)); // 0.2 saniye gecikme
        }
    }

    // GameEnd objesine �arp�ld���nda �a�r�lacak fonksiyon
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("GameEnd")) // GameEnd tag'ine sahip objeye �arpt���nda
        {
            // WinScreen referans�n� kontrol et ve a�
            if (winScreen != null)
            {
                winScreen.Setup(); // Win ekran�n� a�
            }
            else
            {
                Debug.LogError("WinScreen referans� null!");
            }
        }
    }

    private IEnumerator HandleDeath(float delay)
    {
        // Karakteri g�r�nmez yap
        Transform characterTransform = transform.Find("Character");
        if (characterTransform != null)
        {
            characterTransform.gameObject.SetActive(false); // Karakteri g�r�nmez yap
        }

        yield return new WaitForSeconds(delay); // Gecikme s�resi bekleniyor

        // Debug ile GameOverScreen referans�n� kontrol edelim
        if (gameOverScreen == null)
        {
            Debug.LogError("GameOverScreen referans� null!");
        }
        else
        {
            gameOverScreen.Setup(); // GameOverScreen'i aktif et
        }
    }

    // Koin say�s�n� g�ncelleyen metod
    public void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coin.ToString("000"); // Koin say�s�n� 3 haneli formatta g�ncelle
        }
    }

    // Koin say�s�n� artt�r ve UI'yi g�ncelle
    public void AddCoin(int amount)
    {
        coin += amount;
        UpdateCoinText(); // UI'yi g�ncelle
    }
}