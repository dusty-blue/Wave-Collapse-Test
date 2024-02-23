using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.WFC
{
    public class StateLoader
    {
        private String m_root;
        public String rootPath
        {
            get { return m_root; }
            set {
                if (Directory.Exists(value))
                {
                    m_root = value;
                }
                else
                {
                    Debug.Log($"Failed to find path: {value}");
                }
            }
        }
        public StateLoader(String root)
        {
            if(Directory.Exists(root))
            {
                m_root = root;
            }
            else
            {
                Debug.Log($"Failed to find path: {root}");
            }
        }

        public Dictionary<String,State>LoadStates(String[] states)
        {
            Dictionary<String,State >  dic= new(states.Length);
            foreach (String s in states)
            {
                string[] files = Directory.GetFiles($"{m_root}/{s}", "*.asset", SearchOption.TopDirectoryOnly);
                List<NeighbourState> nStates = new();
                foreach (var f in files)
                {
                    ScriptableObject o = AssetDatabase.LoadAssetAtPath<ScriptableObject>(f);
                    if(!o)
                    {
                        Debug.Log($"Object is null at path: {f}");
                    } else 
                    if (o.GetType() == typeof(State))
                    {
                        dic.Add(s, (State)o);
                    }
                    else if (o.GetType() == typeof(NeighbourState))
                    {
                        nStates.Add((NeighbourState)o);
                    } 
                }
                if (dic.ContainsKey(s))
                {
                    dic[s].m_allowedNeighbours = nStates.ToArray();
                }
            }
            return dic;
        }

        public Dictionary<String, State> LoadStates(String[] states, String path)
        {
            rootPath = path;
            return LoadStates(states);
        }
    }
}
