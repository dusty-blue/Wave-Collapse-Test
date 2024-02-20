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

    void initStateDic ()
    {
        stateDic = new Dictionary<string, State>();
        State initial =  ScriptableObject.CreateInstance<State>();
        initial.m_name = "Init";
        initial.m_spawnWeight = 0f;
        initial.m_Neighbourweight = new float[0];
        initial.m_UnityTile = m_rockTile;

        List<NeighbourState> initList = new List<NeighbourState>(stateList.Count); 

        foreach(State s in stateList)
        {
            initList.Add(new NeighbourState(s, s.m_spawnWeight));
            stateDic.Add(s.m_name, s);
        }
        initial.m_allowedNeighbours = initList.ToArray();
        AssetDatabase.CreateAsset(initial, "Assets/Scripts/WFC/States/Test/NInit/init.asset");
        stateDic.Add(initial.m_name, initial);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_rockTile = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
        m_rockTile.sprite = rock;
        initStateDic();
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

        m_tilemap.SetTile(m_WFCMatrix.lastUpdatedPosition, newState.m_UnityTile);
        m_rockTile.RefreshTile(m_WFCMatrix.lastUpdatedPosition, m_tilemap);
    }
}
