using Assets.Scripts.IAJ.Unity.Pathfinding.Path;
using System;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement {
    public class DynamicFollowPath : DynamicArrive {
        public Path Path { get; set; }
        public float PathOffset { get; set; }

        public float CurrentParam { get; set; }

        private MovementOutput EmptyMovementOutput { get; set; }


        public DynamicFollowPath(KinematicData character, Path path) {
            this.Target = new KinematicData();
            this.Character = character;
            this.Path = path;
            this.EmptyMovementOutput = new MovementOutput();
            this.MaxSpeed = 50.0f;
            this.SlowRadius = 15.0f;
            this.StopRadius = 3.0f;

        }

        public override MovementOutput GetMovement() {
            float targetParam = CurrentParam + PathOffset;
            Target.position = Path.GetPosition(targetParam);
            return base.GetMovement();
        }
    }
}
