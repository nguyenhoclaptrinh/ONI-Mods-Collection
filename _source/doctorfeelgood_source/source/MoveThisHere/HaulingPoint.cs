using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using STRINGS;
using Newtonsoft.Json.Linq;

namespace MoveThisHere
{
    public class HaulingPoint : KMonoBehaviour, ISim1000ms, ISingleSliderControl, IGameObjectEffectDescriptor //, IUserControlledCapacity
    {
#pragma warning disable CS0649
#pragma warning disable IDE0044
        [MyCmpGet]
        private Storage storage;
#pragma warning restore IDE0044
#pragma warning restore CS0649

        [Serialize]
        public bool allowManualPumpingStationFetching;

        [Serialize]
        private float userMaxCapacity = float.PositiveInfinity;

        [Serialize]
        private bool willSelfDestruct = false;

        [Serialize]
        private bool willAutoEject = false;

        [Serialize]
        public bool willSpill = false;

        [Serialize]
        public bool allowItemRemoval = false;

        private Tag[] forbidden_tags;

        public float totalMaxCapacity;
        // Sử dụng một slider tùy biến để lưu trữ cấu hình dung tích tối đa từ người dùng,
        // do lớp giao diện IUserControlledCapacity mặc định của game không hỗ trợ số thập phân.

        public string SliderTitleKey => "Maximum Capacity";

        public string SliderUnits => GameUtil.GetCurrentMassUnit();
        public float GetSliderMax(int index)
        {
            return totalMaxCapacity;
        }

        public float GetSliderMin(int index)
        {
            return 0.0f;
        }

        public float GetSliderValue(int index)
        {
            return userMaxCapacity;
        }

        public string GetSliderTooltip(int index)
        {
            return "Maximum mass to bring to this Hauling Point";//string.Format(Strings.Get(GetSliderTooltipKey(0)), userMaxCapacity);
        }

        public string GetSliderTooltipKey(int index)
        {
            return "";
        }
        public void SetSliderValue(float value, int index)
        {
            if (value != userMaxCapacity) //setslidervalue runs each time slider appears AND if changed - check if actually changed to avoid unncessary job interruptions
            {
                if (value > 100f)
                {
                    value = (float)Math.Round((decimal)value);
                    // Làm tròn số thập phân đối với giá trị trên 100kg để tránh các sai số nhỏ khi kéo thanh slider.
                }
                storage.capacityKg = value;
                userMaxCapacity = value; //set both local and Storage variable, local variable gets kept on save/load
                filteredStorage.FilterChanged();
            }
        }
        public int SliderDecimalPlaces(int index)
        {
            return 3; // Độ chính xác 3 chữ số thập phân (đơn vị g).
        }


        public float AmountStored => storage.MassStored();


        protected override void OnPrefabInit()
        {
            Initialize(use_logic_meter: false);
        }


        protected FilteredStorage filteredStorage;

        public string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;

        private static readonly EventSystem.IntraObjectHandler<HaulingPoint> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<HaulingPoint>(delegate (HaulingPoint component, object data)
        {
            component.OnCopySettings(data);
        });
        private static readonly EventSystem.IntraObjectHandler<HaulingPoint> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<HaulingPoint>(delegate (HaulingPoint component, object data)
        {
            component.OnRefreshUserMenu(data);
        });



        protected void Initialize(bool use_logic_meter)
        {
            //initialize comes first, then spawn
            base.OnPrefabInit();

            ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(choreTypeID);

            forbidden_tags = (allowManualPumpingStationFetching ? new Tag[0] : new Tag[2] { GameTags.LiquidSource, GameTags.GasSource });

            filteredStorage = new FilteredStorage(this, forbidden_tags, null, use_logic_meter, fetch_chore_type);

            Subscribe((int)GameHashes.CopySettings, OnCopySettingsDelegate);
            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenuDelegate);

        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (userMaxCapacity >= totalMaxCapacity)
            {
                userMaxCapacity = totalMaxCapacity;
            }
            storage.capacityKg = userMaxCapacity; // Đồng bộ dung lượng lưu trữ thực tế với cấu hình đã load khi Spawn.

