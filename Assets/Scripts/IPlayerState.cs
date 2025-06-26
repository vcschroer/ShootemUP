using UnityEngine;

public interface IPlayerState
{
    void Enter(Player player);
    void UpdateState(Player player);
    void OnTriggerEnter(Player player, Collider2D collision);
    void TakeDamage(Player player, int damage);
}
