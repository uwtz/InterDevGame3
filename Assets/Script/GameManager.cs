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

    float boneSpawnRate = 10f; // per min
    float boneLastSpawnTime = 0f;

    private void SpawnBone()
    {
        Time.timeScale = timeScale;

        if (Time.time - boneLastSpawnTime > 60f / boneSpawnRate)
        {
            boneLastSpawnTime = Time.time;
            Instantiate(bone, GetRandomPointOnNavMesh(), Quaternion.identity);
            //Instantiate(bone, new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity);
        }
    }


    [SerializeField] private Transform floor;
    public static Vector2 floorXBound;
    public static Vector2 floorYBound;

    private void Start()
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



    public int beefCount;
    public int steveCount;
    private void MaintainPopulation()
    {
        beefCount = GameObject.FindGameObjectsWithTag("Beef").Length;
        steveCount = GameObject.FindGameObjectsWithTag("Steve").Length;
        if (beefCount < 1)
        { Instantiate(beef, GetRandomPointOnNavMesh(), Quaternion.identity); }
        if (steveCount < 2)
        { Instantiate(steve, GetRandomPointOnNavMesh(), Quaternion.identity); }
    }
}
