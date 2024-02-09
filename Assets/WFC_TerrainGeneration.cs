using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WFC_TerrainGeneration : MonoBehaviour
{
    [SerializeField] private Tilemap m_tilemap;
    [SerializeField] private Sprite rock,paper,scissor;
    private Tile m_rockTile;
    private float m_deltaTimeSum =0;
    public BoundsInt area;

    // Start is called before the first frame update
    void Start()
    {
        m_rockTile = ScriptableObject.CreateInstance<Tile>();
        m_rockTile.sprite = rock;

        Tilemap tilemap = GetComponent<Tilemap>();
        TileBase[] tileArray = tilemap.GetTilesBlock(area);
        for (int index = 0; index < tileArray.Length; index++)
        {
            print(tileArray[index]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_tilemap.SetTile(new Vector3Int(0, 0, 0), m_rockTile);
        m_deltaTimeSum += Time.deltaTime;
        if(m_deltaTimeSum > 10)
        {
            TileBase p = m_tilemap.GetTile(new Vector3Int(0, 0, 0));
            m_rockTile.sprite = paper;
            m_tilemap.SetTile(new Vector3Int(0, 0, 0), m_rockTile);
            m_deltaTimeSum = 0;
        }
    }
}
