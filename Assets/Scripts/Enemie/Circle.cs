using UnityEngine;

public class Circle : Enemie
{
    [SerializeField] private float leftLimit = -8f;   // Limite esquerdo
    [SerializeField] private float rightLimit = 8f;   // Limite direito
    [SerializeField] private float homingDuration = 1.5f; // Tempo que o tiro persegue o jogador

    private Transform player; // Referência ao jogador
    private Vector2 targetPosition; // Posição atual do jogador
    private Vector2 horizontalDirection = Vector2.right;
    [SerializeField] private float verticalSpeed = 0.5f; // velocidade de descida contínua

    


    protected override void Start()
    {
        scoreValue = 20; // ou 50, ou qualquer valor específico para esse inimigo
        base.Start();

        // Começa indo para a direita
        horizontalDirection = Vector2.right;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        InvokeRepeating("Shot", shotCouldown, shotCouldown);
    }
    protected override void Moviment()
    {
        base.Moviment();

        // Verifica limites horizontais para inverter direção
        if ((transform.position.x <= leftLimit && horizontalDirection.x < 0) ||
            (transform.position.x >= rightLimit && horizontalDirection.x > 0))
        {
            horizontalDirection *= -1;
        }

        // Composição do movimento: horizontal + vertical contínuo
        Vector2 finalDirection = horizontalDirection + Vector2.down * verticalSpeed;

        rb.linearVelocity = finalDirection.normalized * movimentSpeed;
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