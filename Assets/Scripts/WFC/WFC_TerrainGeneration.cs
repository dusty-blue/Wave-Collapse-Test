using Assets.Scripts.WFC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    
    [SerializeField]  private List<State> stateList;
    [SerializeField] private State initialState;
    [SerializeField] private PlayerInput InputComp;
    [SerializeField]  private InputAction m_CollapseAction;
    
    public TileBase defaultTile
    {
        get { return initialState.m_UnityTile; }
    }

    private List<WFCSocket> GenerateSocketList ()
    {
        
        List<WFCSocket> initList = new List<WFCSocket>(8);

        foreach(State s in stateList)
        {
            foreach (var socket in s.wfcSockets)
            {
                if (!initList.Contains(socket))
                {
                    initList.Add(socket);
                    Debug.Log($"Capacity is {initList.Capacity} and Count is {initList.Count}");
                }
            }
        }
        initList.TrimExcess();
        return initList;
    }

    // Start is called before the first frame update
    void Start()
    {
        WFCTile initWfcTile = new WFCTile(stateList.ToArray(),initialState);

        m_WFCMatrix = new WFC_Matrix(area, initWfcTile, entropyThreshold,GenerateSocketList());
        Tilemap tilemap = GetComponent<Tilemap>();
        TileBase[] tileArray = tilemap.GetTilesBlock(area);
        
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
    }

    public void ResetMatrix()
    {
        //TO DO Reset all Sockets as well
        m_WFCMatrix.SetAllTiles(new WFCTile(stateList.ToArray(),initialState));
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
    private Vector3Int ConvertWorldToTileIndex(Vector3 worldPos)
    {
        Transform t = this.GetComponentInParent<Transform>();
        Vector3 localPos = t.InverseTransformPoint(worldPos);
        return m_tilemap.LocalToCell(localPos);
    }
    public void ResetTile(Vector3 worldPos )
    {
        Vector3Int cellPos = ConvertWorldToTileIndex(worldPos);
        m_WFCMatrix.SetTile(cellPos, new WFCTile(initialState));
    }

    public void LockTile(Vector3 worldPos,float lockTime)
    {
        Vector3Int cellPos = ConvertWorldToTileIndex(worldPos);
        StartCoroutine(m_WFCMatrix.LockTile(cellPos, lockTime));
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
        if(context.started) {
            Vector2 screenPos = context.ReadValue<Vector2>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y));
            CollapseTile(worldPos);
        }

    }
}
