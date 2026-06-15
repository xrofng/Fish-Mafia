using Sirenix.OdinInspector;
using UnityEngine;

public class CloudPopulator : MonoBehaviour
{
    [Header("Spawn Count")]
    public int spawnCount = 20;

    [Header("Spawn Area (X/Y)")]
    public Vector2 xRange = new Vector2(-10, 10);
    public Vector2 yRange = new Vector2(-5, 5);

    [Header("Z Range")]
    public Vector2 zRange = new Vector2(0, 10);

    [Header("Uniform Scale Range")]
    public Vector2 scaleRange = new Vector2(0.5f, 2f);

    [Header("Sprite")]
    public Sprite[] CloudSprites;

    [Header("Parent")]
    public Transform parent;

    [Button("Populate Clouds")]
    public void Populate()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // Random position
            float x = Random.Range(xRange.x, xRange.y);
            float y = Random.Range(yRange.x, yRange.y);
            float z = Random.Range(zRange.x, zRange.y);

            Vector3 pos = new Vector3(x, y, z);

            // Instantiate
            GameObject cloud = Instantiate(new GameObject(), pos, Quaternion.identity, parent);
            SpriteRenderer spr = cloud.AddComponent<SpriteRenderer>();
            spr.sprite = CloudSprites[Random.Range(0, CloudSprites.Length)];
            spr.flipX = Random.Range(0, 2) >= 1 ? true : false;

            // Uniform scale
            float scale = Random.Range(scaleRange.x, scaleRange.y);
            cloud.transform.localScale = Vector3.one * scale;
        }
    }
}