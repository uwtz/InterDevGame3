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

    [Range(1f, 100f)]
    public float timeScale = 1f;

    [SerializeField] public GameObject steve;
    [SerializeField] public GameObject beef;
    [SerializeField] public GameObject bone;

    float boneSpawnRate = 10f; // per min
    float boneLastSpawnTime = 0f;

    private void Update()
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



    public static Vector2 GetRandomPointOnNavMesh()
    {
        Vector2 center = new Vector2(Random.Range(floorXBound[0], floorXBound[1]),
                                     Random.Range(floorYBound[0], floorYBound[1]));

        return GetRandomPointOnNavMesh(center);
    }

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
