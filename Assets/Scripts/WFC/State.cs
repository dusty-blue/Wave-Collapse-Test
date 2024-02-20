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

        public State (String name, float weight)
        {
            m_name = name;
            m_spawnWeight = weight;
        }

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
    [CreateAssetMenu(fileName = "Data", menuName = "WFC/Neigbour/State", order = 1)]
    public class NeighbourState : ScriptableObject
    {
        public State m_state;
        public float m_weight =1f;
        public NeighbourState(State neighbour, float weight)
        {
            m_state = neighbour;
            m_weight = weight;
        }
        public float spawnWeight
        {
            get { return m_weight * m_state.m_spawnWeight; }
        }

    }
}
