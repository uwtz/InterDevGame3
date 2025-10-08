using UnityEngine;

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
            Instantiate(bone, new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity);
        }
    }


    [SerializeField] private Transform floor;
    public Vector2 floorXBound;
    public Vector2 floorYBound;

    private void Start()
    {
        floorXBound = new Vector2(floor.position.x - floor.localScale.x / 2, floor.position.x + floor.localScale.x / 2);
        floorYBound = new Vector2(floor.position.y - floor.localScale.y / 2, floor.position.y + floor.localScale.y / 2);
    }
}
