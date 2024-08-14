using System;
using HarmonyLib;
using Sandbox.Game.Entities.Character.Components;
using VRageMath;

namespace thejoun.JetpackGravityAlignment
{
    [HarmonyPatch(typeof(MyCharacterJetpackComponent))]
    [HarmonyPatch("MoveAndRotate")]
    public class Patch
    {
        static void Postfix(MyCharacterJetpackComponent __instance, ref Vector2 rotationIndicator)
        {
            var character = __instance.Character;
            var gravity = character.Gravity;
            var matrix = character.WorldMatrix;
            var gravityUp = -gravity.Normalized();
            
            var isAffectedByGravity = gravity.LengthSquared() > 0.1f;
            var isYawing = Math.Abs(rotationIndicator.Y) > 1.40129846432482E-45;

            if (!isAffectedByGravity) return;

            if (isYawing)
            {
                // revert default yaw and reapply around gravity axis
                var angle = (double) rotationIndicator.Y * character.RotationSpeed * 0.0199999995529652;
                var revertYaw = MatrixD.CreateFromAxisAngle(matrix.Up, angle);
                var reapplyRotation = MatrixD.CreateFromAxisAngle(gravityUp, -angle);
                var height = character.ModelCollision.BoundingBoxSizeHalf.Y;
                var position = character.Physics.GetWorldMatrix().Translation + matrix.Up * height;
                matrix = matrix.GetOrientation() * revertYaw * reapplyRotation;
                matrix.Translation = position - matrix.Up * height;
            }
            
            // ensure alignment with plane
            var right = matrix.Right;
            var up = matrix.Up;
            var dotRight = Vector3.Dot(right, gravityUp);
            var newRight = (right - dotRight * gravityUp).Normalized();
            var dotUp = Vector3.Dot(up, newRight);
            var newUp = (up - dotUp * newRight).Normalized();
            matrix.Right = newRight;
            matrix.Up = newUp;

            character.WorldMatrix = matrix;
        }
    }
}