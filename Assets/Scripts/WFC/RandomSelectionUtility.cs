using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.WFC
{
    public class RejectionSampling
    {
        protected List<int> table;
        protected float scale;
        private Random rnd;
        protected RejectionSampling (Dictionary<int, float> distr)
        {

            float minP= distr.Min(p => p.Value);
            scale = 1f / minP;
            rnd = new Random();
        }
         public int DrawSample()
        {
            return table[rnd.Next(table.Count) ];
        }

    }
    public class RejectionSamplingSubTable:RejectionSampling
    {
        
        public RejectionSamplingSubTable(Dictionary<int,float> distr) :base(distr)
        {
            int amount;
            
            table = new List<int>();
            foreach(KeyValuePair< int,float> p in distr)
            {
                amount = (int)Math.Round(scale * p.Value);
                List<int> subTable = new List<int>(amount);
                for(int j =0; j< amount; j++)
                {
                    subTable[j] = p.Key;
                }
                table.AddRange(subTable);
                
            }
        }
    }

    public class RejectionSamplingAddSingle :RejectionSampling
    {
        
        public RejectionSamplingAddSingle(Dictionary<int, float> distr): base (distr)
        {
            int amount;
            table = new List<int>();
            foreach (KeyValuePair<int, float> p in distr)
            {
                amount = (int)Math.Round(scale * p.Value);
                for (int j = 0; j < amount; j++)
                {
                    table.Add( p.Key);
                }
            }
        }
    }
    public class RandomSelectionUtility
    {
          
    }
}
