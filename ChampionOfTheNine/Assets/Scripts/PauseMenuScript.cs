using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the pause menu
/// </summary>
public class PauseMenuScript : MonoBehaviour
{
    #region Fields

    bool open = false;
    const float NOT_OPEN_X = -13.5f;
    const float OPEN_X = -3.78f;
    const float SPEED = 10;
    Rigidbody2D rbody;

    #endregion

    #region Properties



    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { open = !open; }

        if (open && transform.localPosition.x < OPEN_X)
        { rbody.velocity = new Vector2(SPEED, 0); }
        else if (!open && transform.transform.localPosition.x > NOT_OPEN_X)
        { rbody.velocity = new Vector2(-SPEED, 0); }
        else
        { rbody.velocity = Vector2.zero; }
    }

    #endregion
}
