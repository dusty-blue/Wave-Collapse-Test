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
    public class State
    {
        //make scriptable object
        public string name;
        public float spawnChance;
        public State[] allowedNeighbours;
    }
    public class Tile
    {
        public State currentState;
        public State[] possibleStates;
        public Tile(State[] states, State starting )
        {
            possibleStates = states;
            currentState = starting;
        }
        public Tile(State starting)
        {
            currentState = starting;
            possibleStates = starting.allowedNeighbours;
        }
        public float getEntropy()
        {
            float sum = 0;
            foreach(State s in possibleStates)
            {
                sum += s.spawnChance;
            }
            return sum;
        }
        public void updateStates(State[] neighbours)
        {
            possibleStates = neighbours.Intersect(possibleStates).ToArray();
        }
        public void SelectCurrentState()
        {
            System.Random rdm = new System.Random();
            currentState = possibleStates[rdm.Next(possibleStates.Length)]; //TO DO consider spawnrates

        }
    }


    public interface IWFC
    {

        //State[,] StateMatrix { get; }
        void UpdateTiles();
        public Tile GetTile(Vector3Int index);
        float EntropyThreshold {get;set;} 
    }

    public class WFC_Matrix : IWFC
    {
        Tile[,] TileMatrix;
        
        WFC_Matrix(BoundsInt size)
        {
            State grass = new State();
            grass.name = "Grass";
            grass.spawnChance = 0.5f;
            TileMatrix = new Tile[size.x, size.y];

            for(int i = 0; i < TileMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < TileMatrix.GetLength(1); j++)
                {
                    TileMatrix[i, j] = new Tile(new[] { grass }, grass);
                    //TO DO Constructor and States
                }
            }
        }

        public float EntropyThreshold { get => EntropyThreshold; set => EntropyThreshold = value; }

        public Tile GetTile(Vector3Int index)
        {
            return TileMatrix[index.x, index.y];
        }

        private bool IsInBoundsX(int i)
        {
            return i > 0 && i < TileMatrix.GetLength(0);
        }
        private bool IsInBoundsY(int j)
        {
            return j > 0 && j < TileMatrix.GetLength(1);
        }

        private bool IsInBounds(int i, int j)
        {
            return IsInBoundsX(i) && IsInBoundsY(j);
        }

        private Vector2Int[] getNeighbourIndices(int i, int j, int r)
        {
            Vector2Int[] indices = new Vector2Int[0];
            for(int rI =-r; rI<= r; rI++)
            {
                for(int rJ =-r; rJ<= r;rJ++)
                {
                    if (IsInBounds(i + rI, j + rJ))
                    {
                        if (i + rI != 0 && j + rJ != 0) { indices.Append(new Vector2Int(i + rI, j + rJ)); }
                    }
                }
            }
            return indices;
            
        }
        public void UpdateTiles()
        {
            float minEntropy = float.MaxValue;
            float tileEntropy;
            Tile currentTile;
            int minI=0, minJ=0;
            int radius = 1;
            for (int i = 0; i < TileMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < TileMatrix.GetLength(1); j++)
                {
                    foreach (Vector2Int v in getNeighbourIndices(i, j, radius))
                    {

                        if(v.magnitude==radius)
                        {
                            currentTile = TileMatrix[v.x, v.y];
                            currentTile.updateStates(currentTile.currentState.allowedNeighbours);
                        }
                    }
                    tileEntropy = TileMatrix[i, j].getEntropy();
                    if (tileEntropy < minEntropy)
                    {
                        minEntropy = tileEntropy;
                        minI = i;
                        minJ = j;
                    }
                }
            }
            TileMatrix[minI, minJ].SelectCurrentState();
        }
    }
}
