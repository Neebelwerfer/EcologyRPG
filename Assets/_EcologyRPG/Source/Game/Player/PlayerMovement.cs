using EcologyRPG.Core.Character;
using EcologyRPG.Utility;
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


        //Cached Character references
        readonly PlayerCharacter player;
        readonly Stat MovementSpeed;
        readonly int isStill = Animator.StringToHash("Is_Still");
        readonly int isWalking = Animator.StringToHash("Is_Walking");

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
                if(player.playerSettings.Walking != null)
                {
                    if(player.AudioSource.clip != player.playerSettings.Walking)
                        player.AudioSource.clip = player.playerSettings.Walking;
                    if(!player.AudioSource.isPlaying) 
                        player.AudioSource.Play();

                }
                var speed = MovementSpeed.Value * Characters.BaseMoveSpeed;
                var dir = (movement.y * forward + movement.x * right).normalized;
                if(Physics.Raycast(transform.Position, Vector3.down, out RaycastHit hit, 100, Game.Settings.WalkableGroundMask))
                {
                    dir = Vector3.ProjectOnPlane(dir, hit.normal).normalized;
                }

                if(BaseCharacter.IsLegalMove(player, dir, speed * TimeManager.IngameDeltaTime))
                {
                    Move(dir, speed);
                }
                else rb.velocity = Physics.gravity;
                if (player.CanRotate) rb.MoveRotation(Quaternion.Slerp(transform.Rotation, Quaternion.LookRotation((movement.y * forward + movement.x * right)), TimeManager.IngameDeltaTime * rotationSpeed));
            }
            else
            {
                if(player.AudioSource.isPlaying) player.AudioSource.Stop();
                animator.SetBool(isWalking, false);
                animator.SetBool(isStill, true);
                if(player.CanRotate)
                {
                    UpdateRotationBasedOnMouse();
                }
            }
        }

        void Move(Vector3 dir, float speed)
        {
            player.Rigidbody.velocity = dir * speed;
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
                player.Rigidbody.MoveRotation(rot);
            }
        }
    }
}