using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public Transform player;

    public GameObject tile1;
    public GameObject tile2;

    public float tileLength = 100f;
    public float tileOffset = 70f;

    public float extraDistance = 20f;

    private GameObject currentTile;
    private GameObject nextTile;

    void Start()
    {
        currentTile = tile1;
        nextTile = tile2;
    }

    void Update()
    {
        // теперь ждём, пока игрок проедет чуть дальше
        if (player.position.z > currentTile.transform.position.z + tileLength / 2 + extraDistance)
        {
            MoveTile();
        }
    }

    void MoveTile()
    {
        currentTile.transform.position =
            nextTile.transform.position + Vector3.forward * tileOffset;

        GameObject temp = currentTile;
        currentTile = nextTile;
        nextTile = temp;
    }
}