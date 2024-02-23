using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.WFC
{
    public interface IWFC
    {

        //State[,] StateMatrix { get; }
        void UpdateTiles();
        public WFCTile GetTile(Vector3Int index);
        }

    public class WFC_Matrix : IWFC
    {
        protected WFCTile[,] TileMatrix;

        private float m_ET;

        public Vector3Int lastUpdatedPosition;
        public float entropyThreshold {
            get { return this.m_ET; } 
            set { this.m_ET = value; }
        }


        public WFC_Matrix(BoundsInt bounds, WFCTile defaultTile, float EntropyT)
        {
            
            int xSize = Math.Abs(bounds.xMax - bounds.xMin);
            int ySize = Math.Abs(bounds.yMax - bounds.yMin);

            TileMatrix = new WFCTile[xSize, ySize];

            for (int i = 0; i < TileMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < TileMatrix.GetLength(1); j++)
                {
                    TileMatrix[i, j] = defaultTile.Clone();
                    
                }
            }

            entropyThreshold = EntropyT;
        }

        public WFCTile GetTile(Vector3Int index)
        {
            return TileMatrix[index.x, index.y];
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

        private Vector2Int GetMinEntropy()
        {
            float minE = float.MaxValue;
            float tileEntropy;
            Vector2Int minW = new();
            for (int i=0; i< TileMatrix.GetLength(1); i++)
            {
                for(int j=0; j< TileMatrix.GetLength(0); j++)
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
        public void UpdateTiles()
        {
            float minEntropy = float.MaxValue;
            float tileEntropy;
            WFCTile currentTile, nTile;
            int minI=0, minJ=0;
            int radius = 1;
            WFCTile starter = TileMatrix[TileMatrix.GetLength(0) / 2, 0 ];
            if(starter.isNotCollapsed)
            {
                starter.SelectCurrentState();
                lastUpdatedPosition = new Vector3Int(TileMatrix.GetLength(0)/2, 0, 0);
                return;
            }
            for (int i = 0; i < TileMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < TileMatrix.GetLength(1); j++)
                {
                    currentTile = TileMatrix[i, j];
                    foreach (Vector2Int v in getNeighbourIndicesRound(i, j, radius))
                    {
                        nTile = TileMatrix[v.x, v.y];
                        nTile.updateStates(currentTile.currentState.m_allowedNeighbours);
                        
                    }
                    tileEntropy = TileMatrix[i, j].getEntropy();
                    if (tileEntropy <= minEntropy && currentTile.isNotCollapsed)
                    {
                        minEntropy = tileEntropy;
                        minI = i;
                        minJ = j;
                    }
                }
            }
            currentTile = TileMatrix[minI, minJ];
            if(currentTile.isNotCollapsed && minEntropy < entropyThreshold)
            {
                currentTile.SelectCurrentState();
                lastUpdatedPosition = new Vector3Int(minI, minJ, 0);
            }
            //while (minEntropy < entropyThreshold && minEntropy >0  && passes >0)
            //{
            //    passes--;
            //    currentTile = TileMatrix[minI, minJ];
            //    currentTile.SelectCurrentState();
            //    foreach (Vector2Int v in getNeighbourIndicesRound(minI, minJ, radius))
            //    {
            //        nTile = TileMatrix[v.x, v.y];
            //        nTile.updateStates(currentTile.currentState.m_allowedNeighbours);
            //        tileEntropy = nTile.getEntropy();
            //        if (tileEntropy <= minEntropy && !nTile.isCollapsed)
            //        {
            //            minEntropy = tileEntropy;
            //            minI = v.x;
            //            minJ = v.y;
            //        }
            //    }
            //}
        }
    }
}
