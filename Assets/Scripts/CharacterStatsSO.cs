using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatsSO", menuName = "Scriptable Objects/CharacterStatsSO")]
public class CharacterStatsSO : ScriptableObject
{
    [SerializeField]
    public int maxHealth = 1000;
    public float movementSpeed = 1f;
    public int attack = 1000;
}
