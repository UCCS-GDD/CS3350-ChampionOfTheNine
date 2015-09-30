using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls an explosion
/// </summary>
public class ExplosionScript : MonoBehaviour
{
    #region Fields

    Animator animator;
    float damage;
    string targetTag;

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the explosion
    /// </summary>
    /// <param name="damage">the damage</param>
    /// <param name="targetTag">the tag of the targeted characters</param>
    public void Initialize(float damage, string targetTag)
    {
        this.damage = damage;
        this.targetTag = targetTag;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Handles the explosion colliding with something
    /// </summary>
    /// <param name="other">the other collider</param>
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Checks for if enemy
        if (other.gameObject.tag == targetTag)
        {
            other.gameObject.GetComponent<CharacterScript>().Damage(damage);
        }
    }

    #endregion
}
