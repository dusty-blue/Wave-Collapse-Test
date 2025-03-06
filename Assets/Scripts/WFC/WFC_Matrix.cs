using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.TerrainTools;

namespace Assets.Scripts.WFC
{
    public class WFC_Matrix : IWFC
    {
        protected WFCTile[,] TileMatrix;
        protected List<WFCSocket>[,] SocketMatrix;
        protected List<WFCSocket> allSockets;

        private float m_ET;

        public Vector3Int lastUpdatedPosition { 
            get {
                m_updateQueue.TryDequeue(out m_Last);
                return m_Last;
            } 
            set { m_Last = value; } 
        }
        private Queue<Vector3Int> m_updateQueue;
        private Vector3Int m_Last;
        public Vector3Int[] updateQueue { get { return m_updateQueue.ToArray(); } }
        public float entropyThreshold {
            get { return this.m_ET; } 
            set { this.m_ET = value; }
        }

        public void clearQueue()
        {
            m_updateQueue.Clear();
        }
        public WFC_Matrix(BoundsInt bounds, WFCTile defaultTile, float EntropyT, List<WFCSocket> allSockets)
        {
            
            int xSize = Math.Abs(bounds.xMax - bounds.xMin);
            int ySize = Math.Abs(bounds.yMax - bounds.yMin);
            this.allSockets = allSockets;

            TileMatrix = new WFCTile[xSize, ySize];
            SocketMatrix = new List<WFCSocket>[(xSize+1)*2, ySize+1];

            SetAllTiles(defaultTile);

            entropyThreshold = EntropyT;
            m_updateQueue = new Queue<Vector3Int>();
        }

        public WFCTile GetTile(Vector3Int index)
        {
            return TileMatrix[index.x, index.y];
        }

        public WFCSocket[] GetSocket( int x, int y)
        {
            if (SocketMatrix[x , y] != null)
            {
                return SocketMatrix[x,y].ToArray();
            }
            else
            {
                return allSockets.ToArray();
            }
        }

        public WFCSocket[][] GetSockets(Vector3Int index)
        {
            return GetSockets(new Vector2Int(index.x, index.y));
        }

        public WFCSocket[][] GetSockets(Vector2Int index)
        {
            WFCSocket[][] newSockets = new WFCSocket[4][];

            newSockets[0] = GetSocket( index.x * 2, index.y);
            newSockets[1] = GetSocket( index.x * 2 + 1, index.y + 1);
            newSockets[2] = GetSocket( (index.x + 1) * 2, index.y);
            newSockets[3] = GetSocket( index.x * 2 + 1, index.y);
            return newSockets;
        }

        public void UpdateSockets(Vector2Int index, WFCSocket[][] newSockets)
        {
            SocketMatrix[index.x * 2, index.y] = newSockets[0].ToList();
            SocketMatrix[index.x * 2 + 1, index.y + 1] = newSockets[1].ToList();
            SocketMatrix[(index.x + 1) * 2, index.y] = newSockets[2].ToList();
            SocketMatrix[index.x * 2 + 1, index.y] = newSockets[3].ToList();
        }

        private bool IsInBoundsX(int i)
        {
            return i >= 0 && i < TileMatrix.GetLength(0);
        }
        private bool IsInBoundsY(int j)
        {
            return j >= 0 && j < TileMatrix.GetLength(1);
        }
        protected bool IsInBounds(int i, int j)
        {
            return IsInBoundsX(i) && IsInBoundsY(j);
        }

        protected List<Vector2Int> getNeighbourIndices(int i, int j, int r)
        {
            
            List<Vector2Int> indices = new List<Vector2Int>();
            for(int rI =-r; rI<= r; rI++)
            {
                for(int rJ =-r; rJ<= r;rJ++)
                {
                    if (IsInBounds(i + rI, j + rJ) && !(rI == 0 && rJ == 0))
                    {
                         indices.Add(new Vector2Int(i + rI, j + rJ));
                    }
                }
            }
            return indices;
            
        }
        protected List<Vector2Int> getNeighbourIndicesRound(int i, int j, int r)
        {
            List<Vector2Int> indices = new  List<Vector2Int>();
            Vector2Int center = new Vector2Int(i, j);
            for (int rI = -r; rI <= r; rI++)
            {
                for (int rJ = -r; rJ <= r; rJ++)
                {
                    if (IsInBounds(i + rI, j + rJ) && !(rI == 0  && rJ  == 0))
                    {Vector2Int v = new Vector2Int(i + rI, j + rJ);
                        if((v-center).magnitude <= r){indices.Add(v);}
                    }
                }
            }
            return indices;

        }

