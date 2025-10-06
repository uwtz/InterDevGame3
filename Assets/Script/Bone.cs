using UnityEngine;

public class Bone : MonoBehaviour
{
    public float growth = 0;
    float growthRate = .1f;
    bool fullyGrown = false;

    float minScale = .1f;
    float maxScale = 1f;

    private void Update()
    {
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
    }

    public bool IsFullyGrown()
    { return fullyGrown; }
}
