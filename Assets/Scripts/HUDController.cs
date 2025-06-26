using TMPro; // Biblioteca para trabalhar com textos usando TextMeshPro
using UnityEngine;
using System.Collections.Generic;

public class HUDController : MonoBehaviour, IHUDObserver // Implementa o padrão Observer para observar mudanças no jogador
{
    private Player player; // Referência ao jogador
    private int currentPlayerLife; // Vida atual do jogador (usada para atualizar os ícones de vida)

    [SerializeField] private List<GameObject> lifeIcons; // Lista dos ícones de vida na HUD
    [SerializeField] private int score; // Pontuação atual

    private TextMeshProUGUI scoreHud; // Referência ao componente de texto da pontuação
    private MenuController menuController; // Referência ao controlador do menu (para pausar, etc.)

    private void Start()
    {
        // Procura o jogador na cena e se registra como observador da vida
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.AddObserver(this); // Se inscreve como observador da vida do jogador

        // Pega a referência ao menu principal
        menuController = GameObject.Find("CanvasMenu").GetComponent<MenuController>();

        // Pega a referência ao objeto que exibe a pontuação
        GameObject scoreObject = GameObject.Find("Score");
        if (scoreObject != null)
            scoreHud = scoreObject.GetComponent<TextMeshProUGUI>();

        // Atualiza a HUD com os valores iniciais de vida e pontuação
        OnLifeChanged(player.Life);
        OnScoreChanged(score);
    }

    // Método chamado sempre que a vida do jogador muda
    public void OnLifeChanged(int currentLife)
    {
        currentPlayerLife = currentLife;

        // Ativa/desativa os ícones de vida com base na vida atual
        for (int i = 0; i < lifeIcons.Count; i++)
        {
            lifeIcons[i].SetActive(i < currentPlayerLife); // Ativa somente os ícones correspondentes à vida atual
        }
    }

    // Método chamado sempre que a pontuação muda
    public void OnScoreChanged(int newScore)
    {
        // Atualiza o texto da HUD com a pontuação formatada (4 dígitos)
        scoreHud.text = newScore.ToString("D4");
    }

    // Método chamado por outros scripts para abrir o menu de pausa
    public void OpenPauseMenu()
    {
        menuController.ShowPauseMenu(); // Abre o menu de pausa
    }

    // Método para adicionar pontos e atualizar a HUD
    public void AddScore(int value)
    {
        score += value; // Soma os pontos
        OnScoreChanged(score); // Atualiza a HUD com a nova pontuação
    }
}