        private Vector2Int MinEntropy { get
            {
                float minE = float.MaxValue;
                float tileEntropy;
                Vector2Int minW = new();
                for (int i = 0; i < TileMatrix.GetLength(1); i++)
                {
                    for (int j = 0; j < TileMatrix.GetLength(0); j++)
                    {
                        tileEntropy = TileMatrix[j, i].getEntropy();
                        if (tileEntropy <= minE && TileMatrix[j, i].isNotCollapsed)
                        {
                            minE = tileEntropy;
                            minW.y = i;
                            minW.x = j;
                        }
                    }
                }
                return minW;
            }
        }
        public void UpdateTiles(bool forceUpdate)
        {
            
            int radius = 1;
            int passes = 150;

            CollapseTile(MinEntropy, passes, radius, forceUpdate);

            
        }

        public void SetAllTiles(WFCTile tile)
        {
            for (int i = 0; i < TileMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < TileMatrix.GetLength(1); j++)
                {
                    TileMatrix[i, j] = tile.Clone();

                }
            }
        }

        public Boolean IsNotCollapsed(Vector3Int index)
        {
            return TileMatrix[index.x, index.y].isNotCollapsed;
        }

        public void SetTile(Vector3Int index, WFCTile tile)
        {
            TileMatrix[index.x, index.y] = tile.Clone();
            UpdateTile(index);
            TileMatrix[index.x, index.y].isLocked = true;
            m_updateQueue.Enqueue(index);
        }

        public void UpdateTile(Vector3Int index)
        {
            WFCTile tile = TileMatrix[index.x, index.y];
            foreach(Vector2Int v in getNeighbourIndices(index.x, index.y, 1))
            {
                tile.TryUpdateStates(GetSockets(index));
                //TODO make directional
            }
        }

        public IEnumerator LockTile(Vector3Int index, float lockTime)
        {
            WFCTile tile = TileMatrix[index.x, index.y];
            return tile.LockForSecs(lockTime);
        }

        public void CollapseTile(Vector2Int index, int maxPasses, int radius, bool forceUpdate)
        {
            WFCTile currentTile = TileMatrix[index.x, index.y];
            if(currentTile.isLocked) { return; }
            
            Queue<Vector2Int> indexQueue = new();
            indexQueue.Enqueue(index);
            /*Collapse the selected tile */
            if (currentTile.isNotCollapsed && (currentTile.getEntropy() < entropyThreshold || forceUpdate))
            {
                //UpdateStates before this?
                WFCSocket[][] currentSockets = GetSockets(index);
                currentTile.updateStates(currentSockets);
                currentTile.SelectCurrentState();
                UpdateSockets(index, currentTile.currentState.ReturnRotatedPlacement(currentSockets));
                
                m_updateQueue.Enqueue(new(index.x, index.y, 0));
            }

            /*Update all neighbour tiles*/
            WFCTile nTile;
            int passes = 0;
            // TODO currentTile.currentState.ReturnRotatedPlacement()
            // Requires Neighbouring Sockets
            while (indexQueue.TryDequeue(out Vector2Int currentIndex) && passes < maxPasses)
            {
                passes++;
                currentTile = TileMatrix[currentIndex.x, currentIndex.y];
                foreach (Vector2Int v in getNeighbourIndices(currentIndex.x, currentIndex.y, radius))
                {
                    nTile = TileMatrix[v.x, v.y];
                    if (nTile.isNotCollapsed && nTile.TryUpdateStates(GetSockets(v))) //&&!nTile.TryUpdateStates(currentTile.currentState.m_allowedNeighbours
                    {
                        //TODO also update sockets
                        if (!indexQueue.Contains(v))
                        {
                            indexQueue.Enqueue(v);
                        }
                    }

                }
            }

        }

        public int TransformIndicesToDirection(Vector2Int fromPoint, Vector2Int toPoint)
        {
            Vector2Int directionV = fromPoint - toPoint;
            //directionV = directionV / (int)directionV.magnitude;
            int directionArray = -1;
            if (directionV.Equals(Vector2Int.up))
            {
                directionArray = 0;
            } else if (directionV.Equals(Vector2Int.right))
            {
                directionArray = 1;
            } else if (directionV.Equals(Vector2Int.down))
            {
                directionArray = 2;
            }
            else if (directionV.Equals(Vector2Int.left))
            {
                directionArray = 3;
            }
            return directionArray;
        }
    }
}
