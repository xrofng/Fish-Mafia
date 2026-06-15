using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Xrofng
{
    public class DirectionalInputPanel : BaseControlableMenu
    {
        [System.Flags]
        public enum DirectionReceive
        {
            None = 0,
            X = 1 << 0,
            Y = 1 << 1,
            Shoulder = 1 << 2,
        }

        [FoldoutGroup("Controlable")]
        public DirectionReceive ValidDirectionInput = DirectionReceive.X;

        [ReadOnly, SerializeField]
        private bool authorizeDirectionalControl;

        private Vector2 _lastDir;

        protected override void ProcessControl(PlayerInput player)
        {
            base.ProcessControl(player);

            if (!authorizeDirectionalControl)
                return;

            var actions = player.actions;

            Vector2 nav = actions["Navigate"].ReadValue<Vector2>();

            int playerId = player.playerIndex;

            // X Axis
            if (ValidDirectionInput.HasFlag(DirectionReceive.X))
            {
                if (nav.x > 0.5f)
                {
                    DoHorizontalReceived(1, playerId);
                    DoDirectionReceived(1, playerId);
                }
                else if (nav.x < -0.5f)
                {
                    DoHorizontalReceived(-1, playerId);
                    DoDirectionReceived(-1, playerId);
                }
            }

            // Y Axis
            if (ValidDirectionInput.HasFlag(DirectionReceive.Y))
            {
                if (nav.y > 0.5f)
                {
                    DoVerticalReceived(1, playerId);
                    DoDirectionReceived(1, playerId);
                }
                else if (nav.y < -0.5f)
                {
                    DoVerticalReceived(-1, playerId);
                    DoDirectionReceived(-1, playerId);
                }
            }

            // Shoulder
            if (ValidDirectionInput.HasFlag(DirectionReceive.Shoulder))
            {
                if (actions["Previous"].IsPressed())
                {
                    DoShoulderReceived(-1, playerId);
                    DoDirectionReceived(-1, playerId);
                }

                if (actions["Next"].IsPressed())
                {
                    DoShoulderReceived(1, playerId);
                    DoDirectionReceived(1, playerId);
                }
            }
        }

        protected virtual void DoDirectionReceived(int increment, int playerId) { }
        protected virtual void DoHorizontalReceived(int increment, int playerId) { }
        protected virtual void DoVerticalReceived(int increment, int playerId) { }
        protected virtual void DoShoulderReceived(int increment, int playerId) { }

        protected override void OnShowing()
        {
            base.OnShowing();
            authorizeDirectionalControl = true;
        }

        protected override void OnHiding()
        {
            base.OnHiding();
            authorizeDirectionalControl = false;
        }

        public void SetAuthorizeDirectionalControl(bool auth)
        {
            authorizeDirectionalControl = auth;
        }
    }
}
