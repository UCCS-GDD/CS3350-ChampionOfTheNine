using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the player
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioListener))]
public class PlayerScript : CharacterScript
{
    #region Fields

    [SerializeField]float moveSpeed;
    [SerializeField]float jumpSpeed;
    [SerializeField]Transform groundCheck;
    [SerializeField]LayerMask whatIsGround;

    Rigidbody2D rbody;
    bool grounded = false;

    const float GROUND_CHECK_RADIUS = 0.05f;

    #endregion

    #region Properties



    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        base.Start();
        rbody = GetComponent<Rigidbody2D>();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Handles jumping
        float vMovement = 0;
        grounded = Physics2D.OverlapCircle(groundCheck.position, GROUND_CHECK_RADIUS, whatIsGround);
        if (grounded && Input.GetButtonDown("Jump"))
        { vMovement = jumpSpeed; }

        // Handles horizontal movement
        float hMovement = Input.GetAxis("Horizontal") * moveSpeed;
        rbody.velocity = new Vector2(hMovement, rbody.velocity.y + vMovement);
    }

    #endregion
}
