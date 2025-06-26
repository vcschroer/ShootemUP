using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemie : MonoBehaviour
{
    [SerializeField] private int maxLife; // Vida máxima do inimigo
    private int life; // Vida atual do inimigo

    protected Rigidbody2D rb; // Referência ao Rigidbody2D
    [SerializeField] protected float movimentSpeed; // Velocidade de movimento
    [SerializeField] protected float movimentAngle; // Ângulo de movimento (em graus)
    protected Vector2 movimentDirection; // Direção calculada com base no ângulo

    [SerializeField] protected GameObject shotPrefab; // Prefab do projétil
    [SerializeField] protected float shotCouldown; // Tempo entre tiros
    [SerializeField] protected float shotVelocity; // Velocidade do tiro
    [SerializeField] protected int shotDamage; // Dano do tiro
    [SerializeField] protected float shotAngle; // Ângulo de disparo
    [SerializeField] protected float shotLife; // Tempo de vida do tiro
    [SerializeField] protected int scoreValue = 10; // Pontos concedidos ao jogador quando o inimigo morre

    private List<IHUDObserver> observers = new List<IHUDObserver>(); // Lista de observadores (ex: HUDController)

    public int Life { get { return life; } set { life = value; } } // Propriedade para acessar/modificar a vida
    public float MaxLife { get { return maxLife; } } // Propriedade para acessar a vida máxima

    // Template Method: define a sequência fixa de passos para inicialização
    protected virtual void Start()
    {
        Initialize();           // Passo 1: Inicializa variáveis comuns (vida, rigidbody)
        ConfigureDirection();   // Passo 2: Calcula direção com base no ângulo
        StartShooting();        // Passo 3: Inicia repetição de disparos
    }

    private void Update()
    {
        Moviment(); // Executa movimentação a cada frame
    }

    // Hook Method: pode ser sobrescrito por inimigos específicos para personalizar a direção
    protected virtual void ConfigureDirection()
    {
        float radian = movimentAngle * Mathf.Deg2Rad; // Converte ângulo para radianos
        movimentDirection = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)); // Direção baseada no ângulo
    }

    // Hook Method: define a lógica de disparos, pode ser alterado pelas subclasses
    protected virtual void StartShooting()
    {
        InvokeRepeating("Shot", shotCouldown, shotCouldown); // Dispara periodicamente
    }

    // Inicialização comum para todos os inimigos
    protected virtual void Initialize()
    {
        rb = GetComponent<Rigidbody2D>(); // Pega o Rigidbody2D anexado
        life = maxLife; // Define a vida inicial

        // Obtém o HUDController pela tag e registra como observador de pontos
        IHUDObserver hud = GameObject.FindGameObjectWithTag("HUDController")?.GetComponent<IHUDObserver>();
        if (hud != null)
            AddObserver(hud);
    }

    // Método que aplica dano ao inimigo
    public void TakeDamage(int damage)
    {
        Life -= damage;

        if (life <= 0)
        {
            life = 0;

            NotifyScore(); // Notifica os observadores para somar a pontuação ao morrer
            Destroy(gameObject); // Destroi o inimigo
        }
    }

    // Método público para adicionar um observador (ex: HUDController)
    public void AddObserver(IHUDObserver observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer); // Adiciona à lista se ainda não estiver nela
    }

    // Notifica todos os observadores que pontos devem ser adicionados
    private void NotifyScore()
    {
        foreach (var observer in observers)
        {
            observer.OnScoreChanged(scoreValue); // Envia a quantidade de pontos para somar
        }
    }

    // Movimento básico: anda na direção definida multiplicado pela velocidade
    protected virtual void Moviment()
    {
        rb.linearVelocity = movimentDirection * movimentSpeed;
    }

    // Disparo básico de projétil
    protected virtual void Shot()
    {
        // Se estiver fora da tela, não atira
        if (transform.position.y >= 10 || transform.position.y <= -9)
            return;

        // Instancia o tiro e define seus parâmetros
        GameObject shot = Instantiate(shotPrefab, transform.position, Quaternion.identity);
        shot.GetComponent<Shot>().Initialize(shotVelocity, shotDamage, false, shotAngle, shotLife);
    }

    // Detecta colisão com tiros do jogador
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Shot")) return; // Ignora se não for tiro

        Shot shot = collision.gameObject.GetComponent<Shot>();
        if (shot == null || !shot.IsShotPlayer) return; // Ignora se o tiro não for do jogador

        TakeDamage(shot.Damage); // Aplica dano
        Destroy(collision.gameObject); // Destroi o tiro
    }
}