using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UtilLibs.UI.FUI;
using UtilLibs.UIcmp;
using static KTabMenuHeader;
using static STRINGS.UI.UISIDESCREENS.AUTOPLUMBERSIDESCREEN.BUTTONS;

namespace SkillsInfoScreen.UI.UIComponents
{
	internal class MinionSelectButton : KMonoBehaviour, IPointerClickHandler, IPointerDownHandler
	{
		public MinionIdentity Minion;

		public void OnPointerDown(PointerEventData eventData)
		{
			if (KInputManager.isFocused && eventData.button == PointerEventData.InputButton.Left)
			{
				KInputManager.SetUserActive();
				PlaySound(UISoundHelper.ClickOpen);
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return;

			if (eventData.clickCount == 2)
			{
				SelectTool.Instance.SelectAndFocus(Minion.transform.GetPosition(), Minion.GetComponent<KSelectable>(), new Vector3(8f, 0.0f, 0.0f));
			}
			else
			{
				SelectTool.Instance.Select(Minion.GetComponent<KSelectable>());
			}
		}
	}
}
