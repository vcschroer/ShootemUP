using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Square : Enemie
{
    [SerializeField] private bool moveRight; // Define a direção inicial
    [SerializeField] private float leftLimit = -8f;
    [SerializeField] private float rightLimit = 8f;

    protected override void Start()
    {
        scoreValue = 15; // ou 50, ou qualquer valor específico para esse inimigo

        base.Start();

        // Define ângulos iniciais de movimento e tiro
        if (moveRight)
        {
            movimentAngle = 315f; // Diagonal direita-baixo
            shotAngle = 225f;
        }
        else
        {
            movimentAngle = 225f; // Diagonal esquerda-baixo
            shotAngle = 315f;
        }

        float radian = movimentAngle * Mathf.Deg2Rad;
        movimentDirection = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

        InvokeRepeating("Shot", shotCouldown, shotCouldown);
    }

    protected override void Moviment()
    {
        base.Moviment();

        // Verifica colisão com os limites horizontais e inverte a direção horizontal
        if (transform.position.x <= leftLimit && movimentDirection.x < 0)
        {
            movimentDirection.x *= -1;
        }
        else if (transform.position.x >= rightLimit && movimentDirection.x > 0)
        {
            movimentDirection.x *= -1;
        }

        rb.linearVelocity = movimentDirection * movimentSpeed;
    }
}
