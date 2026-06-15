using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections;

namespace Xrofng
{
    public abstract class BaseControlableMenu : BaseAlphaView
    {
        [ReadOnly]
        [SerializeField] bool _isControlAuthorized = false;

        public bool IsControlAuthorized
        {
            get => _isControlAuthorized;
            private set => _isControlAuthorized = value;
        }

        [FoldoutGroup("Controlable")] public string PanelID = "";
        [FoldoutGroup("Controlable")] public bool ControlByAllPlayer = false;
        [FoldoutGroup("Controlable")] protected int ControllerId = 0;
        [FoldoutGroup("Controlable")] protected bool IsControllableOnStart = true;
        [FoldoutGroup("Controlable")] bool IsAuthorizeOnShowing = true;
        [FoldoutGroup("Controlable")] public float DelayAutoAuthorizeOnShowing = 0.1f;

        protected Vector2 inputDir;

        //private ControlablePanelMod[] _mods;
        private List<PlayerInput> _players = new();

        protected override void Awake()
        {
            base.Awake();
            //_mods = GetComponents<ControlablePanelMod>();
            UpdateControllingPlayers();
        }

        protected override void Start()
        {
            base.Start();

            IsControlAuthorized = IsControllableOnStart;

            if (IsControllableOnStart)
                ShowPanelSilent();
        }

        protected override void UpdateWhileVisible()
        {
            if (!IsControlAuthorized) return;

            foreach (var player in _players)
            {
                var actions = player.actions;

                inputDir = actions["Navigate"].ReadValue<Vector2>();

                ProcessControl(player);
                ProcessControlOfMod(player);
            }
        }

        protected virtual void ProcessControl(PlayerInput player) { }

        void ProcessControlOfMod(PlayerInput player)
        {
            //foreach (var mod in _mods)
            //    mod.DoControlProcessing(player);
        }

        protected override void OnShowing()
        {
            base.OnShowing();

            if (IsAuthorizeOnShowing)
                StartCoroutine(DelaySetAuthorizationRoutine(DelayAutoAuthorizeOnShowing));
        }

        protected override void OnHiding()
        {
            base.OnHiding();
            IsControlAuthorized = false;
        }

        IEnumerator DelaySetAuthorizationRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            IsControlAuthorized = true;
        }

        protected bool GetButton(string actionName)
        {
            foreach (var player in _players)
                if (player.actions[actionName].WasPressedThisFrame())
                    return true;

            return false;
        }

        public void SetControllerId(int id)
        {
            ControllerId = id;
            UpdateControllingPlayers();
        }

        public void SetControlByAllPlayer(bool byAll)
        {
            ControlByAllPlayer = byAll;
            UpdateControllingPlayers();
        }

        void UpdateControllingPlayers()
        {
            _players.Clear();

            if (ControlByAllPlayer)
            {
                _players.AddRange(FindObjectsByType<PlayerInput>(FindObjectsSortMode.None));
            }
            else
            {
                foreach (var p in FindObjectsByType<PlayerInput>(FindObjectsSortMode.None))
                {
                    if (p.playerIndex == ControllerId)
                        _players.Add(p);
                }
            }
        }
    }
}
