using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.WFC
{
    public class AliasSampling
    {
        private List<float> U;
        private List<int> K;
        private Random rnd;
        public AliasSampling(List<float> distr)
        {
            rnd = new Random();

            float sum = distr.Sum();
            int n = distr.Count();
            U = new List<float>(n);
            K = new List<int>(n);
            
            for(int i =0; i<n; i++)
            {
                U.Add( (distr[i] * n)/sum );
                K.Add(-1);
            }

            int l = 0, s = 0;
            int largeL=0, smallS=0;
            for(int j =0; j < n; j++)
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
}
