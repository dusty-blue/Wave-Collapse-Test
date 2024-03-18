using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Utilities.Console.Commands;
using Assets.Scripts.WFC;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Console.Commands
{
    [CreateAssetMenu(fileName = "New Log command", menuName = "Utilities/Console/Command/Reset Command")]

    public  class ResetCommand : ConsoleCommand
    {
        [SerializeField] String tilemapName;
        private  Tilemap m_tilemap;
        private WFC_TerrainGeneration m_wfc;
        public override bool Process(string[] args)
        {
            if(m_tilemap == null || m_wfc==null)
            {
                GameObject parentObject = GameObject.Find(tilemapName);
                
                if (parentObject == null)
                {
                    Debug.Log($"Could not find GameObject with name {tilemapName}");
                    return false;
                }else
                {
                    m_tilemap = parentObject.GetComponent<Tilemap>();
                    m_wfc = parentObject.GetComponent<WFC_TerrainGeneration>();
                    if(m_tilemap == null || m_wfc == null )
                    {
                        Debug.Log($"Could not find Tilemap or WFC_TerrainGeneration Component");
                        return false;
                    } 
                }

            }

            

            if (args.Length == 1)
            {
                m_wfc.SetAllTiles(m_wfc.defaultTile);
                m_wfc.ResetMatrix(float.Parse(args[0]));
                return true;
            } else if (args.Length == 2) {
                float x, y;
                Vector3 worldPos;
                if(float.TryParse(args[0], out x) && float.TryParse(args[1], out y))
                {
                    worldPos.x = x;
                    worldPos.y = y;
                    worldPos.z =0;
                    m_wfc.ResetTile(worldPos);
                    m_wfc.LockTile(worldPos, 3);
                    return true;
                }
                return false;
                
            }
            else
            {
                m_wfc.SetAllTiles(m_wfc.defaultTile);
                m_wfc.ResetMatrix();
                return true;
            }

        }
    }
}
