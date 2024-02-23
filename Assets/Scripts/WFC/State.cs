using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Assets.Scripts.WFC
{
    [CreateAssetMenu(fileName = "Data", menuName = "WFC/State", order = 1)]
    public class State :ScriptableObject

    {
        //make scriptable object
        public string m_name;
        public float m_spawnWeight;
        public NeighbourState[] m_allowedNeighbours;
        public float[] m_Neighbourweight;
        public TileBase m_UnityTile;
        public bool Contains(State s)
        {
            foreach (NeighbourState n in m_allowedNeighbours)
            {
                if(n.m_state == s)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
