using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Shot
{
    [SerializeField] private float explosionRadius = 2f;   // Raio da explosão
    [SerializeField] private LayerMask enemyLayer;         // Camada dos inimigos
    [SerializeField] private GameObject explosionEffect;   // Efeito visual opcional

    private void Start()
    {
        Invoke(nameof(Explode), 2f); // Explode após 2 segundos se não colidir antes
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        // Efeito visual da explosão
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Detecta inimigos dentro do raio
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Aqui supomos que o inimigo tem um script com método TakeDamage(int)
            enemy.GetComponent<Enemie>()?.TakeDamage(25);
        }

        Destroy(gameObject);
    }

    // Gizmo para visualizar o raio no editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
