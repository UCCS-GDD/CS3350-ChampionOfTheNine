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
    public const int GROUND_LAYER = 8;
    public const float GROUND_CHECK_RADIUS = 0.05f;

    public const float RANGER_HEALTH = 100;
    public const float RANGER_ENERGY = 100;
    public const float RANGER_MOVE_SPEED = 4;
    public const float RANGER_JUMP_SPEED = 4;
    public const float RANGER_REGEN = 20;

    public const float BASIC_ARROW_SPEED = 10;
    public const float BASIC_ARROW_DAMAGE = 10;
    public const float BASIC_ARROW_CD = 0.5f;
    public const float BASIC_ARROW_COST = 5;

    public const float EXP_ARROW_SPEED = 9;
    public const float EXP_ARROW_DAMAGE = 40;
    public const float EXP_ARROW_CD = 0.1f;
    public const float EXP_ARROW_COST = 30;
}
