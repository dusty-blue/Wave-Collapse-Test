using Assets.Scripts.WFC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class WFC_TerrainGeneration : MonoBehaviour
{
    [SerializeField] private Tilemap m_tilemap;
    [SerializeField] private Sprite rock,paper,scissor;
    [SerializeField] private float entropyThreshold, rockWeight, paperWeight, scissorWeight;
    private UnityEngine.Tilemaps.Tile m_rockTile;
    private float m_deltaTimeSum =0;
    public BoundsInt area;

    private WFC_Matrix m_WFCMatrix;

    private Dictionary<string, State> stateDic;

    void initStateDic ()
    {
        State grass = new State("Grass", scissorWeight);

        Assert.AreEqual("Grass", grass.m_name);

        State shrubs = new State("Shrubs", paperWeight);

        State trees = new State("Trees", rockWeight);

        State initial = new State("Init", 0f);

        initial.m_allowedNeighbours = new[] { grass, shrubs, trees };
        grass.m_allowedNeighbours = new[] { grass, shrubs };
        shrubs.m_allowedNeighbours = new[] { grass, shrubs, trees };
        trees.m_allowedNeighbours = new[] { shrubs };

        stateDic = new Dictionary<string, State>();
        stateDic.Add(initial.m_name, initial);
        stateDic.Add(grass.m_name, grass);
        stateDic.Add(shrubs.m_name, shrubs);
        stateDic.Add(trees.m_name, trees);
    }

    // Start is called before the first frame update
    void Start()
    {
        initStateDic();
        m_rockTile = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
        m_rockTile.sprite = rock;
        m_WFCMatrix = new WFC_Matrix(area, new WFCTile(stateDic["Init"]), entropyThreshold);
        Tilemap tilemap = GetComponent<Tilemap>();
        TileBase[] tileArray = tilemap.GetTilesBlock(area);
    }

    // Update is called once per frame
    void Update()
    {
        
        m_deltaTimeSum += Time.deltaTime;
        m_WFCMatrix.UpdateTiles();
        State newState = m_WFCMatrix.GetTile(m_WFCMatrix.lastUpdatedPosition).currentState;

        switch (newState.m_name)
        {
            case "Grass":
                m_rockTile.sprite = scissor;
                break;
            case "Shrubs":
                m_rockTile.sprite = paper;
                break;
            case "Trees":
                m_rockTile.sprite = rock;
                break;
            default:
                m_rockTile.sprite = scissor;
                break;
        }
        m_tilemap.SetTile(m_WFCMatrix.lastUpdatedPosition, m_rockTile);
        m_rockTile.RefreshTile(m_WFCMatrix.lastUpdatedPosition, m_tilemap);
    }
}
