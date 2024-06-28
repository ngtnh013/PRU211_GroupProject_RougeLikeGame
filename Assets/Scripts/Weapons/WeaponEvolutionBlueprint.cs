using UnityEngine;

[CreateAssetMenu(fileName = "WeaponEvolutionBlueprint", menuName = "ScriptableObjects/WeaponEvolutionBlueprint")]
public class WeaponEvolutionBlueprint : ScriptableObject
{
    public WeaponScriptableObject baseWeaponData;
    public PassiveItemScriptableObject catalystItem;
    public WeaponScriptableObject evolvedWeaponData;
    public GameObject evolvedWeapon;
}
