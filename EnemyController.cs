using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType { Horizontal, Vertical }  // D��man t�rleri
    public EnemyType enemyType;

    public float speed;

    public Transform pointA;
    public Transform pointB;

    public Transform player;  // Player karakterinin Transform'u

    private Vector2 targetPosition;
    private bool movingToB;

    private void Start()
    {
        targetPosition = pointB.position;  // Ba�lang��ta B noktas�na git
    }

    private void Update()
    {
        // D��man t�r�ne g�re hareket et
        switch (enemyType)
        {
            case EnemyType.Horizontal:
                MoveHorizontal();
                break;
            case EnemyType.Vertical:
                MoveVertical();
                break;
        }

        // D��man t�r�ne g�re farkl� bak�� davran���
        if (enemyType == EnemyType.Horizontal)
        {
            FlipBasedOnMovement();  // Yatay d��man hareket y�n�ne g�re bakar
        }
        else if (enemyType == EnemyType.Vertical)
        {
            LookAtPlayer();  // Dikey d��man sadece Player'a bakar
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

    // Yatay d��man�n hareket etti�i y�ne g�re y�n�n� de�i�tirme
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

    // Player'a bakma i�lemi (dikey d��manlar i�in)
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

    // Karakterin y�n�n� ters �evirme (X ekseni)
    private void Flip()
    {
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
