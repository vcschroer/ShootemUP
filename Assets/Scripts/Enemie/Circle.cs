using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : Enemie
{
    [SerializeField] private float leftLimit = -8f; // Limite esquerdo
    [SerializeField] private float rightLimit = 8f; // Limite direito
    [SerializeField] private float homingDuration = 1.5f; // Duração do tiro perseguidor
    [SerializeField] private float verticalSpeed = 0.5f; // Velocidade de descida

    private Transform player; // Referência ao jogador
    private Vector2 targetPosition; // Posição atual do jogador
    private Vector2 horizontalDirection = Vector2.right; // Direção horizontal

    protected override void Initialize()
    {
        base.Initialize();
        horizontalDirection = Vector2.right; // Começa indo para a direita
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Encontra jogador
    }

    // Movimento combinado horizontal + descida constante
    protected override void Moviment()
    {
        base.Moviment();

        // Inverte direção horizontal ao atingir limites
        if ((transform.position.x <= leftLimit && horizontalDirection.x < 0) ||
            (transform.position.x >= rightLimit && horizontalDirection.x > 0))
        {
            horizontalDirection *= -1;
        }

        // Soma direção horizontal e vertical, e normaliza para manter velocidade
        Vector2 finalDirection = horizontalDirection + Vector2.down * verticalSpeed;
        rb.linearVelocity = finalDirection.normalized * movimentSpeed;
    }

    private void GetPlayerPosition()
    {
        if (player != null)
            targetPosition = player.position;
    }

    // Dispara tiro que persegue o jogador
    protected override void Shot()
    {
        if (transform.position.y >= 10 || transform.position.y <= -9)
            return;

        GetPlayerPosition();

        GameObject shot = Instantiate(shotPrefab, transform.position, Quaternion.identity);
        shot.GetComponent<HoamingShot>().Initialize(
            shotVelocity, shotDamage, false, shotAngle, shotLife, homingDuration, targetPosition
        );
    }
}