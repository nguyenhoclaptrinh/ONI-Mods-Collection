using System;
using UnityEngine;
using KSerialization;

namespace MoveGeyserInstant {
    [SerializationConfig(MemberSerialization.OptIn)]
    public sealed class MovableGeyser : KMonoBehaviour {
        [Serialize] public bool keepAnalysis = true;
        [Serialize] public bool keepEruptionState = true;
        [Serialize] public bool resetVacillator = false;

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
                GetMoveIconName(),
                buttonText,
                new System.Action(ActivateMoveTool),
                global::Action.NumActions,
                tooltipText: tooltipText);

            Game.Instance.userMenu.AddButton(gameObject, button, 1f);

            if (isGeyser) {
                // Nút Toggle Giữ phân tích
                string analysisText = keepAnalysis ? "Giữ phân tích: BẬT" : "Giữ phân tích: TẮT";
                var analysisButton = new KIconButtonMenu.ButtonInfo(
                    "action_research",
                    analysisText,
                    () => {
                        keepAnalysis = !keepAnalysis;
                        Game.Instance.userMenu.Refresh(gameObject);
                    },
                    global::Action.NumActions,
                    tooltipText: "Quyết định xem có giữ nguyên trạng thái đã phân tích của mạch sau khi di chuyển hay bắt phân tích lại.");
                Game.Instance.userMenu.AddButton(gameObject, analysisButton, 0.9f);

                // Nút Toggle Giữ chu kỳ
                string eruptionText = keepEruptionState ? "Giữ chu kỳ: BẬT" : "Giữ chu kỳ: TẮT";
                var eruptionButton = new KIconButtonMenu.ButtonInfo(
                    "action_direction_right",
                    eruptionText,
                    () => {
                        keepEruptionState = !keepEruptionState;
                        Game.Instance.userMenu.Refresh(gameObject);
                    },
                    global::Action.NumActions,
                    tooltipText: "Quyết định xem có giữ nguyên thời gian và chu kỳ phun trào hiện tại của mạch hay reset ngẫu nhiên.");
                Game.Instance.userMenu.AddButton(gameObject, eruptionButton, 0.8f);
            }

            if (GetComponent<GeneShuffler>() != null) {
                string vacillatorText = resetVacillator ? "Reset Vacillator: BẬT" : "Reset Vacillator: TẮT";
                var vacillatorButton = new KIconButtonMenu.ButtonInfo(
                    "action_recharge",
                    vacillatorText,
                    () => {
                        resetVacillator = !resetVacillator;
                        Game.Instance.userMenu.Refresh(gameObject);
                    },
                    global::Action.NumActions,
                    tooltipText: "Quyết định xem có reset sạc lại thiết bị sau khi di chuyển để sử dụng tiếp hay giữ nguyên trạng thái đã sử dụng.");
                Game.Instance.userMenu.AddButton(gameObject, vacillatorButton, 0.9f);
            }
        }

        private void ActivateMoveTool() {
            MoveGeyserTool.Instance.BeginMove(gameObject);
        }

        private static string GetMoveIconName() {
            if (Assets.GetSprite("MoveGeyserToolIcon") != null) {
                return "MoveGeyserToolIcon";
            }
            return "action_move_to_storage";
        }
    }
}
