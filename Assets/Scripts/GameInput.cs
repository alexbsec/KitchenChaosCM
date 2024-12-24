using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
	/// <summary>
	/// Event handler for interaction input
	/// </summary>
	public event EventHandler OnInteractAction;
	
	/// <summary>
	/// The player input actions object
	/// for new Input System
	/// </summary>
    private PlayerInputActions _playerInputActions;
	
    /// <summary>
    /// Awake method to start new input system
    /// </summary>
    private void Awake() 
	{
		_playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Interact.performed += Interact_performed;
	}

    /// <summary>
    /// Listens and fires an event
    /// </summary>
    /// <param name="context">
    /// The context
    /// </param>
	private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		OnInteractAction?.Invoke(this, EventArgs.Empty);
	}

    /// <summary>
    /// Gets the movement vector based on input
    /// </summary>
    /// <returns>
    /// Returns a Vector2 object <see cref="Vector2"/>
    /// </returns>
    public Vector2 GetMovementVectorNormalized() 
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
