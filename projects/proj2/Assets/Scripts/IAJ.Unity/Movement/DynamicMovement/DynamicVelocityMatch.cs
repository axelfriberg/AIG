namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement {
    public class DynamicVelocityMatch : DynamicMovement {
        public override string Name {
            get { return "VelocityMatch"; }
        }

        public override KinematicData Target { get; set; }

        public float TimeToTargetSpeed { get; set; }

        public KinematicData MovingTarget { get; set; }

        public DynamicVelocityMatch() {
            //TimeToTargetSpeed = 0.5f;
        }
        public override MovementOutput GetMovement() {
            var output = new MovementOutput();
            output.linear = (MovingTarget.velocity - Character.velocity) / TimeToTargetSpeed;

            if (output.linear.sqrMagnitude > MaxAcceleration * MaxAcceleration) {
                output.linear = output.linear.normalized * MaxAcceleration;
            }
            output.angular = 0;
            return output;
        }
    }
}