using UnityEngine;

public class Circle : Enemie
{
    [SerializeField] private float leftLimit = -8f;   // Limite esquerdo
    [SerializeField] private float rightLimit = 8f;   // Limite direito
    [SerializeField] private float homingDuration = 1.5f; // Tempo que o tiro persegue o jogador

    private Transform player; // Referência ao jogador
    private Vector2 targetPosition; // Posição atual do jogador

    protected override void Start()
    {
        movimentAngle = 180f; // Começa indo para a esquerda
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected override void Moviment()
    {
        base.Moviment();

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

    private void GetPlayerPosition()
    {
        if (player != null)
        {
            targetPosition = player.position;
        }
    }

    protected override void Shot()
    {
        if (transform.position.y >= 10 || transform.position.y <= -9)
        {
            return;
        }

        GetPlayerPosition();

        GameObject shot = Instantiate(shotPrefab, transform.position, Quaternion.identity);
        shot.GetComponent<HoamingShot>().Initialize(shotVelocity, shotDamage, false, shotAngle, shotLife, homingDuration, targetPosition);
    }
}