using UnityEngine;

public class Consumable : MonoBehaviour
{
    GameObject predator;

    public void ClaimConsumable(GameObject go)
    { predator = go; }

    public bool HasPredator()
    { return predator != null; }


    // stats: energy
}
