using UnityEngine;

public class Bone : Consumable
{
    public float lifetime = 0f;
    private float maxLifetime = 60f;
    
    public float growth = 0;
    float growthRate = .2f;
    bool fullyGrown = false;

    float minScale = .1f;
    float maxScale = 1f;

    private void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime >= maxLifetime && !HasPredator())
        { Destroy(gameObject); }


        // grow
        if (!IsFullyGrown() && growth < 1)
        {
            growth += growthRate * Time.deltaTime;
            if (growth >= 1)
            {
                growth = 1;
                fullyGrown = true;
                tag = "Bone";
            }
        }

        float s = (maxScale - minScale) * growth + minScale;
        transform.localScale = new Vector3(s, s, 0);

        // sprout bones at nearby locations
        if (IsFullyGrown())
        {
            //SproutBonesPeriodically();
        }
    }

    public bool IsFullyGrown()
    { return fullyGrown; }

    /*
    float boneSpawnRate = 1f; // per min
    float boneLastSpawnTime = 0f;
    float range = 5f;
    private void SproutBonesPeriodically()
    {
        if (Time.time - boneLastSpawnTime > 60f / boneSpawnRate)
        {
            boneLastSpawnTime = Time.time;
            Instantiate(GameManager.instance.bone, transform.position + new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0), Quaternion.identity);
        }
    }
    */
}
