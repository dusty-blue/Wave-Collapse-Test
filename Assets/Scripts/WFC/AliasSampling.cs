using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.WFC
{
    public class AliasSampling
    {
        private List<float> U,P;
        private List<int> K;
        private Random rnd;
        public AliasSampling(List<float> distr)
        {
            rnd = new Random();

            float sum = distr.Sum();
            int n = distr.Count();
            P = new List<float>(n);
            U = new List<float>(n);
            K = new List<int>(n);
            float p_i;
            for(int i =0; i<n; i++)
            {
                p_i = (distr[i] * n) / sum;
                P.Add(p_i);
                U.Add( p_i );
                K.Add(-1);
            }

            int l = 0, s = 0;
            int largeL=0, smallS=0;
            for(int j =0; j < n; j++)
            {
                if (P[j]> 1f)
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
                U[jay] = P[jay];
                P[k] =P[k] + P[jay] - 1f;
                if (P[k]>1f)
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
            if(U.Count==0)
            {
                return -1;
            }
            if (y < U[i])
            {
                return i;
            } else
            {
                if (K[i]<0)
                {
                    return i;
                }
                return K[i];
            }

        }
    }
}
