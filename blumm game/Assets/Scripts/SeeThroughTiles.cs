using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SeeThroughTiles : MonoBehaviour
{
    List<Vector3Int> cellPositions;
    private Tilemap map;
    private Color basicColor = new Color(1f, 1f, 1f, 1f);
    private Color TransparentColor = new Color(1f, 1f, 1f, 0.5f);
    public BoxCollider2D playerCol;
    public float playerGroundColCenterX;
    public float extent;
    private bool playerInside = false;
    // Start is called before the first frame update
    void Start()
    {

        cellPositions = new List<Vector3Int>();
        map = transform.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!playerInside)
        {
            playerInside = true;

            BoxCollider2D playerGroundCol = playerCol;
            playerGroundColCenterX = playerGroundCol.bounds.center.x;
            extent = playerGroundCol.bounds.extents.x;
            Vector3Int tempTile = map.WorldToCell(new Vector3(playerGroundColCenterX - extent - 0.5f, playerGroundCol.bounds.center.y));
            if (map.GetTile(tempTile))
            {
                GetTilesLeft(tempTile);
            }
            if (map.GetTile(map.WorldToCell(new Vector3(playerGroundColCenterX + extent + 0.5f, playerGroundCol.bounds.center.y))))
            {
                tempTile = map.WorldToCell(new Vector3(playerGroundColCenterX + extent + 0.5f, playerGroundCol.bounds.center.y));
                GetTilesRight(tempTile);
            }
            MakeTilesTransparent();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        RemoveTransparency();
        playerInside = false;
    }
    void GetTilesRight(Vector3Int firstTileToShow)
    {
        Vector3Int curTileCellPos = firstTileToShow;
        while (map.GetTile(curTileCellPos))
        {
            cellPositions.Add(curTileCellPos);
            GetTilesUp(curTileCellPos);
            GetTilesDown(curTileCellPos);
            curTileCellPos = map.WorldToCell(new Vector3(curTileCellPos.x + 1.2f, curTileCellPos.y, 0));
        }
    }
    void GetTilesLeft(Vector3Int firstTileToShow)
    {
        Vector3Int curTileCellPos = firstTileToShow;
        while (map.GetTile(curTileCellPos))
        {
            cellPositions.Add(curTileCellPos);
            GetTilesUp(curTileCellPos);
            GetTilesDown(curTileCellPos);
            curTileCellPos = map.WorldToCell(new Vector3(curTileCellPos.x - 0.5f, curTileCellPos.y, 0));
        }
    }
    void GetTilesUp(Vector3Int curTile)
    {
        curTile = map.WorldToCell(new Vector3(curTile.x, curTile.y + 1.2f, 0));
        while (map.GetTile(curTile))
        {
            cellPositions.Add(curTile);
            curTile = map.WorldToCell(new Vector3(curTile.x, curTile.y + 1.2f, 0));
        }
    }
    void GetTilesDown(Vector3Int curTile)
    {
        curTile = map.WorldToCell(new Vector3(curTile.x, curTile.y - 0.5f, 0));
        while (map.GetTile(curTile))
        {
            cellPositions.Add(curTile);
            curTile = map.WorldToCell(new Vector3(curTile.x, curTile.y - 0.5f, 0));
        }
    }
    void MakeTilesTransparent()
    {

        for (int i = 0; i < cellPositions.Count; i++)
        {
            map.RemoveTileFlags(cellPositions[i], TileFlags.LockColor);
            map.SetColor(cellPositions[i], TransparentColor);
        }
    }

    void RemoveTransparency()
    {
        foreach (Vector3Int pos in cellPositions)
        {
            map.SetColor(pos, basicColor);
        }
        cellPositions.Clear();
    }
}
