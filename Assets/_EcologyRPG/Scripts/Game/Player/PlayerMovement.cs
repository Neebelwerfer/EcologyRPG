using EcologyRPG.Core.Character;
using EcologyRPG.Utility;
using log4net.Util;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EcologyRPG.GameSystems.PlayerSystems
{
    public class PlayerMovement
    {
        readonly InputActionReference Movement;
        readonly float rotationSpeed = 4f;

        //Rotated forward and right vectors to match the camera
        readonly Vector3 forward = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.forward;
        readonly Vector3 right = Quaternion.Euler(new Vector3(0, 135, 0)) * Vector3.forward;

        readonly PlayerCharacter player;

        //Cached Character references
        Stat MovementSpeed;

        StatModification sprintMod;

        int isStill = Animator.StringToHash("Is_Still");
        int isWalking = Animator.StringToHash("Is_Walking");
        int isRunning = Animator.StringToHash("Is_Running");

        public PlayerMovement(PlayerCharacter player)
        {
            this.player = player;
            Movement = player.playerSettings.Movement;
            Movement.action.Enable();
            rotationSpeed = player.playerSettings.rotationSpeed;
            MovementSpeed = player.Stats.GetStat("movementSpeed");
        }

        public void FixedUpdate()
        {
            var transform = player.Transform;
            var animator = player.Animator;

            var rb = player.Rigidbody;
            Vector2 movement = Movement.action.ReadValue<Vector2>();

            if(!player.CanMove || player.state == CharacterStates.disabled || player.state == CharacterStates.dead || player.state == CharacterStates.dodging )
            {
                return;
            }

            if (movement.magnitude > 0)
            {
                animator.SetBool(isStill, false);
                animator.SetBool(isWalking, true);
                var speed = MovementSpeed.Value * Characters.BaseMoveSpeed * TimeManager.IngameDeltaTime;
                var dir = (movement.y * forward + movement.x * right).normalized;
                if(Physics.Raycast(transform.Position, Vector3.down, out RaycastHit hit, 100, Game.Settings.GroundMask))
                {
                    dir = Vector3.ProjectOnPlane(dir, hit.normal).normalized;
                }

                if(IsLegalMove(player, dir, speed))
                {
                    transform.Move(dir, speed);
                }
                if (player.CanRotate) transform.Rotation = Quaternion.Slerp(transform.Rotation, Quaternion.LookRotation(dir), TimeManager.IngameDeltaTime * rotationSpeed);
            }
            else
            {
                animator.SetBool(isWalking, false);
                animator.SetBool(isStill, true);
                if(player.CanRotate)
                {
                    UpdateRotationBasedOnMouse();
                }
            }
        }

        public static bool IsLegalMove(BaseCharacter character, Vector3 dir, float speed, bool doWallTest = false)
        {
            bool groundTest = false;
            bool wallTest = true;
            var origin = character.Transform.Position;
            var checkPos = origin + dir * (speed * 2);
            checkPos.y += 1f;
            Debug.DrawRay(checkPos, Vector3.down * 1, Color.red);
            if (Physics.Raycast(checkPos, Vector3.down, out var hit, 30, Game.Settings.GroundMask))
            {
                if (hit.distance < 2f)
                    groundTest = true;
            }

            if (doWallTest)
            {
                checkPos.y -= 0.5f;
                Debug.DrawRay(checkPos, character.Transform.Forward * 0.5f, Color.blue);
                if (Physics.SphereCast(checkPos, 0.5f, character.Transform.Forward, out var hit1, 0.5f, ~Game.Settings.EntityMask, QueryTriggerInteraction.Ignore))
                {
                    wallTest = false;
                }
            }
           
            return groundTest && wallTest;
        }

        public void UpdateRotationBasedOnMouse()
        {
            var transform = player.Transform;
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
            {
                var dir = (hit.point - transform.Position).normalized;
                dir = Vector3.ProjectOnPlane(dir, Vector3.up).normalized;
                var rot = Quaternion.Slerp(transform.Rotation, Quaternion.LookRotation(dir), TimeManager.IngameDeltaTime * rotationSpeed);
                transform.Rotation = rot;
            }
        }
    }
}