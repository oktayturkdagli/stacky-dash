using UnityEngine;

[CreateAssetMenu(fileName = "SOUserData", menuName = "ScriptableObjects/User Data")]
public class SOUserData : ScriptableObject
{
    [SerializeField] public int currentLevel = 1;
    [SerializeField] public int highestLevel = 1;
    [SerializeField] public int totalGold = 0;
    [SerializeField] public int bestTotalGold = 0;
    [SerializeField] public int totalStack = 0;
}