using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hexagon : Enemie
{
    private Vector2 targetPosition; // Posição do jogador
    [SerializeField] private float homingDuration; // Duração que o tiro persegue
    private Transform player; // Referência ao jogador

    protected override void Initialize()
    {
        base.Initialize();
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Busca o jogador
    }

    // Atualiza a posição do jogador
    private void GetPlayerPosition()
    {
        if (player != null)
            targetPosition = player.position;
    }

    // Dispara projétil perseguidor
    protected override void Shot()
    {
        if (transform.position.y >= 10 || transform.position.y <= -9)
            return; // Fora da tela

        GetPlayerPosition(); // Atualiza alvo

        // Cria o projétil e inicializa com dados de perseguição
        GameObject shot = Instantiate(shotPrefab, transform.position, Quaternion.identity);
        shot.GetComponent<HoamingShot>().Initialize(
            shotVelocity, shotDamage, false, shotAngle, shotLife, homingDuration, targetPosition
        );
    }
}
