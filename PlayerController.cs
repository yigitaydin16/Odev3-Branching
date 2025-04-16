using System.Collections;
using UnityEngine;
using TMPro; // TextMeshPro kullanmak için gerekli

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

    public int coin; // Koin sayýsý
    public TextMeshProUGUI coinText; // TextMeshProUGUI referansý
    private AudioManager audioManager;

    public GameOverScreen gameOverScreen; // GameOver ekraný referansý
    public WinScreen winScreen; // Win ekraný referansý

    private void Awake()
    {
        // AudioManager'ý buluyoruz
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        facingRight = true;

        UpdateCoinText(); // Baþlangýçta koin sayýsýný güncelle
    }

    private void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Yön deðiþtir
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }

        // Zemin kontrolü
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Duvar kontrolü
        isTouchingWall = Physics2D.Raycast(wallCheck.position, facingRight ? Vector2.right : Vector2.left, wallCheckDistance, wallLayer);

        // Zýplama kontrolü
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

        // Animasyon kontrolü
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
            // Ölüm sesini çal
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

    // GameEnd objesine çarpýldýðýnda çaðrýlacak fonksiyon
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("GameEnd")) // GameEnd tag'ine sahip objeye çarptýðýnda
        {
            // WinScreen referansýný kontrol et ve aç
            if (winScreen != null)
            {
                winScreen.Setup(); // Win ekranýný aç
            }
            else
            {
                Debug.LogError("WinScreen referansý null!");
            }
        }
    }

    private IEnumerator HandleDeath(float delay)
    {
        // Karakteri görünmez yap
        Transform characterTransform = transform.Find("Character");
        if (characterTransform != null)
        {
            characterTransform.gameObject.SetActive(false); // Karakteri görünmez yap
        }

        yield return new WaitForSeconds(delay); // Gecikme süresi bekleniyor

        // Debug ile GameOverScreen referansýný kontrol edelim
        if (gameOverScreen == null)
        {
            Debug.LogError("GameOverScreen referansý null!");
        }
        else
        {
            gameOverScreen.Setup(); // GameOverScreen'i aktif et
        }
    }

    // Koin sayýsýný güncelleyen metod
    public void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coin.ToString("000"); // Koin sayýsýný 3 haneli formatta güncelle
        }
    }

    // Koin sayýsýný arttýr ve UI'yi güncelle
    public void AddCoin(int amount)
    {
        coin += amount;
        UpdateCoinText(); // UI'yi güncelle
    }
}