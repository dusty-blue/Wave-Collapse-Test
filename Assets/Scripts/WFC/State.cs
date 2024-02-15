using System;

namespace Assets.Scripts.WFC
{
    public class State

    {
        //make scriptable object
        public string m_name;
        public float m_spawnWeight;
        public State[] m_allowedNeighbours;

        public State (String name, float weight)
        {
            m_name = name;
            m_spawnWeight = weight;
        }

    }
}
