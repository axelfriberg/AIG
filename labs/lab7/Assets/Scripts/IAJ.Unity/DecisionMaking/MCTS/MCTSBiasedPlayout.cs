using Assets.Scripts.GameManager;
using System;
using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTSBiasedPlayout : MCTS {
        public MCTSBiasedPlayout(CurrentStateWorldModel currentStateWorldModel) : base(currentStateWorldModel) {
        }

        protected override Reward Playout(WorldModel initialPlayoutState) {
            GOB.Action[] actions = initialPlayoutState.GetExecutableActions();
            int bestHvalue = int.MaxValue;
            int bestActionIndex = -1;
            WorldModel currentState = initialPlayoutState;
            while (!currentState.IsTerminal()) {
                for (int i = 0; i < actions.Length; i++) {
                    GOB.Action action = actions[i];
                    int h = action.getHvalue(currentState);
                    if (h < bestHvalue) {
                        bestActionIndex = i;
                        bestHvalue = h;
                    }
                }
                WorldModel childState = initialPlayoutState.GenerateChildWorldModel();
                actions[bestActionIndex].ApplyActionEffects(childState);
                childState.CalculateNextPlayer();
               currentState = childState;
                base.CurrentDepth++;
            }
            Reward r = new Reward();
            r.Value = currentState.GetScore();
            return r;
        }

        protected override MCTSNode Expand(MCTSNode parent, GOB.Action action) {
            WorldModel currentState = parent.State.GenerateChildWorldModel();
            action.ApplyActionEffects(currentState);
            currentState.CalculateNextPlayer();

            MCTSNode newChild = new MCTSNode(currentState) {
                Parent = parent,
                Action = action
            };
            parent.ChildNodes.Add(newChild);

            return newChild;
        }
    }
}
