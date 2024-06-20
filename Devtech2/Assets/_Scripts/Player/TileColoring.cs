using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileColoring : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Tile _virusTile;
    [SerializeField] private int _maxColors = 99;
    [SerializeField] private bool _isPlayerRelated = true;

    public void SetMaxColors(int max)
    {
        _maxColors = max;
    }
    private void Awake()
    {
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
    }

    private void FixedUpdate()
    {
        CheckTiles();
    }

    private void CheckTiles()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.55f, groundMask);
        if (hit.collider == null)
            return;

        var hitPoint = tilemap.WorldToCell(hit.point) + Vector3Int.down;

        if(tilemap.GetSprite(hitPoint) != _virusTile.sprite && _maxColors > 0)
        {
            tilemap.SetTile(hitPoint, _virusTile);
            if(_isPlayerRelated)
            {
                GameManager.VirusTiles++;
                _maxColors--; 
            } else
            {
                GameManager.VirusTiles--;
            }
            if(_maxColors <= 0)
            {

            }
        }
        

    }
}
