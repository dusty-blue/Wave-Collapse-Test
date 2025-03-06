using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace Assets.Scripts.WFC
{
    public class LoadedObjects
    {
        public State[] allStates;
        public WFCSocket[] allSockets;
        public ScriptableObject[] otherObjects;
    }
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
                string[] files = Directory.GetFiles($"{m_root}/{s}", "*.asset", SearchOption.AllDirectories);
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
                    //dic[s].m_allowedNeighbours = nStates.ToArray();
                }
            }
            return dic;
        }

        public LoadedObjects LoadStateandSockets(List<String> folderList)
        {
            
            List<ScriptableObject> extra = new ();
            List<State> states = new();
            List<WFCSocket> sockets = new();
            foreach (var folder in folderList)
            {
                string[] filePath = Directory.GetFiles($"{m_root}/{folder}", "*.asset", SearchOption.AllDirectories);
                foreach (var f in filePath)
                {
                    ScriptableObject o = AssetDatabase.LoadAssetAtPath<ScriptableObject>(f);
                    if (!o)
                    {
                        Debug.Log($"Object is null at path: {f}");
                    }
                    else if (o is State state) 
                    {
                        states.Add(state);
                    }
                    else if (o is WFCSocket socket)
                    {
                        sockets.Add(socket);
                    }
                    else
                    {
                        extra.Add(o);
                    }
                }
            }
            LoadedObjects objects = new LoadedObjects();
            objects.allStates = states.ToArray();
            objects.allSockets = sockets.ToArray();
            objects.otherObjects = extra.ToArray();
            return objects;
        }

        public Dictionary<String, State> LoadStates(String[] states, String path)
        {
            rootPath = path;
            return LoadStates(states);
        }
    }
}
