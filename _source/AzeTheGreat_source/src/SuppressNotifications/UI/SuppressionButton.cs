using System;
using System.Collections.Generic;
using System.Linq;

namespace SuppressNotifications
{
    public class SuppressionButton : KMonoBehaviour
    {
        [MyCmpAdd] private StatusItemsSuppressedComp statusItemsSuppressedComp;
        [MyCmpAdd] private NotificationsSuppressedComp notificationsSuppressedComp;

        public override void OnPrefabInit()
        {
            Subscribe((int)GameHashes.RefreshUserMenu, (_) => OnRefreshUserMenu());
        }

        private void OnRefreshUserMenu()
        {
            try
            {
                if (Game.Instance == null || Game.Instance.userMenu == null) return;

                if (AreSuppressable())
                    AddButton(MYSTRINGS.SUPPRESSBUTTON.NAME, OnSuppressClick, GetSuppressableString);
                else if (AreSuppressed())
                    AddButton(MYSTRINGS.CLEARBUTTON.NAME, OnClearClick, GetSuppressedString);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[SuppressNotifications] Lỗi OnRefreshUserMenu: " + e.Message);
            }

            void AddButton(string text, System.Action onClick, Func<string> getTooltip)
            {
                try
                {
                    // For vanilla and SO, must explicitly set the right action since this enum is different.
                    Enum.TryParse(nameof(Action.NumActions), out Action action);

                    Game.Instance.userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo("action_building_disabled", text, onClick, action, tooltipText: getTooltip()));
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("[SuppressNotifications] Lỗi AddButton: " + e.Message);
                }
            }
        }

        internal virtual bool AreSuppressable() => 
            statusItemsSuppressedComp.GetSuppressableStatusItems().Any() || notificationsSuppressedComp.GetSuppressableNotifications().Any();
        internal virtual bool AreSuppressed() => 
            statusItemsSuppressedComp.suppressedStatusItemTitles.Any() || notificationsSuppressedComp.suppressedNotifications.Any();

        internal virtual void OnSuppressClick()
        {
            try
            {
                notificationsSuppressedComp.SuppressNotifications();
                statusItemsSuppressedComp.SuppressStatusItems();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[SuppressNotifications] Lỗi OnSuppressClick: " + e.Message);
            }
        }

        internal virtual void OnClearClick()
        {
            try
            {
                notificationsSuppressedComp.UnsupressNotifications();
                statusItemsSuppressedComp.UnsuppressStatusItems();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[SuppressNotifications] Lỗi OnClearClick: " + e.Message);
            }
        }

        internal virtual string GetSuppressableString()
        {
            return MYSTRINGS.SUPPRESSBUTTON.TOOLTIP + Environment.NewLine
                    + GetStatusItemListText(statusItemsSuppressedComp.GetSuppressableStatusItems())
                    + GetNotificationListText(notificationsSuppressedComp.GetSuppressableNotifications());
        }

        internal virtual string GetSuppressedString()
        {
            return MYSTRINGS.CLEARBUTTON.TOOLTIP + Environment.NewLine
                    + GetStatusItemListText(statusItemsSuppressedComp.suppressedStatusItemTitles)
                    + GetNotificationListText(notificationsSuppressedComp.suppressedNotifications);
        }

        private string GetStatusItemListText(List<StatusItem> statusItems) => GetStatusItemListText(statusItems.Select(x => x.Name).ToList());
        private string GetStatusItemListText(List<string> statusItems) => GetItemListText(MYSTRINGS.STATUS_LABEL, statusItems);

        private string GetNotificationListText(List<Notification> notifications) => GetNotificationListText(notifications.Select(x => x.titleText).ToList());
        private string GetNotificationListText(List<string> notifications) => GetItemListText(MYSTRINGS.NOTIFICATION_LABEL, notifications);

        private string GetItemListText(string label, List<string> items)
        {
            if (!items.Any())
                return string.Empty;

            string text = Environment.NewLine + label + Environment.NewLine;
            foreach (var item in items)
                text += item + Environment.NewLine;

            return text;
        }
    }
}
