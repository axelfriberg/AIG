/* using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement {
    public class DynamicArrive : DynamicSeek {
        public float MaxSpeed { get; set; }

        public float TimeToTarget { get; set; }

        public float TargetRadius { get; set; }

        public float SlowRadius { get; set; }

        public override string Name {
            get { return "Arrive"; }
        }

        public DynamicArrive() {
            //this.TimeToTarget = 1;
            //this.TargetRadius = 1;
            //this.SlowRadius = 10;
        }


        public override MovementOutput GetMovement() {
            float targetSpeed;
            MovementOutput output = new MovementOutput();

            var direction = this.Target.position - this.Character.position;
            var distance = direction.magnitude;

            if (distance < this.TargetRadius) {
                output.linear = Vector3.zero;
                return output;
            }

            if (distance > this.SlowRadius) {
                //maximum speed
                targetSpeed = this.MaxSpeed;
            } else {
                targetSpeed = this.MaxSpeed * distance / this.SlowRadius;
            }

            direction.Normalize();
            direction *= targetSpeed;

            output.linear = direction - this.Character.velocity;
            output.linear /= this.TimeToTarget;


            // If that is too fast, then clip the acceleration
            if (output.linear.sqrMagnitude > this.MaxAcceleration * this.MaxAcceleration) {
                output.linear.Normalize();
                output.linear *= this.MaxAcceleration;
            }

            return output;
        }
    }
} */

using Assets.Scripts.IAJ.Unity.Utils;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement {
    public class DynamicArrive : DynamicVelocityMatch {


        public DynamicArrive() {
        }

        public override string Name {
            get { return "Arrive"; }
        }
        public float MaxSpeed { get; set; }

        public float StopRadius { get; set; }
        public float SlowRadius { get; set; }

        public override MovementOutput GetMovement() {

            Vector3 direction = Target.position - Character.position;
            Debug.Log("Target pos: " + Target.position);
            Debug.Log("Character pos: " + Character.position);
            float distance = Vector3.Magnitude(direction);
            float targetSpeed;

            if (distance <= StopRadius) {
                //Arrived = true;
                //var output = new MovementOutput();
                //return output;
                targetSpeed = 0;
            }

            if (distance > SlowRadius) {
                targetSpeed = MaxSpeed;
            } else {
                targetSpeed = MaxSpeed * (distance / SlowRadius);
            }

            this.MovingTarget = new KinematicData();
            this.MovingTarget.velocity = direction.normalized * targetSpeed;
            //Debug.Log("Distance: " + distance);
            //Debug.Log("Target vel: " + this.MovingTarget.velocity);
            //Debug.Log("Character vel: " + this.Character.velocity);


            return base.GetMovement();
        }
    }
}
