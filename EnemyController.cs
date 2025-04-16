using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType { Horizontal, Vertical }  // Düþman türleri
    public EnemyType enemyType;

    public float speed;

    public Transform pointA;
    public Transform pointB;

    public Transform player;  // Player karakterinin Transform'u

    private Vector2 targetPosition;
    private bool movingToB;

    private void Start()
    {
        targetPosition = pointB.position;  // Baþlangýçta B noktasýna git
    }

    private void Update()
    {
        // Düþman türüne göre hareket et
        switch (enemyType)
        {
            case EnemyType.Horizontal:
                MoveHorizontal();
                break;
            case EnemyType.Vertical:
                MoveVertical();
                break;
        }

        // Düþman türüne göre farklý bakýþ davranýþý
        if (enemyType == EnemyType.Horizontal)
        {
            FlipBasedOnMovement();  // Yatay düþman hareket yönüne göre bakar
        }
        else if (enemyType == EnemyType.Vertical)
        {
            LookAtPlayer();  // Dikey düþman sadece Player'a bakar
        }
    }

    // Yatay hareket
    private void MoveHorizontal()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (movingToB)
            {
                targetPosition = pointA.position;
            }
            else
            {
                targetPosition = pointB.position;
            }

            movingToB = !movingToB;
        }
    }

    // Dikey hareket
    private void MoveVertical()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (movingToB)
            {
                targetPosition = pointA.position;
            }
            else
            {
                targetPosition = pointB.position;
            }

            movingToB = !movingToB;
        }
    }

    // Yatay düþmanýn hareket ettiði yöne göre yönünü deðiþtirme
    private void FlipBasedOnMovement()
    {
        if (targetPosition.x > transform.position.x && transform.localScale.x < 0)
        {
            Flip();
        }
        else if (targetPosition.x < transform.position.x && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    // Player'a bakma iþlemi (dikey düþmanlar için)
    private void LookAtPlayer()
    {
        if (player != null)
        {
            Vector2 direction = player.position - transform.position;

            if (direction.x > 0 && transform.localScale.x < 0)
            {
                Flip();
            }
            else if (direction.x < 0 && transform.localScale.x > 0)
            {
                Flip();
            }
        }
    }

    // Karakterin yönünü ters çevirme (X ekseni)
    private void Flip()
    {
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
