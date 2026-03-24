using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionMapChanger : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameManagers gameManager;

    private void HandleGameStateChanged(GameManagers.GameState state)
    {
        switch (state)
        {
            case GameManagers.GameState.UI:
                playerInput.SwitchCurrentActionMap("UI");
                break;
            case GameManagers.GameState.Gameplay:
                playerInput.SwitchCurrentActionMap("Gameplay");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void OnEnable()
    {
        gameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        gameManager.OnGameStateChanged -= HandleGameStateChanged;
    }
}