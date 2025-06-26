using UnityEngine;

public class NormalState : IPlayerState
{
    public void Enter(Player player)
    {
        // Quando entra no estado normal: nada acontece de especial
    }

    public void UpdateState(Player player)
    {
        // Estado normal não tem atualizações específicas a cada frame
    }

    public void OnTriggerEnter(Player player, Collider2D collision)
    {
        // Se colidir com inimigo, toma 1 de dano
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(player, 1);
        }
        // Se colidir com um tiro inimigo, aplica o dano do projétil
        else if (collision.CompareTag("Shot"))
        {
            Shot shot = collision.GetComponent<Shot>();
            if (shot != null && !shot.IsShotPlayer) // Verifica se o tiro não é do jogador
            {
                TakeDamage(player, shot.Damage);
                GameObject.Destroy(collision.gameObject); // Destroi o projétil
            }
        }
    }

    public void TakeDamage(Player player, int damage)
    {
        player.Life -= damage; // Reduz a vida
        player.StartCoroutine(player.DamageFeedback()); // Inicia feedback visual

        player.SetState(new InvulnerableState()); // Muda para o estado invulnerável

        if (player.Life <= 0)
        {
            player.Life = 0;
            player.Die(); // Encerra o jogo
        }
    }
}