            storage.allowItemRemoval = allowItemRemoval;

            if (allowManualPumpingStationFetching)
            {
                filteredStorage.RemoveForbiddenTag(GameTags.LiquidSource);
                filteredStorage.RemoveForbiddenTag(GameTags.GasSource);
            }
            else
            {
                filteredStorage.AddForbiddenTag(GameTags.LiquidSource);
                filteredStorage.AddForbiddenTag(GameTags.GasSource);
            }
            filteredStorage.FilterChanged();

        }
        private void OnChangeAllowManualPumpingStationFetching()
        {
            allowManualPumpingStationFetching = !allowManualPumpingStationFetching;

            if (allowManualPumpingStationFetching)
            {
                filteredStorage.RemoveForbiddenTag(GameTags.LiquidSource);
                filteredStorage.RemoveForbiddenTag(GameTags.GasSource);
            }
            else
            {
                filteredStorage.AddForbiddenTag(GameTags.LiquidSource);
                filteredStorage.AddForbiddenTag(GameTags.GasSource);
            }
            filteredStorage.FilterChanged();
        }
        private void OnChangeWillSpill()
        {
            willSpill = !willSpill;

        }
        private void ToggleWillSelfDestruct()
        {
            willSelfDestruct = !willSelfDestruct;
            if (willSelfDestruct)
            {
                willAutoEject = false;
            }
        }
        private void ToggleWillAutoEject()
        {
            willAutoEject = !willAutoEject;
            if (willAutoEject)
            {
                willSelfDestruct = false;
            }
        }
        private void ToggleAllowItemRemoval()
        {
            allowItemRemoval = !allowItemRemoval;
            storage.allowItemRemoval = allowItemRemoval;
        }

        private void OnManualEject()
        {
            if (storage != null)
            {
                storage.DropAll(willSpill, willSpill);
                if (filteredStorage != null)
                {
                    filteredStorage.FilterChanged();
                }
            }
        }

        public List<Descriptor> GetDescriptors(GameObject go)
        {
            List<Descriptor> list = new List<Descriptor>();
            
            // Auto-Eject status
            string autoEjectText = willAutoEject ? STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.ENABLED.ToString() : STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.DISABLED.ToString();
            Descriptor autoEjectDesc = new Descriptor(
                string.Format("{0}: {1}", STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.STATUS_AUTO_EJECT, autoEjectText),
                STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_EJECT_ON_TOOLTIP,
                Descriptor.DescriptorType.Effect
            );
            list.Add(autoEjectDesc);

            // Auto-Deconstruct status
            string autoDecText = willSelfDestruct ? STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.ENABLED.ToString() : STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.DISABLED.ToString();
            Descriptor autoDecDesc = new Descriptor(
                string.Format("{0}: {1}", STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.STATUS_AUTO_DECONSTRUCT, autoDecText),
                STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_DROP_ON_TOOLTIP,
                Descriptor.DescriptorType.Effect
            );
            list.Add(autoDecDesc);

            // Retrieval status
            string retrievalText = allowItemRemoval ? STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.ALLOWED.ToString() : STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.DISALLOWED.ToString();
            Descriptor retrievalDesc = new Descriptor(
                string.Format("{0}: {1}", STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.STATUS_RETRIEVAL, retrievalText),
                STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.ALLOW_ITEM_REMOVAL_ON_TOOLTIP,
                Descriptor.DescriptorType.Effect
            );
            list.Add(retrievalDesc);

            return list;
        }

        protected override void OnCleanUp()
        {
            filteredStorage.CleanUp();
        }


