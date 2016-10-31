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
            float finalDistance = float.MaxValue;
            float tempDistance = 0;
            Cluster clusterStart = ScriptableObject.CreateInstance<Cluster>();
            Cluster clusterEnd = ScriptableObject.CreateInstance<Cluster>();
            clusterStart = this.ClusterGraph.Quantize(node);
            clusterEnd = this.ClusterGraph.Quantize(goalNode);
            if ((object.ReferenceEquals(null, clusterStart)) || (object.ReferenceEquals(null, clusterEnd))) {
                return EuclideanDistance(node.LocalPosition, goalNode.LocalPosition);
            }
            if (clusterStart.center == clusterEnd.center) {
                return EuclideanDistance(node.LocalPosition, goalNode.LocalPosition);
            } else {
                foreach (Gateway startGateway in clusterStart.gateways) {
                    float startDistance = EuclideanDistance(node.LocalPosition, startGateway.center);
                    foreach (Gateway endGateway in clusterEnd.gateways) {
                        float endDistance = EuclideanDistance(goalNode.LocalPosition, endGateway.center);
                        while (true) {
                            //Debug.Log(1);
                            if (ClusterGraph.gateways[i].center == startGateway.center) {
                                //Debug.Log(ClusterGraph.gateways[i].center);
                                //Debug.Log(startGateway.center);
                                break;
                            }
                            i++;
                        }
                        while (true) {
                            //Debug.Log(1);
                            //Debug.Log(ClusterGraph.gateways[j].center);
                            //Debug.Log(endGateway.center);
                            if (ClusterGraph.gateways[j].center == endGateway.center) {
                                //Debug.Log(1);
                                //Debug.Log(ClusterGraph.gateways[j].center);
                                //Debug.Log(endGateway.center);
                                break;
                            }
                            j++;
                        }
                        float middleDistance = ClusterGraph.gatewayDistanceTable[i].entries[j].shortestDistance;
                        i = 0;
                        j = 0;
                        tempDistance = startDistance + middleDistance + endDistance;
                        if (tempDistance < finalDistance) {
                            finalDistance = tempDistance;
                        }
                    }
                }
                return finalDistance;
            }
        }


        /*public float H(NavigationGraphNode node, NavigationGraphNode goalNode) {
            float finalDistance = float.MaxValue;
            Cluster clusterStart = ScriptableObject.CreateInstance<Cluster>();
            Cluster clusterEnd = ScriptableObject.CreateInstance<Cluster>();
            Cluster clusterTemp1 = ScriptableObject.CreateInstance<Cluster>();
            Cluster clusterTemp2 = ScriptableObject.CreateInstance<Cluster>();
            clusterStart = this.ClusterGraph.Quantize(node);
            clusterEnd = this.ClusterGraph.Quantize(goalNode);
            if ((object.ReferenceEquals(null, clusterStart)) || (object.ReferenceEquals(null, clusterEnd))) {
                return EuclideanDistance(node.LocalPosition, goalNode.LocalPosition);
            }
            if (clusterStart.center == clusterEnd.center) {
                return EuclideanDistance(node.LocalPosition, goalNode.LocalPosition);
            } else {
                foreach (Gateway startGateway in clusterStart.gateways) {
                    while (true) {
                        if (startGateway.clusters[0] == clusterStart) {
                            clusterTemp2 = startGateway.clusters[1];
                            clusterTemp1 = startGateway.clusters[0];
                        } else {
                            clusterTemp2 = startGateway.clusters[0];
                            clusterTemp1 = startGateway.clusters[1];
                        }
                    }
                }
            }
            return finalDistance;
        } */

        public float EuclideanDistance(Vector3 startPosition, Vector3 endPosition) {
            return (endPosition - startPosition).magnitude;
        }
    }
}