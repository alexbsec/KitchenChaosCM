using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    /// <summary>
    /// Player property gets and sets Player
    /// </summary>
    public static Player Instance { get; private set; }
    
    /// <summary>
    /// Select counter event handler
    /// </summary>
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectCounterChanged;

    /// <summary>
    /// Acts as event args to know which counter is selected
    /// in the event handler
    /// </summary>
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    /// <summary>
    /// The player movement speed serializable
    /// </summary>
    [SerializeField] private float _moveSpeed = 7f;
    
    /// <summary>
    /// The player rotation speed serializable
    /// </summary>
    [SerializeField] private float _rotateSpeed = 10f;
    
    /// <summary>
    /// The game input object serializable
    /// </summary>
	[SerializeField] private GameInput _gameInput;

    /// <summary>
    /// Layer for counter type objects
    /// </summary>
    [SerializeField] private LayerMask _countersLayerMask;

    /// <summary>
    /// The reference to the empty object where player holds
    /// kitchen object
    /// </summary>
    [SerializeField] private Transform _kitchenObjectHoldPoint;
    private bool _isWalking;
    private float _moveDistance;
    private Vector2 _inputVector;
    private Vector3 _movementDirection;
    private Vector3 _lastMoveDirection;
    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;
    

    /// <summary>
    /// Directions enum
    /// </summary>
    private enum Direction
    {
        X,
        Y,
        Z,
    }

    /// <summary>
    /// Sets the instance to the player object
    /// </summary>
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player in the scene!");
        }
        
        Instance = this;
    }

    /// <summary>
    /// Starts interaction event handler of counters
    /// </summary>
    private void Start()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
        _gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.InteractAlternate(this);
        }
    }

    /// <summary>
    /// Interacts with selected counter object if not null
    /// </summary>
    /// <param name="sender">
    /// The sender
    /// </param>
    /// <param name="e">
    /// The event args
    /// </param>
    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact(this);
        }
    }
    
    /// <summary>
    /// The update method
    /// </summary>
    private void Update()
    {
        _inputVector = _gameInput.GetMovementVectorNormalized();
        HandleMovement();
        HandleInteractions();
    }

    /// <summary>
    /// Handles the movement of the player
    /// </summary>
    private void HandleMovement()
    {
        _moveDistance = _moveSpeed * Time.deltaTime;
        _movementDirection = new Vector3(_inputVector.x, 0.0f, _inputVector.y);
        
        bool canMoveX = CanMove(Direction.X, _movementDirection, out Vector3 xMovement);
        bool canMoveZ = CanMove(Direction.Z, _movementDirection, out Vector3 zMovement);
        bool canMoveFreely = canMoveX && canMoveZ;
        bool canMove = canMoveX || canMoveZ;

        if (!canMoveFreely && canMoveX)
        {
            _movementDirection = xMovement;
        } 
        else if (!canMoveFreely && canMoveZ)
        {
            _movementDirection = zMovement;
        }
        
        if (canMove)
        {
            transform.position += _movementDirection * _moveDistance;
        }
        
        _isWalking = _movementDirection != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, _movementDirection, _rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Handles player interactions
    /// </summary>
    private void HandleInteractions()
    {
        _movementDirection = new Vector3(_inputVector.x, 0.0f, _inputVector.y);
        float interactionDistance = 2.0f;
        
        if (_movementDirection != Vector3.zero)
        {
            _lastMoveDirection = _movementDirection;
        }
        
        bool didHit = Physics.Raycast(
            transform.position,
            _lastMoveDirection,
            out RaycastHit raycastHit,
            interactionDistance,
            _countersLayerMask
        );

        if (!didHit)
        {
            SetSelectedCounter(null);
            return;
        }
        
        bool foundCounter = raycastHit.transform.TryGetComponent(out BaseCounter counter);

        if (!foundCounter)
        {
            SetSelectedCounter(null);
            return;
        }

        if (counter != _selectedCounter)
        {
            SetSelectedCounter(counter);
        }
    }

    /// <summary>
    /// Checks if player can move in the given direction
    /// </summary>
    /// <param name="direction">
    /// The direction to check movement possibility
    /// </param>
    /// <param name="movement">
    /// The player's Vector3 movement direction
    /// </param>
    /// <param name="result">
    /// The result movement direction vector
    /// </param>
    /// <returns>
    /// True if can move in that direction, false otherwise
    /// </returns>
    private bool CanMove(Direction direction, Vector3 movement, out Vector3 result)
    {
        float playerRadius = 0.7f;
        Vector3 playerHeight = Vector3.up * 2.0f;
        
        result = Vector3.zero;
        bool moveChecker = true;

        switch (direction)
        {
            case Direction.X:
            {
                result = new Vector3(movement.x, 0.0f, 0.0f);
                moveChecker = result.x != 0;
                break;
            }

            case Direction.Z:
            {
                result = new Vector3(0.0f, 0.0f, movement.z);
                moveChecker = result.z != 0;
                break;
            }

            case Direction.Y:
            {
                return false;
            }
        }

        bool canMoveToDirection = moveChecker && !Physics.CapsuleCast(
            transform.position,
            transform.position + playerHeight,
            playerRadius,
            result,
            _moveDistance
        );
        
        return canMoveToDirection;
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;
        OnSelectCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = _selectedCounter });
    }

    /// <summary>
    /// Check whether player is walking or not
    /// </summary>
    /// <returns>
    /// True or false
    /// </returns>
    public bool IsWalking()
    {
        return _isWalking;
    }

    /// <summary>
    /// Gets the counter top transform for referencing
    /// </summary>
    /// <returns>
    /// The counter top point empty object transform
    /// </returns>
    public Transform GetKitchenObjectFollowTransform()
    {
        return _kitchenObjectHoldPoint;
    }

    /// <summary>
    /// Sets a new kitchen object to the player
    /// </summary>
    /// <param name="kitchenObject">
    /// The new kitchen object
    /// </param>
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }

    /// <summary>
    /// Gets current kitchen object held by the player
    /// </summary>
    /// <returns>
    /// The current kitchen object
    /// </returns>
    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    /// <summary>
    /// Clears current held kitchen object
    /// </summary>
    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    /// <summary>
    /// Checks whether player is holding a kitchen object
    /// </summary>
    /// <returns>
    /// True if it's holding, false otherwise
    /// </returns>
    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}
