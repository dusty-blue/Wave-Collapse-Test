using System;
using System.Collections.Generic;
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
        public bool isImpossible = false;
        
        public WFCTile(NeighbourState[] states, State starting )
        {
            possibleStates = states;
            isImpossible = possibleStates.Length == 0;
            currentState = starting;
        }
        public WFCTile(State starting)
        {
            currentState = starting;
            possibleStates = starting.m_allowedNeighbours;
            isImpossible = starting.m_allowedNeighbours.Length == 0;
            
        }
        public float getEntropy()
        {
            float sum = 0;
            float p =0;
            if(possibleStates.Length ==1)
            {
                return -possibleStates[0].spawnWeight;
            }else if (possibleStates.Length ==0)
            {
                return float.MinValue;
            }
            foreach(NeighbourState s in possibleStates)
            {
                p = s.spawnWeight;
                sum -= p* Mathf.Log(p,2);
            }
            return sum;
        }
        public void updateStates(NeighbourState[] neighbours)
        {
            List<NeighbourState> newList = new();
            foreach (NeighbourState n in neighbours)
            {
                foreach(NeighbourState s in possibleStates)
                {
                    if(n.m_state == s.m_state)
                    { 
                        if ( n.spawnWeight <= s.spawnWeight)
                        {
                            newList.Add(n);
                        }
                        else
                        {
                            newList.Add(s);
                        }
                    }
                }
            }
            possibleStates = newList.ToArray();
        }

        public Boolean TryUpdateStates(NeighbourState[] neighbours)
        {
            List<NeighbourState> newList = new();
            foreach (NeighbourState n in neighbours)
            {
                foreach (NeighbourState s in possibleStates)
                {
                    if (n.m_state == s.m_state)
                    {
                        if (n.spawnWeight <= s.spawnWeight)
                        {
                            newList.Add(n);
                        }
                        else
                        {
                            newList.Add(s);
                        }
                    }
                }
            }
            foreach ( NeighbourState s in possibleStates)
            {
                if(!newList.Contains(s) )
                {
                    possibleStates = newList.ToArray();
                    return false;
                }
            }
            if (newList.Count == 0)
            {
                isImpossible = true;
                possibleStates = newList.ToArray();
                return true;
            }

            possibleStates = newList.ToArray();
            return true;
        }
        public void SelectCurrentState()
        {
            if(isImpossible)
            {
                isNotCollapsed = false;
                return;
            }
            rnd = new AliasSampling(possibleStates.Select(x => x.spawnWeight).ToList<float>());
            int i = rnd.DrawSample();
            if(i<0)
            {
                isImpossible = true;
                return;
            }
            currentState = possibleStates[i].m_state;

            isNotCollapsed = false;
        }

        public WFCTile Clone()
        {
            return (WFCTile)this.MemberwiseClone();
        }

    }
}
