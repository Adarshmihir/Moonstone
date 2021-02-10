using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Interactable/Plant")]
public class Plant : ScriptableObject
{
    public new string name = "New Plant";
    public Sprite icon;
}
