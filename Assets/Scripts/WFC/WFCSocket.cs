using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.WFC
{
    [CreateAssetMenu(fileName = "Data", menuName = "WFC/WFCSocket", order = 1)]
    public class WFCSocket : ScriptableObject
    {
        public State State1;
        public State State2;
        public float weight = 1f;

        public float spawnWeight
        {
            get { return weight * MathF.Min(State1.m_spawnWeight, State2.m_spawnWeight); }
        }
    }
}
