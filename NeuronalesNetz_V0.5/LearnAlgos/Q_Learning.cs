using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace NeuronalesNetz_V0._5
{
    [Serializable]
    public class Q_Learning
    {   
        public Dictionary<object, Q_State> Q_Table;

        private static Random rnd = new Random();
        public double gamma;
        public double exploreProb;
        public int actionsCount;
        public double exploreReward;
        public object lastState;
        public int? lastAction;
        private List<StAck> allreadyUpdated = new List<StAck>();

        public Q_Learning(double gamma, double exploreProb, int actionsCount, double exploreReward)
        {
            this.gamma = gamma;
            this.exploreProb = exploreProb;
            this.actionsCount = actionsCount;
            this.exploreReward = exploreReward;


            Q_Table = new Dictionary<object, Q_State>();
        }

        public int Get_SetAction(object thisState, double reward)
        {

            int nextAction;

            if (!Q_Table.ContainsKey(thisState))
            {
                Q_Table.Add(thisState, new Q_State(actionsCount, exploreReward, lastState, lastAction));

                //Q_Table.OrderBy(i => i.Key);
            }

            if (lastState != null && !lastState.Equals(thisState) && reward == 0)
            {
                Q_Table[thisState].AddVongState(new StAck(lastState, (int)lastAction));
            }
            //if (reward == 0 && lastAction != null && lastState.Equals(thisState)) 
            //{
            //    Q_Table[thisState].actionReward[(int)lastAction] = -10;
            //}

            if (reward != 0)
            {
                Q_Table[lastState].actionReward[(int)lastAction] = reward;

                //if (reward > 0)
                    UpdateRecursive(thisState);
                //else
                    //UpateLOL();

                allreadyUpdated = new List<StAck>();
                lastState = null;
                lastAction = null;
            }

            nextAction = Q_Table[thisState].GetMaxAction(exploreProb);

            if (reward == 0)
            {
                lastAction = nextAction;
                lastState = thisState;
            }

            return nextAction;
        }

        public void UpateLOL()
        {
            foreach(KeyValuePair<object,Q_State> item in Q_Table)
            {
                if (item.Value.actionReward.Max() > 0)
                {
                    UpdateRecursive(item.Key);
                }
            }
        }

        public void UpdateRecursive(object startState)
        {

            for(int i = 0; i < Q_Table[startState].vongStack.Count; ++i)
            {

                if (!allreadyUpdated.Contains(Q_Table[startState].vongStack[i]))
                {
                    allreadyUpdated.Add(Q_Table[startState].vongStack[i]);

                    Q_Table[Q_Table[startState].vongStack[i].state].actionReward[Q_Table[startState].vongStack[i].action] = (gamma+(rnd.NextDouble()/100)) * Q_Table[startState].actionReward.Max();

                    UpdateRecursive(Q_Table[startState].vongStack[i].state);
                }
            }
        }


    }

    [Serializable]
    public class Q_State
    {
        private static Random rnd = new Random();
        public double[] actionReward;        //alle states, die zu diesem state führen         //die action in vong state, welche zu diesem state führen
        public List<StAck> vongStack;

        public Q_State(int actionsCout, double exploreReward)
        {
            actionReward = new double[actionsCout];
          

            vongStack = new List<StAck>();

            for (int i = 0; i < actionsCout; ++i)
            {
                actionReward[i] = exploreReward;
            }

        }

        public Q_State(int actionsCout, double exploreReward, object lastState, int? lastAction)
        {
            actionReward = new double[actionsCout];
            vongStack = new List<StAck>();

            if (lastAction != null)
            {
                vongStack.Add(new StAck(lastState,(int)lastAction));
            }


            for (int i = 0; i < actionsCout; ++i)
            {
                actionReward[i] = exploreReward;
            }

        }

        public Q_State(int actionsCout, double exploreReward, List<StAck> lastStack)
        {
            vongStack = new List<StAck>();
            actionReward = new double[actionsCout];

            for (int i = 0; i < actionsCout; ++i)
            {
                actionReward[i] = exploreReward;
            }

        }

        public void AddVongState(StAck state)
        {
            if (!vongStack.Contains(state))
            {
                vongStack.Add(state);
            }

        }

        public int GetMaxAction(double exploreProb)
        {
            List<int> maxAction = new List<int>();

            for (int i = 0; i < actionReward.Length; ++i)
            {
                if (actionReward[i] == actionReward.Max())
                    maxAction.Add(i);
            }

            if (rnd.NextDouble() > exploreProb)
            {
                return maxAction[rnd.Next(maxAction.Count)];
            }
            else
            {
                if (maxAction.Count < actionReward.Length)
                {
                    int randAction;

                    do
                    {
                        randAction = rnd.Next(actionReward.Length);

                    } while (actionReward[randAction] < 0 && rnd.NextDouble() > 0.001);

                    return randAction;
                }
                else
                {
                    return rnd.Next(actionReward.Length);
                }

            }
        }


        public double GetStateReward(double exploreProb)
        {
            double maxIndexCount = 0;
            for (int i = 0; i < actionReward.Length; ++i)
            {
                if (actionReward[i] == actionReward.Max())
                {
                    maxIndexCount++;
                }
            }

            if (maxIndexCount == actionReward.Length)
            {
                return actionReward.Max();
            }
            else
            {
                double stateReward = 0;

                for (int i = 0; i < actionReward.Length; ++i)
                {
                    if (actionReward[i] == actionReward.Max())
                    {
                        //if (actionReward.Max() < 0)
                        //{
                        //    stateReward += actionReward[i] / maxIndexCount;
                        //}
                        //else
                        
                            stateReward += actionReward[i] * ((1 - exploreProb) / maxIndexCount);
                        
                    }
                    else
                    {
                        stateReward += actionReward[i] * (exploreProb / (actionReward.Length - maxIndexCount));
                    }
                }

                return stateReward;
            }

        }

        public override string ToString()
        {
            string a = "";

            for (int i = 0; i < actionReward.Length; ++i)
            {
                a += Math.Round(actionReward[i]) + ", ";
            }


            return a;
        }
    }

    public struct StAck
    {
        public object state;
        public int action;

        public StAck(object state, int action)
        {
            this.state = state;
            this.action = action;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is StAck))
            {
                return false;
            }

            StAck temp = (StAck)obj;

            return temp.state.Equals(state) && temp.action == action;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}