using RAIN.Navigation.Graph;
using Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures.HPStructures;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics {
    public class GatewayHeuristic : IHeuristic {
        private ClusterGraph ClusterGraph { get; set; }

        public GatewayHeuristic(ClusterGraph clusterGraph) {
            this.ClusterGraph = clusterGraph;
        }

        public float H(NavigationGraphNode node, NavigationGraphNode goalNode) {
            int i = 0;
            int j = 0;
            int gatewayFound = 0;
            float finalDistance = float.MaxValue;
            float tempDistance = 0;
            Cluster clusterStart = this.ClusterGraph.Quantize(node);
            Cluster clusterEnd = this.ClusterGraph.Quantize(goalNode);
            if (clusterStart.center == clusterEnd.center) {
                return EuclideanDistance(node.LocalPosition, goalNode.LocalPosition);
            } else {
                foreach (Gateway startGateway in clusterStart.gateways) {
                    float startDistance = EuclideanDistance(node.LocalPosition, startGateway.center);
                    foreach (Gateway endGateway in clusterEnd.gateways) {
                        float endDistance = EuclideanDistance(node.LocalPosition, endGateway.center);
                        if (gatewayFound == 0) {
                            while (true) {
                                if (ClusterGraph.gateways[i].center == startGateway.center) {
                                    break;
                                }
                                i++;
                            }
                            while (true) {
                                if (ClusterGraph.gateways[j].center == endGateway.center) {
                                    break;
                                }
                                j++;
                            }
                        }
                        gatewayFound = 1;
                        float middleDistance = ClusterGraph.gatewayDistanceTable[i].entries[j].shortestDistance;
                        tempDistance = startDistance + middleDistance + endDistance;
                        if (tempDistance < finalDistance) {
                            finalDistance = tempDistance;
                        }
                    }
                }
                return finalDistance;
            }
        }

        public float EuclideanDistance(Vector3 startPosition, Vector3 endPosition) {
            return (endPosition - startPosition).magnitude;
        }
    }
}
