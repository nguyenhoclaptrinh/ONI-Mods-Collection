using System;
using UnityEngine;

namespace MoveGeyserInstant {
    public sealed class MovableGeyser : KMonoBehaviour {
        private static readonly EventSystem.IntraObjectHandler<MovableGeyser> RefreshUserMenuHandler =
            new EventSystem.IntraObjectHandler<MovableGeyser>((component, data) => component.OnRefreshUserMenu());

        public override void OnSpawn() {
            base.OnSpawn();
            Subscribe((int)GameHashes.RefreshUserMenu, RefreshUserMenuHandler);
        }

        private void OnRefreshUserMenu() {
            if (GetComponent<Geyser>() == null || MoveGeyserTool.Instance == null)
                return;

            var button = new KIconButtonMenu.ButtonInfo(
                "action_mirror",
                Strings.Get("STRINGS.MOVEGEYSERINSTANT.MOVE_BUTTON"),
                new System.Action(ActivateMoveTool),
                global::Action.NumActions,
                tooltipText: Strings.Get("STRINGS.MOVEGEYSERINSTANT.MOVE_TOOLTIP"));

            Game.Instance.userMenu.AddButton(gameObject, button, 1f);
        }

        private void ActivateMoveTool() {
            MoveGeyserTool.Instance.BeginMove(gameObject);
        }
    }
}
