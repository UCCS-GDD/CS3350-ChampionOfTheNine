using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Class that holds the game's constants
/// </summary>
public static class Constants
{
    public const string ENEMY_TAG = "Enemy";
    public const string PLAYER_TAG = "Player";
    public static int GROUND_LAYER = LayerMask.NameToLayer("Ground");
    public const float GROUND_CHECK_RADIUS = 0.05f;

    public const float RANGER_AI_RANGE = 7;
    public const float RANGER_AI_ANGLE_RANGE = 10;
    public const float RANGER_HEALTH = 100;
    public const float RANGER_ENERGY = 100;
    public const float RANGER_MOVE_SPEED = 5;
    public const float RANGER_JUMP_SPEED = 4;
    public const float RANGER_REGEN = 20;
    public const float RANGER_GCD = 0.3f;
    public const float RANGER_BOOST_CD = 30f;
    public const float RANGER_BOOST_TIME = 6f;
    public const float RANGER_BOOST_MOVE_MULT = 1.3f;
    public const float RANGER_BOOST_JUMP_MULT = 1.5f;
    public const float RANGER_BOOST_CD_MULT = 0.7f;
    public const float RANGER_BOOST_ARROW_SPEED_MULT = 1.2f;
    public const float RANGER_BOOST_ARROW_DAMAGE_MULT = 1.6f;
    public const float RANGER_BOOST_ENERGY_REGEN_MULT = 1.8f;

    public const float BASIC_ARROW_SPEED = 11;
    public const float BASIC_ARROW_DAMAGE = 10;
    public const float BASIC_ARROW_COST = 5;

    public const float PIERCE_ARROW_SPEED = 25;
    public const float PIERCE_ARROW_DAMAGE = 15;
    public const float PIERCE_ARROW_COST = 12;
    public const float PIERCE_SHOOT_WINDOW = 0.7f;
    public const float PIERCE_SHOOT_CD = 0.1f;
    public const float PIERCE_ABILITY_CD = 10f;

    public const float EXP_ARROW_SPEED = 10;
    public const float EXP_ARROW_DAMAGE = 20;
    public const float EXP_ARROW_COST = 30;

    public const float AI_SCAN_DELAY = 5;

    /// <summary>
    /// Gets the mouse position in world space
    /// </summary>
    public static Vector2 MousePosition
    { get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); } }
}
