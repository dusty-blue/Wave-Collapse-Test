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
        public Tile m_UnityTile;

        public State (String name, float weight)
        {
            m_name = name;
            m_spawnWeight = weight;
        }

    }

    public class NeighbourState
    {
        public State m_state;
        public float m_weight =1f;
        public NeighbourState(State neighbour, float weight)
        {
            m_state = neighbour;
            m_weight = weight;
        }

    }
}
