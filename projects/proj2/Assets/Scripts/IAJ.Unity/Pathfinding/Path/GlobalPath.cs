using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.Utils;
using RAIN.Navigation.Graph;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Path {
    public class GlobalPath : Path {
        public List<NavigationGraphNode> PathNodes { get; protected set; }
        public List<Vector3> PathPositions { get; protected set; }
        public bool IsPartial { get; set; }
        public float Length { get; set; }
        public List<LocalPath> LocalPaths { get; protected set; }


        public GlobalPath() {
            this.PathNodes = new List<NavigationGraphNode>();
            this.PathPositions = new List<Vector3>();
            this.LocalPaths = new List<LocalPath>();
        }

        public void CalculateLocalPathsFromPathPositions(Vector3 initialPosition) {
            Vector3 previousPosition = initialPosition;
            for (int i = 0; i < this.PathPositions.Count; i++) {

                if (!previousPosition.Equals(this.PathPositions[i])) {
                    this.LocalPaths.Add(new LineSegmentPath(previousPosition, this.PathPositions[i]));
                    previousPosition = this.PathPositions[i];
                }
            }
        }

        public override float GetParam(Vector3 position, float previousParam) {
            return MathHelper.closestParamInLineSegmentToPoint(PathNodes[0].Position, PathNodes[PathNodes.Count-1].Position, position);

        }

        public override Vector3 GetPosition(float param) {
            Vector3 VectorAux, Dist;
            Vector3 EndPosition = PathPositions[PathPositions.Count - 1];
            Vector3 StartPosition = PathPositions[0];
            Dist.x = EndPosition.x - StartPosition.x;
            Dist.z = EndPosition.z - StartPosition.z;

            VectorAux.x = StartPosition.x + Dist.x * param;
            VectorAux.y = StartPosition.y;
            VectorAux.z = StartPosition.z + Dist.z * param;

            return VectorAux;
        }

        public override bool PathEnd(float param) {
            float paramMax = LocalPaths.Count;
            if (param <= paramMax-0.01) {
                return false;
            } else
                return true;
        }
    }
}
