using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


namespace Assets.Scripts.WFC
{
    [CreateAssetMenu(fileName = "Data", menuName = "WFC/NeigbourState", order = 1)]
    public class NeighbourState : ScriptableObject
    {
        public State m_state;
        public float m_weight =1f;
        public float spawnWeight
        {
            get { return m_weight * m_state.m_spawnWeight; }
        }
    }
}
