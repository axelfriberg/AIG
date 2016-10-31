using Assets.Scripts.IAJ.Unity.Pathfinding.Path;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement {
    public class DynamicFollowPath : DynamicArrive {
        public Path Path { get; set; }
        public float PathOffset { get; set; }

        public float CurrentParam { get; set; }

        private MovementOutput EmptyMovementOutput { get; set; }

        public DynamicFollowPath(KinematicData character, Path path) {

            this.MaxSpeed = 150.0f;
            this.SlowRadius = 5.0f;
            this.TimeToTargetSpeed = 1.0f;
            this.StopRadius = 0.5f;
            this.MaxAcceleration = 50.0f;

            this.Target = new KinematicData();
            this.Character = character;
            this.Path = path;
            this.CurrentParam = 0.0f;
            this.PathOffset = 1.0f;
            this.EmptyMovementOutput = new MovementOutput();
        }

        public override MovementOutput GetMovement() {
            if (this.Path == null) {
                return this.EmptyMovementOutput;
            }

            this.CurrentParam = this.Path.GetParam(this.Character.position, this.CurrentParam);

            if (this.Path.PathEnd(this.CurrentParam)) {
                Character.velocity = new Vector3(0, 0, 0);
                return EmptyMovementOutput;
            }

            var targetParam = this.CurrentParam + PathOffset;
            Target.position = this.Path.GetPosition(targetParam);
                
            return base.GetMovement();
        }
    }
} 



