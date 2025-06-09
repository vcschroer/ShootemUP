using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Classe responsável por capturar os inputs do jogador
public class PlayerInputs : MonoBehaviour
{
    [SerializeField] private FixedJoystick digitalAnalogic;
    private Vector2 inputDirection;

    [SerializeField] private Button bombButton;

    private bool bombPressed = false;

    public bool BombPressed
    {
        get { return bombPressed; }
        set { bombPressed = value; }
    }

    // Chame este método do PlayerShot sempre que a quantidade de bombas mudar
    public void UpdateBombButtonVisibility(int bombCount)
    {
        bombButton.gameObject.SetActive(bombCount > 0);
    }

    private void Start()
    {
        bombButton.onClick.AddListener(OnBombButtonClicked);
    }

    private void Update()
    {
        MovimentInput();
    }

    public Vector2 GetInputDirection()
    {
        return inputDirection;
    }

    public void MovimentInput()
    {
        inputDirection = digitalAnalogic.Direction;
    }

    private void OnBombButtonClicked()
    {
        bombPressed = true;
    }
}