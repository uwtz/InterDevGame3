using UnityEngine;

public class Consumable : MonoBehaviour
{
    GameObject predator;

    public void ClaimConsumable(GameObject go)
    { predator = go; }

    public bool HasPredator()
    { return predator != null; }

    protected bool isDead = false;
    public void Kill()
    {
        isDead = true;
        Destroy(gameObject);
    }

    public SpriteRenderer sr;

    public virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    public virtual void Update()
    {
        sr.sortingOrder = (int)-transform.position.y;
    }
}
