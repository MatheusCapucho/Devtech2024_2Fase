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

        if(!tilemap.GetSprite(hitPoint) != _virusTile.sprite)
        {

            tilemap.SetTile(hitPoint, _virusTile);
        }
        

    }
}
