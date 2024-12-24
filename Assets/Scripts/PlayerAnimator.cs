using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    /// <summary>
    /// The player object serializable
    /// </summary>
    [SerializeField] private Player player;
    
    /// <summary>
    /// The trigger name
    /// </summary>
    private const string IS_WALKING = "isWalking";
    
    /// <summary>
    /// The animator object
    /// </summary>
    private Animator animator;

    /// <summary>
    /// The Awake method to initialize the
    /// animator
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Update method to change player animation
    /// state
    /// </summary>
    private void Update()
    {
        bool isWalking = player.IsWalking();
        animator.SetBool(IS_WALKING, isWalking);
    }
}
