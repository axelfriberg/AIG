using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTS
    {
        public const float C = 1.4f;
        public bool InProgress { get; private set; }
        public int MaxIterations { get; set; }
        public int MaxIterationsProcessedPerFrame { get; set; }
        public int MaxPlayoutDepthReached { get; private set; }
        public int MaxSelectionDepthReached { get; private set; }
        public float TotalProcessingTime { get; private set; }
        public MCTSNode BestFirstChild { get; set; }
        public List<GOB.Action> BestActionSequence { get; private set; }


        private int CurrentIterations { get; set; }
        private int CurrentIterationsInFrame { get; set; }
        private int CurrentDepth { get; set; }

        private CurrentStateWorldModel CurrentStateWorldModel { get; set; }
        private MCTSNode InitialNode { get; set; }
        private System.Random RandomGenerator { get; set; }
        
        

        public MCTS(CurrentStateWorldModel currentStateWorldModel)
        {
            this.InProgress = false;
            this.CurrentStateWorldModel = currentStateWorldModel;
            this.MaxIterations = 100;
            this.MaxIterationsProcessedPerFrame = 10;
            this.RandomGenerator = new System.Random();
        }


        public void InitializeMCTSearch()
        {
            this.MaxPlayoutDepthReached = 0;
            this.MaxSelectionDepthReached = 0;
            this.CurrentIterations = 0;
            this.CurrentIterationsInFrame = 0;
            this.TotalProcessingTime = 0.0f;
            this.CurrentStateWorldModel.Initialize();
            this.InitialNode = new MCTSNode(this.CurrentStateWorldModel)
            {
                Action = null,
                Parent = null,
                PlayerID = 0
            };
            this.InProgress = true;
            this.BestFirstChild = null;
            this.BestActionSequence = new List<GOB.Action>();
        }

        public GOB.Action Run()
        {
            MCTSNode selectedNode;
            Reward reward;

            var startTime = Time.realtimeSinceStartup;
            this.CurrentIterationsInFrame = 0;

            while (this.CurrentIterationsInFrame < this.MaxIterationsProcessedPerFrame && 
                this.CurrentIterations < this.MaxIterations)
            {
                this.CurrentDepth = 0;
                selectedNode = Selection(this.InitialNode);
                if (this.CurrentDepth > this.MaxSelectionDepthReached)
                    MaxSelectionDepthReached = CurrentDepth;
                reward = Playout(selectedNode.State);
                if (this.CurrentDepth > this.MaxPlayoutDepthReached)
                    MaxPlayoutDepthReached = CurrentDepth;
                Backpropagate(selectedNode, reward);
                this.CurrentIterationsInFrame++;
                this.CurrentIterations++;
            }
            var endTime = Time.realtimeSinceStartup;
            TotalProcessingTime += endTime- startTime;
            MCTSNode child = BestChild(InitialNode);
            this.BestFirstChild = child;
            return child.Action;
        }

        private MCTSNode Selection(MCTSNode initialNode)
        {
            GOB.Action nextAction;
            MCTSNode currentNode = initialNode;
            MCTSNode bestChild = null;

            while (!currentNode.State.IsTerminal())
            {
                nextAction = currentNode.State.GetNextAction();
                if (nextAction != null)
                {
                    return Expand(currentNode, nextAction);
                }else
                {
                    this.CurrentDepth++;
                    bestChild = BestUCTChild(currentNode);
                    currentNode = bestChild;
                    return bestChild;
                }

            }

            return bestChild;
        }

        private Reward Playout(WorldModel initialPlayoutState)
        {
            WorldModel currentState = initialPlayoutState;
            while (!currentState.IsTerminal())
            {
                GOB.Action[] actions = currentState.GetExecutableActions();
                if (actions.Length == 0)
                    continue;
                int index = this.RandomGenerator.Next(0, actions.Length);
                GOB.Action action = actions[index];
                currentState = currentState.GenerateChildWorldModel();
                action.ApplyActionEffects(currentState);
                this.CurrentDepth++;
            }
            Reward reward = new Reward();
            reward.Value = currentState.GetScore();
            return reward;
        }

        private void Backpropagate(MCTSNode node, Reward reward)
        {
            MCTSNode currentNode = node;
            while(currentNode != null)
            {
                currentNode.N += 1;
                currentNode.Q += reward.Value;
                currentNode = currentNode.Parent;                  
            }
        }

        private MCTSNode Expand(MCTSNode parent, GOB.Action action)
        {
            WorldModel currentState = parent.State.GenerateChildWorldModel();
            action.ApplyActionEffects(currentState);
            MCTSNode newChild = new MCTSNode(currentState)
            {
                Parent = parent,
                Action = action
            };
            parent.ChildNodes.Add(newChild);

            return newChild;
        }

        //gets the best child of a node, using the UCT formula
        private MCTSNode BestUCTChild(MCTSNode node)
        {
            List<MCTSNode> children = node.ChildNodes;
            double bestUCT = -1;
            int bestChildIndex = -1;
            for(int i = 0; i < children.Count; i++)
            {
                MCTSNode child = children[i];
                float mui = child.Q/child.N; //Do as multiplication with power of -1?               
                int ni = child.N;
                int N = node.N;
                double C = Math.Sqrt(2);
                double UCT = mui + C * (Math.Sqrt(Math.Log(N)/ni));
                if (UCT > bestUCT)
                {
                    bestUCT = UCT;
                    bestChildIndex = i;
                }
            }
            return children[bestChildIndex];
        }

        //this method is very similar to the bestUCTChild, but it is used to return the final action of the MCTS search, and so we do not care about
        //the exploration factor
        private MCTSNode BestChild(MCTSNode node)
        {
            List<MCTSNode> children = node.ChildNodes;
            double bestUCT = -1;
            int bestChildIndex = -1;
            for (int i = 0; i < children.Count; i++)
            {
                MCTSNode child = children[i];
                float mui = child.Q / child.N; //Do as multiplication with power of -1?
                int ni = child.N;
                int N = node.N;
                double C = 1;
                double UCT = mui + C * (Math.Sqrt(Math.Log(N) / ni));
                if (UCT > bestUCT)
                {
                    bestUCT = UCT;
                    bestChildIndex = i;
                }
            }

            return children[bestChildIndex];
        }
    }
}
