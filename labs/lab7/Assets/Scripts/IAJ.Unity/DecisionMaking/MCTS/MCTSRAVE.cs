using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using Assets.Scripts.IAJ.Unity.Utils;
using System;
using System.Collections.Generic;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.GOB.Action;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS {
    public class MCTSRAVE : MCTS {
        protected const float b = 1;
        protected List<Pair<int, Action>> ActionHistory { get; set; }
        public MCTSRAVE(CurrentStateWorldModel worldModel) : base(worldModel) {
            this.ActionHistory = new List<Pair<int, GOB.Action>>();
        }

        protected override MCTSNode BestUCTChild(MCTSNode node) {
            float MCTSValue;
            float RAVEValue;
            float UCTValue;
            float bestUCT = float.MinValue;
            int k = node.N;

            //step 1, calculate beta and 1-beta. beta does not change from child to child. So calculate this only once
            //step 2, calculate the MCTS value, the RAVE value, and the UCT for each child and determine the best one

            List<MCTSNode> children = node.ChildNodes;
            int bestChildIndex = -1;
            double beta = node.NRAVE / (node.N + node.NRAVE + 4 * node.N * Math.Pow(Math.Pow(node.NRAVE, b), 2));
            double betaminus = 1 - beta;
            for (int i = 0; i < children.Count; i++) {
                MCTSNode child = children[i];
                MCTSValue = child.Q / child.N; //Do as multiplication with power of -1?          
                RAVEValue = child.QRAVE / child.NRAVE;
                int ni = child.N;
                int N = node.N;
                double C = Math.Sqrt(2);
                //double UCT = MCTSValue + C * (Math.Sqrt(Math.Log(N) / ni));
                UCTValue = (float)((betaminus * MCTSValue + beta * RAVEValue) + C * (Math.Sqrt(Math.Log(N) / ni)));
                if (UCTValue > bestUCT) {
                    bestUCT = UCTValue;
                    bestChildIndex = i;
                }
            }
            return children[bestChildIndex];
        }


        protected override Reward Playout(WorldModel initialPlayoutState) {
            WorldModel currentState = initialPlayoutState;
            while (!currentState.IsTerminal()) {
                GOB.Action[] actions = currentState.GetExecutableActions();
                if (actions.Length == 0)
                    continue;
                int index = this.RandomGenerator.Next(0, actions.Length);
                GOB.Action action = actions[index];
                Pair<int, Action> pair = new Pair<int, Action>(currentState.GetNextPlayer(), action);
                ActionHistory.Add(pair);
                currentState = currentState.GenerateChildWorldModel();
                action.ApplyActionEffects(currentState);
                this.CurrentDepth++;
            }
            Reward reward = new Reward();
            reward.Value = currentState.GetScore();
            return reward;
        }

        protected override void Backpropagate(MCTSNode node, Reward reward) {
            MCTSNode currentNode = node;
            while (currentNode != null) {
                currentNode.N += 1;
                currentNode.Q += reward.Value;
                Pair<int, Action> pair = new Pair<int, Action>(node.Parent.State.GetNextPlayer(), node.Action);
                ActionHistory.Add(pair);
                currentNode = currentNode.Parent;
                if (currentNode != null) {
                    foreach (MCTSNode c in currentNode.ChildNodes) { 
                        Pair<int, Action> childPair = new Pair<int, Action>(currentNode.PlayerID, c.Action);
                        if (ActionHistory.Contains(childPair)) {
                            c.NRAVE += 1;
                            c.QRAVE += reward.Value;
                        }
                    }
                }
            }
        }
    }
}
