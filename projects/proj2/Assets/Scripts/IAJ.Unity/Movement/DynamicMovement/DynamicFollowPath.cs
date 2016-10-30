using Assets.Scripts.IAJ.Unity.Pathfinding.Path;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement {
    public class DynamicFollowPath : DynamicArrive {
        public Path Path { get; set; }
        public float PathOffset { get; set; }

        public float CurrentParam { get; set; }

        private MovementOutput EmptyMovementOutput { get; set; }


        public DynamicFollowPath(KinematicData character, Path path) {

            //arrive properties
            this.MaxSpeed = 100.0f;
            this.SlowRadius = 5.0f;

            this.TimeToTargetSpeed = 5.0f;
            this.StopRadius = 0.5f;

            this.MaxAcceleration = 20.0f;

            this.Target = new KinematicData();
            this.Character = character;
            this.Path = path;
            this.CurrentParam = 0.0f;
            this.PathOffset = 0.1f;
            this.EmptyMovementOutput = new MovementOutput();
        }

        public override MovementOutput GetMovement() {
            //Debug.Log(this.Character.position);
            if (this.Path == null) {
                Debug.Log("path null");
                return this.EmptyMovementOutput;
            }

            this.CurrentParam = this.Path.GetParam(this.Character.position, this.CurrentParam);
            //Debug.Log(CurrentParam);


            if (this.Path.PathEnd(this.CurrentParam)) {
                //Debug.Log("path end");
                Character.velocity = new Vector3(0, 0, 0);
                return EmptyMovementOutput;
            }

            var targetParam = this.CurrentParam + PathOffset;
            //Debug.Log(Target.position);
            Target.position = this.Path.GetPosition(targetParam);
            //Debug.Log(Target.position);
            //Debug.Log("return getmovement");
            return base.GetMovement();
        }
    }
} 



