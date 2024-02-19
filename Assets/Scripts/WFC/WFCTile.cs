using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.WFC
{
    public class WFCTile

    {
        public State currentState;
        public NeighbourState[] possibleStates;
        protected AliasSampling rnd;
        public bool isNotCollapsed = true;
        public WFCTile(NeighbourState[] states, State starting )
        {
            possibleStates = states;
            currentState = starting;
            rnd = new AliasSampling(states.Select(x => x.m_weight * x.m_state.m_spawnWeight).ToList<float>());
        }
        public WFCTile(State starting)
        {
            currentState = starting;
            possibleStates = starting.m_allowedNeighbours;
        }
        public float getEntropy()
        {
            float sum = 0;
            float p =0;
            if(possibleStates.Length ==1)
            {
                return -currentState.m_spawnWeight;
            }
            foreach(NeighbourState s in possibleStates)
            {
                p = s.m_weight*s.m_state.m_spawnWeight;
                sum -= p* Mathf.Log(p,2);
            }
            return sum;
        }
        public void updateStates(NeighbourState[] neighbours)
        {
            possibleStates = neighbours.Intersect(possibleStates).ToArray();
        }
        public void SelectCurrentState()
        {
            rnd = new AliasSampling(possibleStates.Select(x => x.m_weight * x.m_state.m_spawnWeight).ToList<float>());
            int i = rnd.DrawSample();
            currentState = possibleStates[i].m_state;

            isNotCollapsed = false;
        }

        public WFCTile Clone()
        {
            return (WFCTile)this.MemberwiseClone();
        }

    }
}