        private void OnCopySettings(object data)
        {
            try
            {
                GameObject gameObject = (GameObject)data;
                if (!(gameObject == null))
                {
                    HaulingPoint component = gameObject.GetComponent<HaulingPoint>();
                    if (!(component == null))
                    {
                        //this is copying settings TO the local variables from clipboard component
                        userMaxCapacity = component.userMaxCapacity;
                        storage.capacityKg = userMaxCapacity;
                        willSelfDestruct = component.willSelfDestruct;
                        willAutoEject = component.willAutoEject;
                        willSpill = component.willSpill;
                        allowItemRemoval = component.allowItemRemoval;
                        storage.allowItemRemoval = allowItemRemoval;
                        allowManualPumpingStationFetching = component.allowManualPumpingStationFetching;
                        if (allowManualPumpingStationFetching)
                        {
                            filteredStorage.RemoveForbiddenTag(GameTags.LiquidSource);
                            filteredStorage.RemoveForbiddenTag(GameTags.GasSource);
                        }
                        else
                        {
                            filteredStorage.AddForbiddenTag(GameTags.LiquidSource);
                            filteredStorage.AddForbiddenTag(GameTags.GasSource);
                        }
                        filteredStorage.FilterChanged();
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[MoveThisHere] Lỗi OnCopySettings HaulingPoint: " + e.Message);
            }
		}
		public static JObject Blueprints_GetData(GameObject source)
		{
			if (source.TryGetComponent<HaulingPoint>(out var behavior))
			{
				return new JObject()
				{
					{ "userMaxCapacity", behavior.userMaxCapacity},
					{ "willSelfDestruct", behavior.willSelfDestruct},
					{ "willAutoEject", behavior.willAutoEject},
					{ "willSpill", behavior.willSpill},
					{ "allowItemRemoval", behavior.allowItemRemoval},
					{ "allowManualPumpingStationFetching", behavior.allowManualPumpingStationFetching},
				};
			}
			return null;
		}
		public static void Blueprints_SetData(GameObject target, JObject data)
		{
            try
            {
                if (target.TryGetComponent<HaulingPoint>(out var targetHaulingPoint))
                {
                    var token_userMaxCapacity = data.GetValue("userMaxCapacity");
                    var token_allowManualPumpingStationFetching = data.GetValue("allowManualPumpingStationFetching");
                    var token_willSelfDestruct = data.GetValue("willSelfDestruct");
                    var token_willAutoEject = data.GetValue("willAutoEject");
                    var token_willSpill = data.GetValue("willSpill");
                    var token_allowItemRemoval = data.GetValue("allowItemRemoval");
                    if (token_userMaxCapacity == null || token_willSelfDestruct == null || token_willSpill == null || token_allowManualPumpingStationFetching == null)
                        return;
                    float userMaxCapacity = token_userMaxCapacity.Value<float>();
                    bool willSelfDestruct = token_willSelfDestruct.Value<bool>();
                    bool willAutoEject = token_willAutoEject != null ? token_willAutoEject.Value<bool>() : false;
                    bool willSpill = token_willSpill.Value<bool>();
                    bool allowItemRemoval = token_allowItemRemoval != null ? token_allowItemRemoval.Value<bool>() : false;
                    bool allowManualPumpingStationFetching = token_allowManualPumpingStationFetching.Value<bool>();
                    
                    targetHaulingPoint.userMaxCapacity = userMaxCapacity;
                    targetHaulingPoint.storage.capacityKg = userMaxCapacity;
                    targetHaulingPoint.willSelfDestruct = willSelfDestruct;
                    targetHaulingPoint.willAutoEject = willAutoEject;
                    targetHaulingPoint.willSpill = willSpill;
                    targetHaulingPoint.allowItemRemoval = allowItemRemoval;
                    targetHaulingPoint.storage.allowItemRemoval = allowItemRemoval;
                    targetHaulingPoint.allowManualPumpingStationFetching = allowManualPumpingStationFetching;
                    if (allowManualPumpingStationFetching)
                    {
                        targetHaulingPoint.filteredStorage.RemoveForbiddenTag(GameTags.LiquidSource);
                        targetHaulingPoint.filteredStorage.RemoveForbiddenTag(GameTags.GasSource);
                    }
                    else
                    {
                        targetHaulingPoint.filteredStorage.AddForbiddenTag(GameTags.LiquidSource);
                        targetHaulingPoint.filteredStorage.AddForbiddenTag(GameTags.GasSource);
                    }
                    targetHaulingPoint.filteredStorage.FilterChanged();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[MoveThisHere] Lỗi Blueprints_SetData: " + e.Message);
            }
		}

		private void OnRefreshUserMenu(object data)
        {
            try
            {
                if (Game.Instance == null || Game.Instance.userMenu == null) return;

                KIconButtonMenu.ButtonInfo autoBottleButton = (allowManualPumpingStationFetching ?
                    new KIconButtonMenu.ButtonInfo("action_bottler_delivery", STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_BOTTLE_OFF, OnChangeAllowManualPumpingStationFetching, Action.NumActions, null, null, null, STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_BOTTLE_OFF_TOOLTIP) :
                    new KIconButtonMenu.ButtonInfo("action_bottler_delivery", STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_BOTTLE_ON, OnChangeAllowManualPumpingStationFetching, Action.NumActions, null, null, null, STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_BOTTLE_ON_TOOLTIP));
                Game.Instance.userMenu.AddButton(base.gameObject, autoBottleButton, 0.4f);

                KIconButtonMenu.ButtonInfo autoDropButton = (willSelfDestruct ?
                    new KIconButtonMenu.ButtonInfo("action_empty_contents", STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_DROP_OFF, ToggleWillSelfDestruct, Action.NumActions, null, null, null, STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_DROP_OFF_TOOLTIP) :
                    new KIconButtonMenu.ButtonInfo("action_empty_contents", STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_DROP_ON, ToggleWillSelfDestruct, Action.NumActions, null, null, null, STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_DROP_ON_TOOLTIP));
                Game.Instance.userMenu.AddButton(base.gameObject, autoDropButton);

                KIconButtonMenu.ButtonInfo autoEjectButton = (willAutoEject ?
                    new KIconButtonMenu.ButtonInfo("action_empty_contents", STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_EJECT_OFF, ToggleWillAutoEject, Action.NumActions, null, null, null, STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_EJECT_OFF_TOOLTIP) :
                    new KIconButtonMenu.ButtonInfo("action_empty_contents", STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_EJECT_ON, ToggleWillAutoEject, Action.NumActions, null, null, null, STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_EJECT_ON_TOOLTIP));
                Game.Instance.userMenu.AddButton(base.gameObject, autoEjectButton);

                KIconButtonMenu.ButtonInfo allowItemRemovalButton = (allowItemRemoval ?
                    new KIconButtonMenu.ButtonInfo("action_open_door", STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.ALLOW_ITEM_REMOVAL_OFF, ToggleAllowItemRemoval, Action.NumActions, null, null, null, STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.ALLOW_ITEM_REMOVAL_OFF_TOOLTIP) :
                    new KIconButtonMenu.ButtonInfo("action_open_door", STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.ALLOW_ITEM_REMOVAL_ON, ToggleAllowItemRemoval, Action.NumActions, null, null, null, STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.ALLOW_ITEM_REMOVAL_ON_TOOLTIP));
                Game.Instance.userMenu.AddButton(base.gameObject, allowItemRemovalButton);

                KIconButtonMenu.ButtonInfo autoSpillButton = (willSpill ?
                    new KIconButtonMenu.ButtonInfo("action_bottler_delivery", STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_SPILL_OFF, OnChangeWillSpill, Action.NumActions, null, null, null, STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_SPILL_OFF_TOOLTIP) :
                    new KIconButtonMenu.ButtonInfo("action_bottler_delivery", STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_SPILL_ON, OnChangeWillSpill, Action.NumActions, null, null, null, STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.AUTO_SPILL_ON_TOOLTIP));
                Game.Instance.userMenu.AddButton(base.gameObject, autoSpillButton);

                KIconButtonMenu.ButtonInfo manualEjectButton = new KIconButtonMenu.ButtonInfo(
                    "action_empty_contents", 
                    STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.MANUAL_EJECT, 
                    OnManualEject, 
                    Action.NumActions, 
                    null, null, null, 
                    STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.MANUAL_EJECT_TOOLTIP);
                Game.Instance.userMenu.AddButton(base.gameObject, manualEjectButton, 0.5f);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[MoveThisHere] Lỗi OnRefreshUserMenu HaulingPoint: " + e.Message);
            }
        }

        public void Sim1000ms(float dt)
        {
            bool isFull = (AmountStored / userMaxCapacity) >= 0.99f;

            if (isFull)
            {
                if (willSelfDestruct)
                {
                    GetComponentInParent<DeconstructableHaulingPoint>().OnDeconstruct();
                }
                else if (willAutoEject)
                {
                    storage.DropAll(willSpill, willSpill);
                }
            }
        }

    }

    public class DeconstructableHaulingPoint : Workable
    {

        //modified deconstructable to replace default behavior, this one will deconstruct instantly when given decon order
        //however it won't drop any resources from the building itself, important because it's made of vacuum and this gives an error
        //also drops gas resource in canister form

        private static readonly EventSystem.IntraObjectHandler<DeconstructableHaulingPoint> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<DeconstructableHaulingPoint>(delegate (DeconstructableHaulingPoint component, object data)
        {
            component.OnRefreshUserMenu(data);
        });
        private static readonly EventSystem.IntraObjectHandler<DeconstructableHaulingPoint> OnDeconstructDelegate = new EventSystem.IntraObjectHandler<DeconstructableHaulingPoint>(delegate (DeconstructableHaulingPoint component, object data)
        {
            component.OnDeconstruct();
        });
        private CellOffset[] placementOffsets
        {
            get
            {
                Building component = GetComponent<Building>();
                if (component != null)
                {
                    return component.Def.PlacementOffsets;
                }

                Debug.Assert(condition: false, "[MoveThisHere] Khong tim thay Component Building tren doi tuong.", this);
                return null;

            }
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenuDelegate);
            Subscribe((int)GameHashes.StatusChange, OnRefreshUserMenuDelegate);
            Subscribe((int)GameHashes.MarkForDeconstruct, OnDeconstructDelegate);

            CellOffset[][] table = OffsetGroups.InvertedStandardTable;
            CellOffset[] filter = null;
            CellOffset[][] offsetTable = OffsetGroups.BuildReachabilityTable(placementOffsets, table, filter);
            SetOffsetTable(offsetTable);
            // Ke thua cau hinh vung tiep can (reachability offset) tu class deconstructable goc.


        }
        protected override void OnSpawn()
        {
            base.OnSpawn();

        }
        public void OnDeconstruct()
        {

            Storage storage = base.GetComponent<Storage>();
            HaulingPoint haulingPoint = base.GetComponent<HaulingPoint>();

            storage.DropAll(haulingPoint.willSpill, haulingPoint.willSpill); //drop liquids and gasses based on setting

            base.gameObject.DeleteObject(); //goodbye
        }


        private void OnRefreshUserMenu(object data)
        {
            if (!this.HasTag(GameTags.Stored))
            {
                KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo(
                    "action_deconstruct",
                    STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.REMOVE,
                    OnDeconstruct,
                    Action.NumActions,
                    null, null, null,
                    STRINGS.BUILDINGS.BUTTONS.HAULINGPOINT.REMOVE_TOOLTIP);
                Game.Instance.userMenu.AddButton(base.gameObject, button, 0f);
                // Them nut bam thao do cong trinh vao User Menu.
            }
        }



    }





}