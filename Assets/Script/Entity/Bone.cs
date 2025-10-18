using UnityEngine;

public class Bone : Consumable
{
    public float lifetime = 0f;
    private float maxLifetime = 120f;
    
    public float growth = 0;
    float growthRate = .2f;
    bool isFullyGrown = false;

    float minScale = .5f;
    float maxScale = 1f;

    [SerializeField] Sprite[] sprites;

    public override void Update()
    {
        base.Update();

        lifetime += Time.deltaTime;
        if (lifetime >= maxLifetime && !HasPredator())
        { Kill(); }


        // grow
        if (!isFullyGrown && growth < 1)
        {
            growth += growthRate * Time.deltaTime;
        }
        if (growth >= 1)
        {
            growth = 1;
            isFullyGrown = true;
            tag = "Bone";
        }

        float s = (maxScale - minScale) * growth + minScale;
        transform.localScale = new Vector3(s, s, 0);

        // sprout bones at nearby locations
        if (isFullyGrown)
        {
            //SproutBonesPeriodically();
        }



        if (growth < .5) { sr.sprite = sprites[0]; }
        else if (growth < 1) { sr.sprite = sprites[1]; }
        else { sr.sprite = sprites[2]; }
    }

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
