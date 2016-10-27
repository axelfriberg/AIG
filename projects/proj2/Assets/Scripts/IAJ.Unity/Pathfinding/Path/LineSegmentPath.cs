using Assets.Scripts.IAJ.Unity.Utils;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Path {
    public class LineSegmentPath : LocalPath {
        protected Vector3 LineVector;
        public LineSegmentPath(Vector3 start, Vector3 end) {
            this.StartPosition = start;
            this.EndPosition = end;
            this.LineVector = end - start;
        }

        public override Vector3 GetPosition(float param) {
            Vector3 VectorAux, Dist;

            Dist.x = EndPosition.x - StartPosition.x;
            Dist.z = EndPosition.z - StartPosition.z;

            VectorAux.x = StartPosition.x + Dist.x * param;
            VectorAux.y = StartPosition.y;
            VectorAux.z = StartPosition.z + Dist.z * param;

            return VectorAux;
        }

        public override bool PathEnd(float param) {
            if (param <= 0.99f) {
                return false;
            } else
                return true;
        }

        public override float GetParam(Vector3 position, float lastParam) {

            return MathHelper.closestParamInLineSegmentToPoint(StartPosition, EndPosition, position);
        }
    }
}
