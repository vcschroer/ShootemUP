using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inimigo que se move na horizontal e atira na diagonal
public class Square : Enemie
{
    [SerializeField] private bool moveRight; // Define se o movimento começa para a direita
    [SerializeField] private float leftLimit = -8f; // Limite esquerdo de movimento
    [SerializeField] private float rightLimit = 8f; // Limite direito de movimento

    // Sobrescreve o cálculo da direção de movimento e tiro
    protected override void ConfigureDirection()
    {
        // Define ângulos personalizados com base no sentido
        if (moveRight)
        {
            movimentAngle = 315f; // Diagonal direita-baixo
            shotAngle = 225f; // Tiro na diagonal esquerda-baixo
        }
        else
        {
            movimentAngle = 225f; // Diagonal esquerda-baixo
            shotAngle = 315f; // Tiro na diagonal direita-baixo
        }

        base.ConfigureDirection(); // Usa cálculo padrão de direção
    }

    // Movimento com inversão de direção nos limites
    protected override void Moviment()
    {
        base.Moviment(); // Aplica movimento padrão

        // Inverte a direção horizontal ao atingir os limites
        if (transform.position.x <= leftLimit && movimentDirection.x < 0)
            movimentDirection.x *= -1;
        else if (transform.position.x >= rightLimit && movimentDirection.x > 0)
            movimentDirection.x *= -1;

        rb.linearVelocity = movimentDirection * movimentSpeed; // Aplica nova direção
    }
}
