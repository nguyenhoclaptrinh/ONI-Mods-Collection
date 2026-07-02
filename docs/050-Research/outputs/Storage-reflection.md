---
id: storage-reflection
type: research-output
status: done
created: 2026-06-23T09:55:31Z
source: tools/Reflector/Program.cs
---

# Phản chiếu kiểu `Storage`

## Thông tin chung
- Full Name: `Storage`
- Base Type: `Workable`
- Is Abstract: `False`
- Is Sealed: `False`
- Is Interface: `False`

## Fields
| Name | Type | Attributes |
| --- | --- | --- |
| `<allowUIItemRemoval>k__BackingField` | `System.Boolean` | `Private` |
| `allowClearable` | `System.Boolean` | `Public` |
| `allowItemRemoval` | `System.Boolean` | `Public` |
| `allowSettingOnlyFetchMarkedItems` | `System.Boolean` | `Public` |
| `alwaysShowProgressBar` | `System.Boolean` | `Public` |
| `attributeConverter` | `Klei.AI.AttributeConverter` | `Family` |
| `attributeConverterId` | `System.String` | `Family` |
| `attributeExperienceMultiplier` | `System.Single` | `Family` |
| `automatable` | `Automatable` | `Public` |
| `autoRegisterSimRender` | `System.Boolean` | `Family` |
| `capacityKg` | `System.Single` | `Public` |
| `capacityStatusItem` | `StatusItem` | `Private, Static` |
| `currentlyLit` | `System.Boolean` | `Public` |
| `defaultStoredItemModifers` | `System.Collections.Generic.List`1[[Storage+StoredItemModifier, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Private` |
| `deleted_objects` | `System.Collections.Generic.List`1[[UnityEngine.GameObject, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Private` |
| `doDiseaseTransfer` | `System.Boolean` | `Public` |
| `dropOffset` | `UnityEngine.Vector2` | `Public` |
| `dropOnLoad` | `System.Boolean` | `Public` |
| `endOfLife` | `System.Boolean` | `Private` |
| `faceTargetWhenWorking` | `System.Boolean` | `Public` |
| `fetchCategory` | `Storage+FetchCategory` | `Public` |
| `fxPrefix` | `Storage+FXPrefix` | `Public` |
| `gunTargetOffset` | `UnityEngine.Vector2` | `Public` |
| `ignoreSourcePriority` | `System.Boolean` | `Public` |
| `items` | `System.Collections.Generic.List`1[[UnityEngine.GameObject, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Public` |
| `laboratoryEfficiencyBonusStatusItemHandle` | `System.Guid` | `Family` |
| `laboratoryEfficiencyBonusTagRequired` | `Tag` | `Public` |
| `lightEfficiencyBonus` | `System.Boolean` | `Family` |
| `lightEfficiencyBonusStatusItemHandle` | `System.Guid` | `Family` |
| `maxKGPerItem` | `System.Single` | `Family` |
| `minimumAttributeMultiplier` | `System.Single` | `Family` |
| `multitoolContext` | `HashedString` | `Family` |
| `multitoolHitEffectTag` | `Tag` | `Family` |
| `numberOfUses` | `System.Int32` | `Family` |
| `offsetTracker` | `OffsetTracker` | `Family` |
| `OnCopySettingsDelegate` | `EventSystem+IntraObjectHandler`1[[Storage, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Private, Static, InitOnly` |
| `OnDeadTagAddedDelegate` | `EventSystem+IntraObjectHandler`1[[Storage, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Private, Static, InitOnly` |
| `onDestroyItemsDropped` | `System.Action`1[[System.Collections.Generic.List`1[[UnityEngine.GameObject, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=10.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]` | `Public` |
| `onlyFetchMarkedItems` | `System.Boolean` | `Private` |
| `onlyTransferFromLowerPriority` | `System.Boolean` | `Public` |
| `OnQueueDestroyObjectDelegate` | `EventSystem+IntraObjectHandler`1[[Storage, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Private, Static, InitOnly` |
| `OnReachableChangedDelegate` | `EventSystem+IntraObjectHandler`1[[Storage, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Private, Static, InitOnly` |
| `OnStorageChange` | `System.Action`1[[UnityEngine.GameObject, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Public` |
| `OnStorageIncreased` | `System.Action` | `Private` |
| `OnWorkableEventCB` | `System.Action`2[[Workable, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null],[Workable+WorkableEvent, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Public` |
| `overrideAnims` | `KAnimFile[]` | `Public` |
| `primaryElement` | `PrimaryElement` | `Family` |
| `prioritizable` | `Prioritizable` | `Public` |
| `progressBar` | `ProgressBar` | `Family` |
| `readyForSkillWorkStatusItem` | `StatusItem` | `Family` |
| `reportType` | `ReportManager+ReportType` | `Public` |
| `requiredSkillPerk` | `System.String` | `Public` |
| `requireMinionToWork` | `System.Boolean` | `Public` |
| `resetProgressOnStop` | `System.Boolean` | `Public` |
| `rotatable` | `Rotatable` | `Private` |
| `sendOnStoreOnSpawn` | `System.Boolean` | `Public` |
| `shouldSaveItems` | `System.Boolean` | `Private` |
| `shouldShowSkillPerkStatusItem` | `System.Boolean` | `Family` |
| `shouldTransferDiseaseWithWorker` | `System.Boolean` | `Family` |
| `showCapacityAsMainStatus` | `System.Boolean` | `Public` |
| `showCapacityStatusItem` | `System.Boolean` | `Public` |
| `showDescriptor` | `System.Boolean` | `Public` |
| `showInUI` | `System.Boolean` | `Public` |
| `showProgressBar` | `System.Boolean` | `Family` |
| `showSideScreenTitleBar` | `System.Boolean` | `Public` |
| `showUnreachableStatus` | `System.Boolean` | `Public` |
| `simRenderLoadBalance` | `System.Boolean` | `Family` |
| `skillExperienceMultiplier` | `System.Single` | `Family` |
| `skillExperienceSkillGroup` | `System.String` | `Family` |
| `skillsUpdateHandle` | `System.Int32` | `Family` |
| `StandardFabricatorStorage` | `System.Collections.Generic.List`1[[Storage+StoredItemModifier, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Public, Static, InitOnly` |
| `StandardInsulatedStorage` | `System.Collections.Generic.List`1[[Storage+StoredItemModifier, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Public, Static, InitOnly` |
| `StandardSealedStorage` | `System.Collections.Generic.List`1[[Storage+StoredItemModifier, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Public, Static, InitOnly` |
| `storageFilters` | `System.Collections.Generic.List`1[[Tag, Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Public` |
| `storageFullMargin` | `System.Single` | `Public` |
| `storageFXOffset` | `UnityEngine.Vector3` | `Public` |
| `storageID` | `Tag` | `Public` |
| `storageNetworkID` | `System.Int32` | `Public` |
| `storageWorkTime` | `System.Single` | `Public` |
| `StoredItemModifierHandlers` | `System.Collections.Generic.List`1[[Storage+StoredItemModifierInfo, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Private, Static, InitOnly` |
| `storeDropsFromButcherables` | `System.Boolean` | `Public` |
| `surpressWorkerForceSync` | `System.Boolean` | `Public` |
| `synchronizeAnims` | `System.Boolean` | `Public` |
| `trackUses` | `System.Boolean` | `Public` |
| `triggerWorkReactions` | `System.Boolean` | `Public` |
| `useGunForDelivery` | `System.Boolean` | `Public` |
| `useWideOffsets` | `System.Boolean` | `Public` |
| `workAnimPlayMode` | `KAnim+PlayMode` | `Public` |
| `workAnims` | `HashedString[]` | `Public` |
| `workerStatusItem` | `StatusItem` | `Family` |
| `workingPstComplete` | `HashedString[]` | `Public` |
| `workingPstFailed` | `HashedString[]` | `Public` |
| `workingStatusItem` | `StatusItem` | `Family` |
| `workLayer` | `Grid+SceneLayer` | `Public` |
| `workStatusItemHandle` | `System.Guid` | `Family` |
| `workTime` | `System.Single` | `Public` |
| `workTimeRemaining` | `System.Single` | `Family` |

## Properties
| Name | Type | Accessors |
| --- | --- | --- |
| `allowUIItemRemoval` | `System.Boolean` | `get; set;` |
| `Count` | `System.Int32` | `get;` |
| `destroyCancellationToken` | `System.Threading.CancellationToken` | `get;` |
| `enabled` | `System.Boolean` | `get; set;` |
| `gameObject` | `UnityEngine.GameObject` | `get;` |
| `hideFlags` | `UnityEngine.HideFlags` | `get; set;` |
| `isActiveAndEnabled` | `System.Boolean` | `get;` |
| `isNull` | `System.Boolean` | `get;` |
| `isSpawned` | `System.Boolean` | `get;` |
| `Item` | `UnityEngine.GameObject` | `get;` |
| `masterPriority` | `PrioritySetting` | `get;` |
| `name` | `System.String` | `get; set;` |
| `preferUnreservedCell` | `System.Boolean` | `get; set;` |
| `ShouldOnlyTransferFromLowerPriority` | `System.Boolean` | `get;` |
| `ShouldSaveItems` | `System.Boolean` | `get; set;` |
| `tag` | `System.String` | `get; set;` |
| `transform` | `UnityEngine.Transform` | `get;` |
| `useGUILayout` | `System.Boolean` | `get; set;` |
| `worker` | `WorkerBase` | `get; set;` |
| `WorkTimeRemaining` | `System.Single` | `get; set;` |

## Methods
| Signature | Return Type | Attributes |
| --- | --- | --- |
| `AddElement(SimHashes element, Single mass, Single temperature, Byte disease_idx, Int32 disease_count, Boolean keep_zero_mass, Boolean do_disease_transfer)` | `PrimaryElement` | `Public, HideBySig` |
| `AddGasChunk(SimHashes element, Single mass, Single temperature, Byte disease_idx, Int32 disease_count, Boolean keep_zero_mass, Boolean do_disease_transfer)` | `PrimaryElement` | `Public, HideBySig` |
| `AddLiquid(SimHashes element, Single mass, Single temperature, Byte disease_idx, Int32 disease_count, Boolean keep_zero_mass, Boolean do_disease_transfer)` | `PrimaryElement` | `Public, HideBySig` |
| `AddOre(SimHashes element, Single mass, Single temperature, Byte disease_idx, Int32 disease_count, Boolean keep_zero_mass, Boolean do_disease_transfer)` | `PrimaryElement` | `Public, HideBySig` |
| `AddToPrimaryElement(SimHashes element, Single additional_mass, Single temperature)` | `PrimaryElement` | `Public, HideBySig` |
| `ApplyStoredItemModifiers(GameObject go, Boolean is_stored, Boolean is_initializing)` | `System.Void` | `Private, HideBySig` |
| `Awake()` | `System.Void` | `Public, HideBySig` |
| `BoxingTrigger(Int32 hash, Boolean data)` | `System.Void` | `Public, HideBySig` |
| `BoxingTrigger(Int32 hash, T data)` | `System.Void` | `Public, HideBySig` |
| `BroadcastMessage(String methodName, Object parameter, SendMessageOptions options)` | `System.Void` | `Public, HideBySig` |
| `BroadcastMessage(String methodName, Object parameter)` | `System.Void` | `Public, HideBySig` |
| `BroadcastMessage(String methodName)` | `System.Void` | `Public, HideBySig` |
| `BroadcastMessage(String methodName, SendMessageOptions options)` | `System.Void` | `Public, HideBySig` |
| `CancelInvoke()` | `System.Void` | `Public, HideBySig` |
| `CancelInvoke(String methodName)` | `System.Void` | `Public, HideBySig` |
| `Capacity()` | `System.Single` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `ClearItems()` | `System.Void` | `Private, HideBySig` |
| `CompareTag(String tag)` | `System.Boolean` | `Public, HideBySig` |
| `CompleteWork(WorkerBase worker)` | `System.Void` | `Public, HideBySig` |
| `ConfigureMultitoolContext(HashedString context, Tag hitEffectTag)` | `System.Void` | `Public, HideBySig` |
| `ConsumeAllIgnoringDisease()` | `System.Void` | `Public, HideBySig` |
| `ConsumeAllIgnoringDisease(Tag tag)` | `System.Void` | `Public, HideBySig` |
| `ConsumeAndGetDisease(Tag tag, Single amount, Single& amount_consumed, DiseaseInfo& disease_info, Single& aggregate_temperature)` | `System.Void` | `Public, HideBySig` |
| `ConsumeAndGetDisease(Tag tag, Single amount, Single& amount_consumed, DiseaseInfo& disease_info, Single& aggregate_temperature, SimHashes& mostRelevantItemElement)` | `System.Void` | `Public, HideBySig` |
| `ConsumeAndGetDisease(Ingredient ingredient, DiseaseInfo& disease_info, Single& temperature)` | `System.Void` | `Public, HideBySig` |
| `ConsumeIgnoringDisease(Tag tag, Single amount)` | `System.Void` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `ConsumeIgnoringDisease(GameObject item_go)` | `System.Void` | `Public, HideBySig` |
| `Deserialize(IReader reader)` | `System.Void` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Drop(Tag t, List`1 obj_list)` | `System.Void` | `Public, HideBySig` |
| `Drop(Tag t)` | `System.Void` | `Public, HideBySig` |
| `Drop(GameObject go, Boolean do_disease_transfer)` | `UnityEngine.GameObject` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Drop(Int32 ID)` | `UnityEngine.GameObject` | `Public, HideBySig` |
| `DropAll(Vector3 position, Boolean vent_gas, Boolean dump_liquid, Vector3 offset, Boolean do_disease_transfer, List`1 collect_dropped_items)` | `System.Void` | `Public, HideBySig` |
| `DropAll(Boolean vent_gas, Boolean dump_liquid, Vector3 offset, Boolean do_disease_transfer, List`1 collect_dropped_items)` | `System.Void` | `Public, HideBySig` |
| `DropHasTags(Tag[] tag)` | `UnityEngine.GameObject[]` | `Public, HideBySig` |
| `DropSome(Tag tag, Single amount, Boolean ventGas, Boolean dumpLiquid, Vector3 offset, Boolean doDiseaseTransfer, Boolean showInWorldNotification)` | `System.Boolean` | `Public, HideBySig` |
| `DropUnlessHasTag(Tag tag)` | `UnityEngine.GameObject[]` | `Public, HideBySig` |
| `DropUnlessMatching(FetchChore chore)` | `System.Void` | `Public, HideBySig` |
| `Equals(Object other)` | `System.Boolean` | `Public, Virtual, HideBySig` |
| `ExactMassStored()` | `System.Single` | `Public, HideBySig` |
| `Finalize()` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `Find(Tag tag, List`1 result)` | `System.Collections.Generic.List`1[[UnityEngine.GameObject, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Public, HideBySig` |
| `Find(Int32 ID)` | `UnityEngine.GameObject` | `Public, HideBySig` |
| `FindFirst(Tag tag)` | `UnityEngine.GameObject` | `Public, HideBySig` |
| `FindFirstWithMass(Tag tag, Single mass)` | `PrimaryElement` | `Public, HideBySig` |
| `FindOrAdd()` | `T` | `Public, HideBySig` |
| `FindOrAdd(T& c)` | `System.Void` | `Public, HideBySig` |
| `FindPrimaryElement(SimHashes element)` | `PrimaryElement` | `Public, HideBySig` |
| `Flatten(Tag tag_to_combine)` | `System.Void` | `Private, HideBySig` |
| `ForceStore(Tag tag, Single amount)` | `System.Boolean` | `Public, HideBySig` |
| `GetAllIDsInStorage()` | `System.Collections.Generic.HashSet`1[[Tag, Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Public, HideBySig` |
| `GetAmountAvailable(Tag tag)` | `System.Single` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `GetAmountAvailable(Tag tag, Tag[] forbiddenTags)` | `System.Single` | `Public, HideBySig` |
| `GetAnim(WorkerBase worker)` | `Workable+AnimInfo` | `Public, Virtual, HideBySig` |
| `GetAnimController()` | `KAnimControllerBase` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `GetAttributeExperienceMultiplier()` | `System.Single` | `Public, HideBySig` |
| `GetBuildingFacade()` | `BuildingFacade` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `GetCell()` | `System.Int32` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `GetComponent(Type type)` | `UnityEngine.Component` | `Public, HideBySig` |
| `GetComponent()` | `T` | `Public, HideBySig` |
| `GetComponent(String type)` | `UnityEngine.Component` | `Public, HideBySig` |
| `GetComponentFastPath(Type type, IntPtr oneFurtherThanResultValue)` | `System.Void` | `Assembly, HideBySig` |
| `GetComponentInChildren(Type t, Boolean includeInactive)` | `UnityEngine.Component` | `Public, HideBySig` |
| `GetComponentInChildren(Type t)` | `UnityEngine.Component` | `Public, HideBySig` |
| `GetComponentInChildren(Boolean includeInactive)` | `T` | `Public, HideBySig` |
| `GetComponentInChildren()` | `T` | `Public, HideBySig` |
| `GetComponentIndex()` | `System.Int32` | `Public, HideBySig` |
| `GetComponentInParent(Type t, Boolean includeInactive)` | `UnityEngine.Component` | `Public, HideBySig` |
| `GetComponentInParent(Type t)` | `UnityEngine.Component` | `Public, HideBySig` |
| `GetComponentInParent(Boolean includeInactive)` | `T` | `Public, HideBySig` |
| `GetComponentInParent()` | `T` | `Public, HideBySig` |
| `GetComponents(Type type)` | `UnityEngine.Component[]` | `Public, HideBySig` |
| `GetComponents(Type type, List`1 results)` | `System.Void` | `Public, HideBySig` |
| `GetComponents(List`1 results)` | `System.Void` | `Public, HideBySig` |
| `GetComponents()` | `T[]` | `Public, HideBySig` |
| `GetComponentsInChildren(Type t, Boolean includeInactive)` | `UnityEngine.Component[]` | `Public, HideBySig` |
| `GetComponentsInChildren(Type t)` | `UnityEngine.Component[]` | `Public, HideBySig` |
| `GetComponentsInChildren(Boolean includeInactive)` | `T[]` | `Public, HideBySig` |
| `GetComponentsInChildren(Boolean includeInactive, List`1 result)` | `System.Void` | `Public, HideBySig` |
| `GetComponentsInChildren()` | `T[]` | `Public, HideBySig` |
| `GetComponentsInChildren(List`1 results)` | `System.Void` | `Public, HideBySig` |
| `GetComponentsInParent(Type t, Boolean includeInactive)` | `UnityEngine.Component[]` | `Public, HideBySig` |
| `GetComponentsInParent(Type t)` | `UnityEngine.Component[]` | `Public, HideBySig` |
| `GetComponentsInParent(Boolean includeInactive)` | `T[]` | `Public, HideBySig` |
| `GetComponentsInParent(Boolean includeInactive, List`1 results)` | `System.Void` | `Public, HideBySig` |
| `GetComponentsInParent()` | `T[]` | `Public, HideBySig` |
| `GetConversationTopic()` | `System.String` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `GetDescriptors(GameObject go)` | `System.Collections.Generic.List`1[[Descriptor, Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Public, Virtual, HideBySig` |
| `GetEfficiencyMultiplier(WorkerBase worker)` | `System.Single` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `GetFacingTarget()` | `UnityEngine.Vector3` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `GetHashCode()` | `System.Int32` | `Public, Virtual, HideBySig` |
| `GetInstanceID()` | `System.Int32` | `Public, HideBySig` |
| `GetItems()` | `System.Collections.Generic.List`1[[UnityEngine.GameObject, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `GetMassAvailable(Tag tag)` | `System.Single` | `Public, HideBySig` |
| `GetMassAvailable(SimHashes element)` | `System.Single` | `Public, HideBySig` |
| `GetNavigationCost(Navigator navigator, Int32 cell)` | `System.Int32` | `Public, HideBySig` |
| `GetNavigationCost(Navigator navigator)` | `System.Int32` | `Public, HideBySig` |
| `GetOffsets(Int32 cell)` | `CellOffset[]` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `GetOffsets()` | `CellOffset[]` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `GetOnlyFetchMarkedItems()` | `System.Boolean` | `Public, HideBySig` |
| `GetPercentComplete()` | `System.Single` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `GetReportType()` | `ReportManager+ReportType` | `Public, HideBySig` |
| `GetScriptClassName()` | `System.String` | `Assembly, HideBySig` |
| `GetSkillExperienceMultiplier()` | `System.Single` | `Public, HideBySig` |
| `GetSkillExperienceSkillGroup()` | `System.String` | `Public, HideBySig` |
| `GetTargetPoint()` | `UnityEngine.Vector3` | `Public, Virtual, HideBySig` |
| `GetType()` | `System.Type` | `Public, HideBySig` |
| `GetUnitsAvailable(Tag tag)` | `System.Single` | `Public, HideBySig` |
| `GetWorkAnimPlayMode()` | `KAnim+PlayMode` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `GetWorkAnims(WorkerBase worker)` | `HashedString[]` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `GetWorkAttribute()` | `Klei.AI.Attribute` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `GetWorker()` | `WorkerBase` | `Public, HideBySig` |
| `GetWorkerStatusItem()` | `StatusItem` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `GetWorkOffset()` | `UnityEngine.Vector3` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `GetWorkPstAnims(WorkerBase worker, Boolean successfully_completed)` | `HashedString[]` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `GetWorkTime()` | `System.Single` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `Has(Tag tag)` | `System.Boolean` | `Public, HideBySig` |
| `InitializeComponent()` | `System.Void` | `Public, HideBySig` |
| `InstantlyFinish(WorkerBase worker)` | `System.Boolean` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `Invoke(String methodName, Single time)` | `System.Void` | `Public, HideBySig` |
| `InvokeRepeating(String methodName, Single time, Single repeatRate)` | `System.Void` | `Public, HideBySig` |
| `IsEmpty()` | `System.Boolean` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `IsEndOfLife()` | `System.Boolean` | `Public, HideBySig` |
| `IsFull()` | `System.Boolean` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `IsInitialized()` | `System.Boolean` | `Public, HideBySig` |
| `IsInvoking()` | `System.Boolean` | `Public, HideBySig` |
| `IsInvoking(String methodName)` | `System.Boolean` | `Public, HideBySig` |
| `IStateMachineTarget.get_gameObject()` | `UnityEngine.GameObject` | `Private, Final, Virtual, HideBySig, VtableLayoutMask` |
| `IStateMachineTarget.get_name()` | `System.String` | `Private, Final, Virtual, HideBySig, VtableLayoutMask` |
| `IStateMachineTarget.GetComponent()` | `ComponentType` | `Private, Final, Virtual, HideBySig, VtableLayoutMask` |
| `MakeItemInvisible(GameObject go, Boolean is_stored, Boolean is_initializing)` | `System.Void` | `Public, Static, HideBySig` |
| `MakeItemPreserved(GameObject go, Boolean is_stored, Boolean is_initializing)` | `System.Void` | `Public, Static, HideBySig` |
| `MakeItemSealed(GameObject go, Boolean is_stored, Boolean is_initializing)` | `System.Void` | `Public, Static, HideBySig` |
| `MakeItemTemperatureInsulated(GameObject go, Boolean is_stored, Boolean is_initializing)` | `System.Void` | `Public, Static, HideBySig` |
| `MakeWorldActive(GameObject go)` | `System.Void` | `Private, HideBySig` |
| `MarkDirty()` | `System.Void` | `Assembly, HideBySig` |
| `MassStored()` | `System.Single` | `Public, HideBySig` |
| `MemberwiseClone()` | `System.Object` | `FamORAssem, HideBySig` |
| `OnAbortWork(WorkerBase worker)` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `OnCleanUp()` | `System.Void` | `Family, Virtual, HideBySig` |
| `OnCmpDisable()` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `OnCmpEnable()` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `OnCompleteWork(WorkerBase worker)` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `OnCopySettings(Object data)` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `OnDeath(Object data)` | `System.Void` | `Private, HideBySig` |
| `OnDeserialized()` | `System.Void` | `Private, HideBySig` |
| `OnDestroy()` | `System.Void` | `Public, HideBySig` |
| `OnDisable()` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `OnForcedCleanUp()` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `OnLoadLevel()` | `System.Void` | `Family, Virtual, HideBySig` |
| `OnPendingCompleteWork(WorkerBase worker)` | `System.Void` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `OnPrefabInit()` | `System.Void` | `Family, Virtual, HideBySig` |
| `OnPriorityChanged(PrioritySetting priority)` | `System.Void` | `Private, HideBySig` |
| `OnQueueDestroyObject(Object data)` | `System.Void` | `Private, HideBySig` |
| `OnReachableChanged(Object data)` | `System.Void` | `Private, HideBySig` |
| `OnSpawn()` | `System.Void` | `Family, Virtual, HideBySig` |
| `OnStartWork(WorkerBase worker)` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `OnStopWork(WorkerBase worker)` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `OnWorkTick(WorkerBase worker, Single dt)` | `System.Boolean` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `PlaySound3D(String asset)` | `System.Void` | `Public, HideBySig` |
| `PosMax()` | `UnityEngine.Vector2` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `PosMin()` | `UnityEngine.Vector2` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `RefreshReachability()` | `System.Void` | `Public, HideBySig` |
| `RemainingCapacity()` | `System.Single` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Remove(GameObject go, Boolean do_disease_transfer)` | `System.Void` | `Public, HideBySig` |
| `RenotifyAll()` | `System.Void` | `Public, HideBySig` |
| `Require()` | `T` | `Public, HideBySig` |
| `SendMessage(String methodName, Object value)` | `System.Void` | `Public, HideBySig` |
| `SendMessage(String methodName)` | `System.Void` | `Public, HideBySig` |
| `SendMessage(String methodName, Object value, SendMessageOptions options)` | `System.Void` | `Public, HideBySig` |
| `SendMessage(String methodName, SendMessageOptions options)` | `System.Void` | `Public, HideBySig` |
| `SendMessageUpwards(String methodName, Object value, SendMessageOptions options)` | `System.Void` | `Public, HideBySig` |
| `SendMessageUpwards(String methodName, Object value)` | `System.Void` | `Public, HideBySig` |
| `SendMessageUpwards(String methodName)` | `System.Void` | `Public, HideBySig` |
| `SendMessageUpwards(String methodName, SendMessageOptions options)` | `System.Void` | `Public, HideBySig` |
| `Serialize(BinaryWriter writer)` | `System.Void` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `SetContentsDeleteOffGrid(Boolean delete_off_grid)` | `System.Void` | `Public, HideBySig` |
| `SetDefaultStoredItemModifiers(List`1 modifiers)` | `System.Void` | `Public, HideBySig` |
| `SetOffsets(CellOffset[] offsets)` | `System.Void` | `Public, HideBySig` |
| `SetOffsetTable(CellOffset[][] offset_table)` | `System.Void` | `Public, HideBySig` |
| `SetOnlyFetchMarkedItems(Boolean is_set)` | `System.Void` | `Public, HideBySig` |
| `SetReportType(ReportType report_type)` | `System.Void` | `Public, HideBySig` |
| `SetShouldShowSkillPerkStatusItem(Boolean shouldItBeShown)` | `System.Void` | `Public, HideBySig` |
| `SetupStorageStatusItems()` | `System.Void` | `Private, HideBySig` |
| `SetWorkerStatusItem(StatusItem item)` | `System.Void` | `Public, HideBySig` |
| `SetWorkTime(Single work_time)` | `System.Void` | `Public, HideBySig` |
| `ShouldFaceTargetWhenWorking()` | `System.Boolean` | `Public, HideBySig` |
| `ShouldSaveItem(GameObject go)` | `System.Boolean` | `Private, HideBySig` |
| `ShouldShowInUI()` | `System.Boolean` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `ShowProgressBar(Boolean show)` | `System.Void` | `Public, HideBySig` |
| `Spawn()` | `System.Void` | `Public, HideBySig` |
| `Start()` | `System.Void` | `Public, HideBySig` |
| `StartCoroutine(String methodName)` | `UnityEngine.Coroutine` | `Public, HideBySig` |
| `StartCoroutine(String methodName, Object value)` | `UnityEngine.Coroutine` | `Public, HideBySig` |
| `StartCoroutine(IEnumerator routine)` | `UnityEngine.Coroutine` | `Public, HideBySig` |
| `StartCoroutine_Auto(IEnumerator routine)` | `UnityEngine.Coroutine` | `Public, HideBySig` |
| `StartWork(WorkerBase worker_to_start)` | `System.Void` | `Public, HideBySig` |
| `StopAllCoroutines()` | `System.Void` | `Public, HideBySig` |
| `StopCoroutine(IEnumerator routine)` | `System.Void` | `Public, HideBySig` |
| `StopCoroutine(Coroutine routine)` | `System.Void` | `Public, HideBySig` |
| `StopCoroutine(String methodName)` | `System.Void` | `Public, HideBySig` |
| `StopWork(WorkerBase workerToStop, Boolean aborted)` | `System.Void` | `Public, HideBySig` |
| `Store(GameObject go, Boolean hide_popups, Boolean block_events, Boolean do_disease_transfer, Boolean is_deserializing)` | `UnityEngine.GameObject` | `Public, HideBySig` |
| `Subscribe(Int32 hash, Action`1 handler)` | `System.Int32` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Subscribe(Int32 hash, Action`2 handler, Object handlerData)` | `System.Int32` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Subscribe(GameObject target, Int32 hash, Action`1 handler)` | `System.Int32` | `Public, HideBySig` |
| `Subscribe(GameObject target, Int32 hash, Action`2 handler, Object handlerData)` | `System.Int32` | `Public, HideBySig` |
| `Subscribe(Int32 hash, IntraObjectHandler`1 handler)` | `System.Int32` | `Public, HideBySig` |
| `ToString()` | `System.String` | `Public, Virtual, HideBySig` |
| `Transfer(Storage target, Boolean block_events, Boolean hide_popups)` | `System.Void` | `Public, HideBySig` |
| `Transfer(Storage dest_storage, Tag tag, Single amount, Boolean block_events, Boolean hide_popups)` | `System.Single` | `Public, HideBySig` |
| `Transfer(GameObject go, Storage target, Boolean block_events, Boolean hide_popups)` | `System.Boolean` | `Public, HideBySig` |
| `TransferDiseaseWithObject(GameObject obj)` | `System.Void` | `Private, HideBySig` |
| `TransferMass(Storage dest_storage, Tag tag, Single amount, Boolean flatten, Boolean block_events, Boolean hide_popups)` | `System.Boolean` | `Public, HideBySig` |
| `TransferUnitMass(Storage dest_storage, Tag tag, Single unitAmount, Boolean flatten, Boolean block_events, Boolean hide_popups)` | `System.Void` | `Public, HideBySig` |
| `Trigger(Int32 hash, Object data)` | `System.Void` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Trigger(Int32 hash, T data)` | `System.Void` | `Public, HideBySig` |
| `TryGetComponent(Type type, Component& component)` | `System.Boolean` | `Public, HideBySig` |
| `TryGetComponent(T& component)` | `System.Boolean` | `Public, HideBySig` |
| `UnitsStored()` | `System.Single` | `Public, HideBySig` |
| `Unsubscribe(Int32 hash, Action`1 handler)` | `System.Void` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Unsubscribe(Int32 id)` | `System.Void` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Unsubscribe(Int32& id)` | `System.Void` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Unsubscribe(GameObject target, Int32 hash, Action`1 handler)` | `System.Void` | `Public, HideBySig` |
| `Unsubscribe(GameObject target, Int32 id)` | `System.Void` | `Public, HideBySig` |
| `Unsubscribe(GameObject target, Int32& id)` | `System.Void` | `Public, HideBySig` |
| `Unsubscribe(Int32 hash, IntraObjectHandler`1 handler, Boolean suppressWarnings)` | `System.Void` | `Public, HideBySig` |
| `UpdateFetchCategory()` | `System.Void` | `Private, HideBySig` |
| `UpdateStatusItem(Object data)` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `UpdateStoredItemCachedCells()` | `System.Void` | `Public, HideBySig` |
| `ValidateOffsets(Int32 cell)` | `System.Boolean` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `WorkTick(WorkerBase worker, Single dt)` | `System.Boolean` | `Public, HideBySig` |

