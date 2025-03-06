using Assets.Scripts.WFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EditorTests
{
    public static class UnitTestUtility
    {
        public static Dictionary<String, State> SetupStates(List<String> stateNames)
        {

            StateLoader loader = new("Assets/Scripts/WFC/States/Test");
            Dictionary<String, State> stateDic = loader.LoadStates(stateNames.ToArray());

            return stateDic;
        }

        public static LoadedObjects SetupSidedStates(List<String> stateList)
        {
            StateLoader loader = new("Assets/Scripts/WFC/States/Test");
            return loader.LoadStateandSockets(stateList);
        }
    }
}
