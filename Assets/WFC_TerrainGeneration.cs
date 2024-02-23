using Assets.Scripts.WFC;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    [SerializeField]  private List<State> stateList;
    [SerializeField] private State initialState;

    void initStateDic ()
    {
        stateDic = new Dictionary<string, State>();

        List<NeighbourState> initList = new List<NeighbourState>(stateList.Count+1);
        stateDic.Add(initialState.m_name, initialState);

        foreach(State s in stateList)
        {
            
            stateDic.Add(s.m_name, s);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_rockTile = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
        m_rockTile.sprite = rock;
        initStateDic();
        m_WFCMatrix = new WFC_Matrix(area, new WFCTile(initialState), entropyThreshold);
        Tilemap tilemap = GetComponent<Tilemap>();
        TileBase[] tileArray = tilemap.GetTilesBlock(area);
    }

    // Update is called once per frame
    void Update()
    {
        m_deltaTimeSum += Time.deltaTime;
        m_WFCMatrix.UpdateTiles();
        State newState = m_WFCMatrix.GetTile(m_WFCMatrix.lastUpdatedPosition).currentState;

        m_tilemap.SetTile(m_WFCMatrix.lastUpdatedPosition, newState.m_UnityTile);
        m_rockTile.RefreshTile(m_WFCMatrix.lastUpdatedPosition, m_tilemap);
    }
}
