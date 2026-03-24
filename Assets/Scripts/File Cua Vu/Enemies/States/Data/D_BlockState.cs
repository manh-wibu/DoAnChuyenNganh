using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newBlockStateData", menuName = "Data/State Data/Block State")]
public class D_BlockState : ScriptableObject
{
    [Header("Block Settings")]
    public float blockRadius = 0.5f;
    public float blockDuration = 1f;  // Thời gian block tối đa
    
    
    [Header("Block Direction")]
    public float blockAngleRange = 90f;  // Tầm góc chặn (VD: 90 độ trước mặt)
    
    [Header("Blocking Reduction")]
    public float poiseReductionPercent = 0.7f;    // % giảm poise damage (0-1)
    public float knockbackReductionPercent = 0.8f; // % giảm knockback (0-1)
    public float damageReductionPercent = 0.5f;  // % giảm sát thương (0-1)
    
    public LayerMask whatIsPlayer;  // Layer của kẻ tấn công
}
