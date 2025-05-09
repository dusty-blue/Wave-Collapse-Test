﻿using System;
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

        public void SetTile(Vector3Int index, WFCTile tile);
        public void CollapseTile(Vector2Int index, int maxPasses ,int radius, bool forceUpdate);
        }
    
}
