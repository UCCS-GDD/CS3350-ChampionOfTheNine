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

    #endregion
}
