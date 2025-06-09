using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

// Classe responsável pelos tiros e bombas do jogador
public class PlayerShot : MonoBehaviour
{
    [SerializeField] private GameObject shotPrefab;
    [SerializeField] private GameObject bombPrefab;

    private int bombs;
    [SerializeField] private int maxBombs;

    [SerializeField] private float shotCouldown;
    [SerializeField] private float shotVelocity;
    [SerializeField] private int shotDamage;
    [SerializeField] private float shotAngle;
    [SerializeField] private float shotLife;

    private PlayerInputs playerInputs;

    public int MaxBombs => maxBombs;

    public int Bombs
    {
        get { return bombs; }
        set
        {
            bombs = Mathf.Clamp(value, 0, maxBombs);
            playerInputs.UpdateBombButtonVisibility(bombs);
        }
    }

    private void Start()
    {
        playerInputs = GetComponent<PlayerInputs>();
        Bombs = maxBombs; // Isso chama o setter e atualiza o botão
        InvokeRepeating(nameof(Shot), shotCouldown, shotCouldown);
    }

    private void Update()
    {
        if (playerInputs.BombPressed && Bombs > 0)
        {
            ThrowBomb();
            playerInputs.BombPressed = false;
        }
    }

    private void Shot()
    {
        GameObject shot = Instantiate(shotPrefab, transform.position, Quaternion.identity);
        shot.GetComponent<Shot>().Initialize(shotVelocity, shotDamage, true, shotAngle, 3f);
    }

    private void ThrowBomb()
    {
        Debug.Log("Bomba lançada");

        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);

        // Aqui você pode ajustar a lógica da bomba para voar na direção
        Vector2 direction = playerInputs.GetInputDirection();
        if (direction == Vector2.zero) direction = Vector2.right;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Bomb bombScript = bomb.GetComponent<Bomb>();
        bombScript.Initialize(25f, 3, true, shotAngle, 3f); // exemplo: velocidade, dano, é do player, ângulo, tempo de vida

        Bombs--; // Reduz bomba e oculta botão se zerar
    }
}
