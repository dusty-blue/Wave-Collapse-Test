using System;
using System.Collections;
using System.Collections.Generic;
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
        public bool isImpossible = false;
        public bool isLocked = false;
        private float m_LockTimeRemaining = 0f;
        
        public WFCTile( State[] states, State starting )
        {
            possibleStates = states;
            isImpossible = possibleStates.Length == 0;
            currentState = starting;
        }
        public WFCTile(State starting)
        {
            currentState = starting;
            isImpossible = starting.wfcSockets.Count ==0;
            
        }

        public IEnumerator LockForSecs(float lockTime)
        {
            isLocked = true;
            yield return new WaitForSeconds(lockTime);
            isLocked = false;
        }
        public float getEntropy()
        {
            float sum = 0;
            float p =0;
            if (possibleStates != null)
            {
                if (possibleStates.Length == 0)
                {
                    return float.MinValue;
                }
                foreach (State s in possibleStates)
                {
                    p = s.m_spawnWeight;
                    sum -= p * Mathf.Log(p, 2);
                }
            }
            else
            {
                return float.MinValue;
            }
            
            return sum;
        }
        public void updateStates(WFCSocket[][] newSockets)
        {
            List<State> newList = new();
            foreach (var s in possibleStates)
            {
                if (s.isPlaceable(newSockets))
                {
                    newList.Add(s);
                }
            }
            possibleStates = newList.ToArray();
        }
        /**Returns true  if new list is updated and not impossible
         *
         */
        public Boolean TryUpdateStates(WFCSocket[][] newSockets)
        {
            List<State> newList = new();
            foreach (var s in possibleStates)
            {
                if (s.isPlaceable(newSockets))
                {
                    newList.Add(s);
                }
            }

            if (newList.Count == 0)
            {
                isImpossible = true;
                possibleStates = newList.ToArray();
                return false;
            }
            foreach ( State s in possibleStates)
            {
                //if something changed return true
                if(!newList.Contains(s) )
                {
                    possibleStates = newList.ToArray();
                    return true;
                }
            }

            possibleStates = newList.ToArray();
            return false;
        }

        public Boolean TryUpdateStates(WFCSocket[] newSockets, int direction)
        {
            WFCSocket[][] newDirectionSockets = new WFCSocket[4][];
            for (int i = 0; i < newDirectionSockets.Length; i++)
            {
                if (i == direction)
                {
                    newDirectionSockets[i] = newSockets;
                }
                else
                {
                    List<WFCSocket> oldSockets = new(); 
                    foreach (var possibleState in possibleStates)
                    {
                        oldSockets.AddRange(possibleState.wfcSockets);
                    }

                    newDirectionSockets[i] = oldSockets.ToArray();
                }
            }

            return TryUpdateStates(newDirectionSockets);
        }
        public void SelectCurrentState()
        {
            if(isImpossible)
            {
                isNotCollapsed = false;
                return;
            }
            rnd = new AliasSampling(possibleStates.Select(x => x.m_spawnWeight).ToList<float>());
            int i = rnd.DrawSample();
            if(i<0)
            {
                isImpossible = true;
                return;
            }
            currentState = possibleStates[i];

            isNotCollapsed = false;
        }

        public WFCTile Clone()
        {
            WFCTile clonedTile = (WFCTile)this.MemberwiseClone();
            if (this.possibleStates != null)
            {
                State[] clonedArray = new State[this.possibleStates.Length];
                for (int j = 0; j < this.possibleStates.Length; j++)
                {
                    clonedArray[j] = this.possibleStates[j];
                }
                clonedTile.possibleStates = clonedArray;
            }
            
            return clonedTile;
        }

    }
}
