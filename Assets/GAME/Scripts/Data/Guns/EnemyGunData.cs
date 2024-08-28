using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObjects/EnemyGunData", order = 0)]
public class EnemyGunData : ScriptableObject
{
    public enum FireMode
    {
        Aimed,
        Fixed
    }
    [Header("Reference")]
    [Tooltip("Bullet projectile to fire.")]
    public GameObject bulletRef = null;
    [Header("Settings")]
    [Tooltip("How many bullets to fire at once")]
    public int bulletAmount = 1;
    [Tooltip("How many bullets per mag")]
    public int magAmount = 1;
    [Tooltip("How long between each bullet (in seconds)")]
    public float fireRate = 1f;
    [Tooltip("How long between each mag (in seconds)")]
    public float reload = 5f;
    [Tooltip("Spread in degrees of the bullets in a mag")]
    public float spread = 0f;
    [Tooltip("Spread shotgun shells or not")]
    public bool spreadShells = false;
    [Tooltip("Spread Mag or not")]
    public bool spreadMag = false;
    [Tooltip("Whether to aim at the player or not")]
    public FireMode fireMode = FireMode.Fixed;
}
