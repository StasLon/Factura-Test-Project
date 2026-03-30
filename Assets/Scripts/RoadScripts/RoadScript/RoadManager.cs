using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private Transform player;

    [Header("Road Tiles")]
    [SerializeField] private GameObject tile1;
    [SerializeField] private GameObject tile2;

    [Header("Tile Settings")]
    [SerializeField] private float tileLength = 100f;
    [SerializeField] private float tileOffset = 70f;
    [SerializeField] private float extraDistance = 20f; 

    private GameObject _currentTile;
    private GameObject _nextTile;

    private void Start()
    {
        _currentTile = tile1;
        _nextTile = tile2;
    }

    private void Update()
    {
        if (IsPlayerPastCurrentTile())
        {
            SwapAndMoveTiles();
        }
    }

    private bool IsPlayerPastCurrentTile()
    {
        return player.position.z > _currentTile.transform.position.z + tileLength / 2 + extraDistance;
    }

    private void SwapAndMoveTiles()
    {
        _currentTile.transform.position = _nextTile.transform.position + Vector3.forward * tileOffset;

        (_currentTile, _nextTile) = (_nextTile, _currentTile);
    }
}