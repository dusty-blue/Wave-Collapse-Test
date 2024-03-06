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
        void UpdateTiles(bool forceUpate);
        public WFCTile GetTile(Vector3Int index);
        public void SetAllTiles(WFCTile tile);

        public void ResetTile(Vector3Int index);
        public void CollapseTile(Vector2Int index, int passes ,int radius, bool forceUpdate);
        }
    
}
