using Assets.Scripts.WFC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class WFC_TerrainGeneration : MonoBehaviour
{
    [SerializeField] private Tilemap m_tilemap;
    [SerializeField] private Sprite rock,paper,scissor;
    [SerializeField] private float entropyThreshold, rockWeight, paperWeight, scissorWeight;
    [SerializeField]  private UnityEngine.Tilemaps.Tile impossibleTile;
    private float m_deltaTimeSum =0;
    public BoundsInt area;

    private WFC_Matrix m_WFCMatrix;

    private Dictionary<string, State> stateDic;
    [SerializeField]  private List<State> stateList;
    [SerializeField] private State initialState;
    [SerializeField] private InputActionAsset InputActions;
    private InputAction m_CollapseAction;
    
    public TileBase defaultTile
    {
        get { return initialState.m_UnityTile; }
    }

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
        
        initStateDic();
        m_WFCMatrix = new WFC_Matrix(area, new WFCTile(initialState), entropyThreshold);
        Tilemap tilemap = GetComponent<Tilemap>();
        TileBase[] tileArray = tilemap.GetTilesBlock(area);
        m_CollapseAction = InputActions.FindAction("Collapse");

        m_CollapseAction.started += CollapseCallBack;
    }

    // Update is called once per frame
    void Update()
    {
        m_deltaTimeSum += Time.deltaTime;
        m_WFCMatrix.UpdateTiles(false);
        State newState;
        //Vector3Int lastPos = m_WFCMatrix.lastUpdatedPosition;
        foreach(Vector3Int pos in m_WFCMatrix.updateQueue)
        {
            newState= m_WFCMatrix.GetTile(pos).currentState;
            if (m_WFCMatrix.GetTile(pos).isImpossible)
            {

                m_tilemap.SetTile(pos, impossibleTile);
                impossibleTile.RefreshTile(pos, m_tilemap);
            } else
            {
                m_tilemap.SetTile(pos, newState.m_UnityTile);
                newState.m_UnityTile.RefreshTile(pos, m_tilemap);
            }

            
        }
        m_WFCMatrix.clearQueue();
        //newState = m_WFCMatrix.GetTile(lastPos).currentState;
        //if (m_WFCMatrix.GetTile(lastPos).isImpossible)
        //{

        //    m_tilemap.SetTile(lastPos, impossibleTile);
        //    impossibleTile.RefreshTile(lastPos, m_tilemap);
        //}

        //m_tilemap.SetTile(lastPos, newState.m_UnityTile);
        //newState.m_UnityTile.RefreshTile(lastPos, m_tilemap);
    }

    public void ResetMatrix()
    {
        m_WFCMatrix.SetAllTiles(new WFCTile(initialState));
    }

    public void ResetMatrix(float newEntropy)
    {
        m_WFCMatrix.entropyThreshold = newEntropy;
        entropyThreshold = newEntropy;
        ResetMatrix();
    }

    public void SetAllTiles(TileBase t)
    {
        m_tilemap.ClearAllTiles();
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y * area.size.z];
        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i] = t;
        }
        m_tilemap.SetTilesBlock(area, tileArray);
    }

    public void CollapseTile(Vector3 worldPos)
    {
        Transform t =this.GetComponentInParent<Transform>();
        Vector3 localPos = t.InverseTransformPoint(worldPos);
        Vector3Int cellPos = m_tilemap.LocalToCell(localPos);
        m_WFCMatrix.CollapseTile(new Vector2Int(cellPos.x, cellPos.y), 30, 1,true);

    }

    public void  CollapseCallBack (InputAction.CallbackContext context)
    {
        Vector2 screenPos = context.ReadValue<Vector2>();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y));
        CollapseTile(worldPos);

    }
}
