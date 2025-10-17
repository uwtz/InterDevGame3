using UnityEngine;

public class Steven : ReproducingEntity<Steven>
{
    public override string[] foods => new string[] { "Steve" };

    public override GameObject GetChildPrefab()
    {
        return GameManager.instance.steven;
    }
}
