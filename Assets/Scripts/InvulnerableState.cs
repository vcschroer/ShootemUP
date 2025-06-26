using UnityEngine;

public class InvulnerableState : IPlayerState
{
    private float invulnerabilityTime;

    public InvulnerableState()
    {
        invulnerabilityTime = 2f; // Tempo de invulnerabilidade
    }

    public void Enter(Player player)
    {
        // Define o tempo restante de invulnerabilidade
        player.CurrentInvulnerability = invulnerabilityTime;
    }

    public void UpdateState(Player player)
    {
        // Reduz o tempo de invulnerabilidade a cada frame
        player.CurrentInvulnerability -= Time.deltaTime;

        // Quando o tempo acabar, volta para o estado normal
        if (player.CurrentInvulnerability <= 0)
        {
            player.SetState(new NormalState());
        }
    }

    public void OnTriggerEnter(Player player, Collider2D collision)
    {
        // Durante a invulnerabilidade, o jogador ignora colisões
    }

    public void TakeDamage(Player player, int damage)
    {
        // Também ignora qualquer tentativa de dano
    }
}