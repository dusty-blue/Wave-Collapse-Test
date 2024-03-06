using Assets.Scripts.Utilities.Console.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Console.Commands
{
    [CreateAssetMenu(fileName = "New Collapse command", menuName = "Utilities/Console/Command/Collapse Command")]

    public class CollapseTileCommand: ConsoleCommand
    {
        [SerializeField] String tilemapName;
        private Tilemap m_tilemap;
        private WFC_TerrainGeneration m_wfc;
        public override bool Process(string[] args)
        {
            if (m_tilemap == null || m_wfc == null)
            {
                GameObject parentObject = GameObject.Find(tilemapName);

                if (parentObject == null)
                {
                    Debug.Log($"Could not find GameObject with name {tilemapName}");
                    return false;
                }
                else
                {
                    m_tilemap = parentObject.GetComponent<Tilemap>();
                    m_wfc = parentObject.GetComponent<WFC_TerrainGeneration>();
                    if (m_tilemap == null || m_wfc == null)
                    {
                        Debug.Log($"Could not find Tilemap or WFC_TerrainGeneration Component");
                        return false;
                    }
                }

            }
            if (args.Length == 3)
            {
                List<float> argF = new();
                foreach(string s in args)
                {
                    argF.Add(float.Parse(s));
                }
                Vector3 worldPos = new Vector3(argF[0], argF[1], argF[2]);
                m_wfc.CollapseTile(worldPos);
                return true;
            }

            return false;
        }
    }
}
