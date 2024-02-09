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

    public class AliasSampling
    {
        private List<float> U;
        private List<int> K;
        private Random rnd;
        public AliasSampling(List<float> distr)
        {
            rnd = new Random();

            int n = distr.Count();
            U = new List<float>(n);
            K = new List<int>(n);
            
            for(int i =0; i<n; i++)
            {
                U.Add( distr[i] * n);
                K.Add(-1);
            }

            int l = 0, s = 0;
            int largeL=0, smallS=0;
            for(int j =0; j > n; j++)
            {
                if (U[j]> 1f)
                {
                    largeL = j;l++;
                } else
                {
                    smallS = j;s++;
                }
            }
            int jay, k;
            while(s!=0 && l !=0)
            {
                s--;
                jay = smallS;
                l--;
                k = largeL;
                K[jay] =k;
                U[k] = U[k] + U[jay] - 1f;
                if (U[k]>1f)
                {
                    largeL = k; l++;
                } else
                {
                    smallS = k; s++;
                }
            }
            while(s>0)
            {
                s--;
                U[smallS] =1f;
            } 
            while(l>0)
            {
                l--;
                U[largeL] = 1f;
            }


        }

        public int DrawSample()
        {
            float x = (float)rnd.NextDouble();
            float n = (float)U.Count();
            int i = (int)Math.Floor(n* x);
            float y = n * x - (float)i;
            if (y < U[i])
            {
                return i;
            } else
            {
                return K[i];
            }

        }
    }
    public class RandomSelectionUtility
    {
          
    }
}
