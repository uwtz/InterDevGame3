using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetBound();
        BoneStart();
    }

    private void Update()
    {
        SpawnBone();
        MaintainPopulation();
    }



    [Range(1f, 100f)]
    public float timeScale = 1f;

    public GameObject steven;
    public GameObject steve;
    public GameObject beef;
    public GameObject bone;

    int stevenCount;
    int steveCount;
    int beefCount;
    public int boneCount;

    public float boneSpawnRate; // per min
    float boneLastSpawnTime = 0f;

    private void SpawnBone()
    {
        Time.timeScale = timeScale;

        if (Time.time - boneLastSpawnTime > 60f / boneSpawnRate)
        {
            boneLastSpawnTime = Time.time;
            Instantiate(bone, GetRandomPointOnNavMesh(), Quaternion.identity);
            //Instantiate(bone, new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity);

            boneSpawnRate = R(boneCount);
        }
    }

    // R models the spawn rate of bones in a way similar to the derivative of a logistic growth function (rate of change of a function that models growth of a population)
    // kinda like a camel hump, low spawn rate at low and high population, high in the middle
    // https://www.desmos.com/calculator/weprud1ieq

    int maxPopulation = 20; // population where bone spawn rate (ideally) reaches minRate, not the max possible number of bones
    float minRate = 7f; // min rate when population is 0 or maxPopulation
    float maxRate = 24f; // max rate when population is maxPopulation/2
    float compression = 8f; // horizontally shrink the 'bump'. how drastic the change in rate is as population changes.

    // logistic function
    private float F(float x)
    {
        return maxPopulation / (1 + Mathf.Exp(-(compression / maxPopulation) * (x - (maxPopulation / 2))));
    }

    // derivative of F plus some customizations
    private float R(float x)
    {
        return minRate + ((4 * (maxRate - minRate)) / maxPopulation) * F(x) * (1 - (F(x) / maxPopulation));
    }




    private void BoneStart()
    {
        boneSpawnRate = R(boneCount);
        for (int i = 0; i < minRate; i++)
        {
            GameObject b = Instantiate(bone, GetRandomPointOnNavMesh(), Quaternion.identity);
            b.GetComponent<Bone>().growth = 1;
        }
    }

    private void MaintainPopulation()
    {
        stevenCount = GameObject.FindGameObjectsWithTag("Steven").Length;
        steveCount = GameObject.FindGameObjectsWithTag("Steve").Length;
        beefCount = GameObject.FindGameObjectsWithTag("Beef").Length;
        boneCount = GameObject.FindGameObjectsWithTag("Bone").Length + GameObject.FindGameObjectsWithTag("BoneGrowing").Length;
        if (beefCount < 3)
        { Instantiate(beef, GetRandomPointOnNavMesh(), Quaternion.identity); }
        if (steveCount < 2)
        { Instantiate(steve, GetRandomPointOnNavMesh(), Quaternion.identity); }
    }











    [SerializeField] private Transform floor;
    public static Vector2 floorXBound;
    public static Vector2 floorYBound;

    private void SetBound()
    {
        floorXBound = new Vector2(floor.position.x - floor.localScale.x / 2, floor.position.x + floor.localScale.x / 2);
        floorYBound = new Vector2(floor.position.y - floor.localScale.y / 2, floor.position.y + floor.localScale.y / 2);
    }

    // get random point
    public static Vector2 GetRandomPointOnNavMesh()
    {
        Vector2 center = new Vector2(Random.Range(floorXBound[0], floorXBound[1]),
                                     Random.Range(floorYBound[0], floorYBound[1]));

        return GetRandomPointOnNavMesh(center);
    }

    // get random point around given point
    public static Vector2 GetRandomPointOnNavMesh(Vector2 p)
    {
        Vector2 result = new Vector2(Mathf.Infinity, Mathf.Infinity);

        float range = 10.0f;
        for (int i = 0; i < 30; i++)
        {
            Vector2 randomPoint = p + Random.insideUnitCircle * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                break;
            }
        }

        //Debug.Log(result);
        return result;
    }
}
