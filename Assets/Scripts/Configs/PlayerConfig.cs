using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Configs/PlayerConfig", order = 1)]
public class PlayerConfig : ScriptableObject
{
    public float moveSpeed;
    public float rotateSmoothing;
    public int parOfMoneyInView;
    public float moneyMoveSpeed;
    public float moneyScaleChangeSpeed;
    public float moneyPickupRadius;
    public int _plantGrowthTime;
    public int _plantBuiltCost;
    public int neededAviaryPlantsCount;
}
