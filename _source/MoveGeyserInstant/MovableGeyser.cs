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
            if (MoveGeyserTool.Instance == null)
                return;

            bool isGeyser = GetComponent<Geyser>() != null;
            string buttonText = isGeyser ? Strings.Get("STRINGS.MOVEGEYSERINSTANT.MOVE_BUTTON") : "Di chuyển cấu trúc";
            string tooltipText = isGeyser 
                ? Strings.Get("STRINGS.MOVEGEYSERINSTANT.MOVE_TOOLTIP") 
                : "Chọn vị trí mới cho cấu trúc này. Có thể chuyển sang hành tinh khác rồi click để đặt.";

            var button = new KIconButtonMenu.ButtonInfo(
                "action_mirror",
                buttonText,
                new System.Action(ActivateMoveTool),
                global::Action.NumActions,
                tooltipText: tooltipText);

            Game.Instance.userMenu.AddButton(gameObject, button, 1f);
        }

        private void ActivateMoveTool() {
            MoveGeyserTool.Instance.BeginMove(gameObject);
        }
    }
}
