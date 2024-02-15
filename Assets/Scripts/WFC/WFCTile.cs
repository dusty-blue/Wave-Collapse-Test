using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.WFC
{
    public class WFCTile

    {
        public State currentState;
        public State[] possibleStates;
        protected AliasSampling rnd;
        public bool isNotCollapsed = true;
        public WFCTile(State[] states, State starting )
        {
            possibleStates = states;
            currentState = starting;
            rnd = new AliasSampling(states.Select(x => x.m_spawnWeight).ToList<float>());
        }
        public WFCTile(State starting)
        {
            currentState = starting;
            possibleStates = starting.m_allowedNeighbours;
        }
        public float getEntropy()
        {
            float sum = 0;
            if(possibleStates.Length ==1)
            {
                return -currentState.m_spawnWeight;
            }
            foreach(State s in possibleStates)
            {
                sum -= s.m_spawnWeight* Mathf.Log(s.m_spawnWeight);
            }
            return sum;
        }
        public void updateStates(State[] neighbours)
        {
            possibleStates = neighbours.Intersect(possibleStates).ToArray();
        }
        public void SelectCurrentState()
        {
            rnd = new AliasSampling(possibleStates.Select(x => x.m_spawnWeight).ToList<float>());
            int i = rnd.DrawSample();
            currentState = possibleStates[i];

            isNotCollapsed = false;
        }

        public WFCTile Clone()
        {
            return (WFCTile)this.MemberwiseClone();
        }

    }
}
