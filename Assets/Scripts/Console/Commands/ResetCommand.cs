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

            m_wfc.SetAllTiles(m_wfc.defaultTile);

            if(args.Length ==1)
            {
                m_wfc.ResetMatrix(float.Parse(args[0]));
                return true;
            } else
            {
                m_wfc.ResetMatrix();
                return true;
            }

        }
    }
}
