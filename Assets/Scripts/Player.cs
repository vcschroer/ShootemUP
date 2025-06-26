using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxLife = 4; // Vida máxima configurada no Inspector
    private int life; // Vida atual

    [SerializeField] private float invulnerability = 2f; // Duração da invulnerabilidade após dano
    private float currentInvulnerability = 0f; // Tempo restante da invulnerabilidade

    private Rigidbody2D rb; // Componente de física
    private Camera mainCamera; // Referência à câmera principal
    private Vector2 minBounds, maxBounds; // Limites da tela
    private SpriteRenderer spriteRenderer; // Para feedback visual de dano

    private List<IHUDObserver> observers = new List<IHUDObserver>(); // Lista de observadores (ex: HUD)

    private IPlayerState currentState; // Estado atual do jogador (normal ou invulnerável)

    public int Life
    {
        get { return life; }
        set
        {
            life = value;
            NotifyLifeChanged(); // Notifica a HUD quando a vida muda
        }
    }

    public float MaxLife => maxLife; // Getter para vida máxima
    public float Invulnerability => invulnerability; // Getter para tempo de invulnerabilidade
    public float CurrentInvulnerability { get => currentInvulnerability; set => currentInvulnerability = value; }

    public void SetState(IPlayerState newState)
    {
        currentState = newState; // Muda o estado atual
        currentState.Enter(this); // Executa lógica ao entrar no novo estado
    }

    private void Start()
    {
        // Inicializa componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        life = maxLife;
        rb = GetComponent<Rigidbody2D>();

        // Começa no estado normal
        SetState(new NormalState());
    }

    private void Update()
    {
        currentState.UpdateState(this); // Chama o update do estado atual

        // Garante que o jogador fique dentro dos limites da câmera
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y);
        transform.position = new Vector2(clampedX, clampedY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Encaminha a colisão para o estado atual
        currentState.OnTriggerEnter(this, collision);
    }

    public IEnumerator DamageFeedback()
    {
        // Pisca o sprite para indicar dano
        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.2f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void AddObserver(IHUDObserver observer)
    {
        // Adiciona observadores (como HUD) à lista
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    private void NotifyLifeChanged()
    {
        // Notifica todos os observadores da mudança de vida
        foreach (var observer in observers)
        {
            observer.OnLifeChanged(life);
        }
    }

    public void Die()
    {
        // Exibe menu de tentar novamente e destrói o jogador
        GameObject.Find("CanvasMenu").GetComponent<MenuController>().showTryAgainMenu();
        Destroy(gameObject);
    }

    public void UpdateVelocity(Vector2 velocity)
    {
        // Atualiza a velocidade do jogador
        rb.linearVelocity = velocity;
    }
}