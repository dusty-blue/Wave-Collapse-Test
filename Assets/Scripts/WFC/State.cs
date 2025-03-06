using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Assets.Scripts.WFC
{
    
    public abstract class State :ScriptableObject

    {
        //public string m_name;
        //public State[] allowedNeighbours;
        //public float[] m_Neighbourweight;
        public TileBase m_UnityTile;
        public float m_spawnWeight;
        
        public List<WFCSocket> wfcSockets;
        public WFCSocket[][] wfcPattern;
        public abstract Boolean isPlaceable(WFCSocket[][] sockets);
        public abstract WFCSocket[][] ReturnRotatedPlacement(WFCSocket[][] sockets);
        

    }
}
