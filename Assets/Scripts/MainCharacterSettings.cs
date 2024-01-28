using UnityEngine;

[CreateAssetMenu(fileName = "MainCharacterSettings", menuName = "Custom/MainCharacterSettings")]
public class MainCharacterSettings : ScriptableObject
{
    public float MovementSpeed;
    public float MaxJumpHeight;
    public Rock RockPrefab;
}