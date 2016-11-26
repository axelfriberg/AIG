using RAIN.Navigation.Graph;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics {
    public class EuclideanHeuristic : IHeuristic {
        public float H(NavigationGraphNode node, NavigationGraphNode goalNode) {
            return (goalNode.Position - node.Position).magnitude;
        }

        public float H(Vector3 node, Vector3 goalNode) {
            return (goalNode - node).magnitude;
        }
    }
}
