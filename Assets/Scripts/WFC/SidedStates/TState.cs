using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.WFC
{
    [CreateAssetMenu(fileName = "Data", menuName = "WFC/T-State", order = 1)]
    /*A border State that has 3 identical sockets and 1 divergent socket
     *
     */
    public class TState:State
    {
        public WFCSocket ThreeSocket;
        public WFCSocket SingleSocket;
        
        public void OnValidate()
        {
            wfcSockets = new()
            {
                ThreeSocket,
                SingleSocket
            };
            wfcPattern = new WFCSocket[4][];
            for (int i = 0; i < wfcPattern.Length; i++)
            {
                wfcPattern[i] = new WFCSocket[1];
                wfcPattern[i][0] = ThreeSocket;
            }

            wfcPattern[0][0] = SingleSocket;
            if (ThreeSocket == SingleSocket)
            {
                Debug.LogWarning($"ThreeSocket and Single socket set to the same instance of {SingleSocket}");
            }
        }
        

        public override Boolean isPlaceable(WFCSocket[][] sockets)
        {
            
            if (sockets.Length < 4)
            {
                return false;
            }
            Boolean topContainsThree = false, topContainsSingle = false, rightContainsThree = false, rightContainsSingle = false, leftContainsThree = false, 
                leftContainsSingle = false, bottomContainsThree = false, bottomContainsSingle = false;

            // Extracting Booleans from the arrays
            for (int j = 0; j < sockets.Length; j++)
            {
                foreach (var socket in sockets[j])
                {
                    if (socket == ThreeSocket)
                    {
                        switch (j)
                        {
                            case 0: topContainsThree = true;
                                break;
                            case 1: rightContainsThree = true;
                                break;
                            case 2: bottomContainsThree = true;
                                break;
                            case 3: leftContainsThree = true;
                                break;
                            default: break;

                        }
                    }
                    else if (socket == SingleSocket)
                    {
                        switch (j)
                        {
                            case 0:
                                topContainsSingle = true;
                                break;
                            case 1:
                                rightContainsSingle = true;
                                break;
                            case 2:
                                bottomContainsSingle = true;
                                break;
                            case 3:
                                leftContainsSingle = true;
                                break;
                            default: break;

                        }
                    }
                }
            }
            // Logical  Functions
            Boolean horizontalMatch = topContainsThree && ((bottomContainsThree && ((rightContainsThree && leftContainsSingle) 
                || (leftContainsThree && rightContainsSingle))) || (bottomContainsSingle && (rightContainsThree && leftContainsSingle)));
            Boolean verticalMatch = rightContainsThree && ((leftContainsThree && ((bottomContainsThree && topContainsSingle)
                || (topContainsThree && bottomContainsSingle))) || (leftContainsSingle && (bottomContainsThree && topContainsSingle)));
            
            // Logic behind it
            //if (sockets[0].Contains(ThreeSocket))
            //{
            //    if (sockets[2].Contains(ThreeSocket))
            //    {
            //        if (sockets[1].Contains(ThreeSocket) && sockets[3].Contains(SingleSocket) ||
            //            sockets[3].Contains(ThreeSocket) && sockets[1].Contains(SingleSocket))
            //        {
            //            return true;
            //        }
            //    }
            //    else if (sockets[2].Contains(SingleSocket))
            //    {
            //        if (sockets[1].Contains(ThreeSocket) && sockets[3].Contains(SingleSocket))
            //        {
            //            return true;
            //        }
            //    }
            //}

            return horizontalMatch || verticalMatch;

        }
        
        /** Returns rotated placement for collapsing
         * Should only be called after isPlaceable
         * Junk goes in, junk goes out
         */
        public override WFCSocket[][] ReturnRotatedPlacement(WFCSocket[][] sockets)
        {
            if (sockets.Length < 4)
            {
                return sockets;
            }
            Boolean topContainsThree = false, topContainsSingle = false, rightContainsThree = false, rightContainsSingle = false, leftContainsThree = false,
                leftContainsSingle = false, bottomContainsThree = false, bottomContainsSingle = false;

            // Extracting Booleans from the arrays
            for (int j = 0; j < sockets.Length; j++)
            {
                foreach (var socket in sockets[j])
                {
                    if (socket == ThreeSocket)
                    {
                        switch (j)
                        {
                            case 0:
                                topContainsThree = true;
                                break;
                            case 1:
                                rightContainsThree = true;
                                break;
                            case 2:
                                bottomContainsThree = true;
                                break;
                            case 3:
                                leftContainsThree = true;
                                break;
                            default: break;

                        }
                    }
                    else if (socket == SingleSocket)
                    {
                        switch (j)
                        {
                            case 0:
                                topContainsSingle = true;
                                break;
                            case 1:
                                rightContainsSingle = true;
                                break;
                            case 2:
                                bottomContainsSingle = true;
                                break;
                            case 3:
                                leftContainsSingle = true;
                                break;
                            default: break;

                        }
                    }
                }
            }

            WFCSocket[][] newSockets = new WFCSocket[4][];
            for (int i =0; i < newSockets.Length; i++)
            {
                newSockets[i] = new WFCSocket[1];
            }
            if (topContainsThree)
            {
                newSockets[0][0] = ThreeSocket;
                if (bottomContainsThree)
                {
                    newSockets[2][0] = ThreeSocket;
                    if (rightContainsThree && leftContainsSingle)
                    {
                        newSockets[1][0] = ThreeSocket;
                        newSockets[3][0] = SingleSocket;
                        return newSockets;
                    }
                    else if(rightContainsSingle && leftContainsThree) // sockets[3].Contains(ThreeSocket) && sockets[1].Contains(SingleSocket))
                    {
                        newSockets[1][0] = SingleSocket;
                        newSockets[3][0] = ThreeSocket;
                        return newSockets;
                    }
                }
                else if (bottomContainsSingle)
                {
                    newSockets[2][0] = SingleSocket;
                    if (rightContainsThree && leftContainsThree)
                    {
                        newSockets[1][0] = ThreeSocket;
                        newSockets[3][0] = ThreeSocket;
                        return newSockets;
                    }
                }
            } else if (topContainsSingle && rightContainsThree && bottomContainsThree && leftContainsThree)
            {
                newSockets[0][0] = SingleSocket;
                newSockets[1][0] = ThreeSocket;
                newSockets[2][0] = ThreeSocket;
                newSockets[3][0] = ThreeSocket;
                return newSockets;
            }
            return sockets;
        }

    }
}
