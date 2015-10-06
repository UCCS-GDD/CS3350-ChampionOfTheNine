﻿using UnityEngine;
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

    // Ranger class
    public const float RANGER_AI_RANGE = 8;
    public const float RANGER_AI_ANGLE_RANGE = 15;
    public const float RANGER_HEALTH = 100;
    public const float RANGER_ENERGY = 100;
    public const float RANGER_MOVE_SPEED = 5;
    public const float RANGER_JUMP_SPEED = 10;
    public const float RANGER_REGEN = 20;
    public const float RANGER_GCD = 0.4f;
    public const float RANGER_BOOST_CD = 30f;
    public const float RANGER_BOOST_TIME = 6f;
    public const float RANGER_BOOST_MOVE_MULT = 1.5f;
    public const float RANGER_BOOST_JUMP_MULT = 1.2f;
    public const float RANGER_BOOST_CD_MULT = 0.6f;
    public const float RANGER_BOOST_ARROW_SPEED_MULT = 1.2f;
    public const float RANGER_BOOST_ARROW_DAMAGE_MULT = 1.6f;
    public const float RANGER_BOOST_ENERGY_REGEN_MULT = 1.8f;

    // Basic arrow
    public const float BASIC_ARROW_SPEED = 13;
    public const float BASIC_ARROW_DAMAGE = 15;
    public const float BASIC_ARROW_COST = 5;

    // Piercing arrow
    public const float PIERCE_ARROW_SPEED = 25;
    public const float PIERCE_ARROW_DAMAGE = 17;
    public const float PIERCE_ARROW_COST = 12;
    public const float PIERCE_SHOOT_WINDOW = 0.7f;
    public const float PIERCE_SHOOT_CD = 0.1f;
    public const float PIERCE_ABILITY_CD = 10f;

    // Exploding arrow
    public const float EXP_ARROW_SPEED = 10;
    public const float EXP_ARROW_DAMAGE = 20;
    public const float EXP_ARROW_COST = 30;
    public const float EXP_ARROW_CD = 1.2f;

    public const float CASTLE_HEALTH = 1000;

    public const float AI_SCAN_DELAY = 5;

	// Dynamic map constants
	public const float CLOUD_DENSITY = 0.5f;
	public const float CLOUD_SCALE_MIN = 0.5f;
	public const float CLOUD_SCALE_MAX = 1.5f;
	//public const float TREE_DENSITY = 1;
	public const float ELEVATION_CHANGE_WEIGHT = 0.35f; // low value = flatter map, high value = more change. 0 - 1
	public const float ELEVATION_CHANGE_OFFSET = 0.1f;
	public const float HEIGHT_DIFFERENCE_WEIGHT = 0.5f; // low value = downward direction, high value = upward direction. 0 - 1
	public const float HEIGHT_DIFFERENCE_OFFSET = 0.4f;
	public const float PARALLAX_SCALE = 0.01f;
    public const int PLATFORM_LENGTH = 10;
    public const int MAP_LENGTH = 100;
    public const int BASE_LEVEL = 5;
    public const int SOIL_HEIGHT = 8;

    /// <summary>
    /// Gets the mouse position in world space
    /// </summary>
    public static Vector2 MousePosition
    { get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); } }
}
