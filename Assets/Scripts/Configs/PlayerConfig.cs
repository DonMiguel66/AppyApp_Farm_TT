using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Configs/PlayerConfig", order = 1)]
public class PlayerConfig : ScriptableObject
{
    public float moveSpeed;
    public float rotateSmoothing;
    public int parOfMoneyInView;
    public float moneyFlySpeed;
    public float moneyPickupRadius;
}
