---
id: solidtransferarm-reflection
type: research-output
status: done
created: 2026-06-23T09:55:31Z
source: tools/Reflector/Program.cs
---

# Phản chiếu kiểu `SolidTransferArm`

## Thông tin chung
- Full Name: `SolidTransferArm`
- Base Type: `StateMachineComponent`1[[SolidTransferArm+SMInstance, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]`
- Is Abstract: `False`
- Is Sealed: `False`
- Is Interface: `False`

## Fields
| Name | Type | Attributes |
| --- | --- | --- |
| `arm_anim` | `SolidTransferArm+ArmAnim` | `Private` |
| `arm_anim_ctrl` | `KBatchedAnimController` | `Private` |
| `arm_go` | `UnityEngine.GameObject` | `Private` |
| `arm_rot` | `System.Single` | `Private` |
| `AsyncUpdateVisitor` | `System.Func`3[[System.Object, System.Private.CoreLib, Version=10.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[SolidTransferArm, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null],[Util+IterationInstruction, Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Private, Static` |
| `autoRegisterSimRender` | `System.Boolean` | `Family` |
| `choreConsumer` | `ChoreConsumer` | `Private` |
| `choreDriver` | `ChoreDriver` | `Private` |
| `gameCell` | `System.Int32` | `Private` |
| `HASH_ROTATION` | `HashedString` | `Private, Static` |
| `kPrefabID` | `KPrefabID` | `Private` |
| `link` | `KAnimLink` | `Private` |
| `looping_sounds` | `LoopingSounds` | `Private` |
| `max_carry_weight` | `System.Single` | `Private` |
| `OnEndChoreDelegate` | `EventSystem+IntraObjectHandler`1[[SolidTransferArm, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Private, Static, InitOnly` |
| `OnOperationalChangedDelegate` | `EventSystem+IntraObjectHandler`1[[SolidTransferArm, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Private, Static, InitOnly` |
| `operational` | `Operational` | `Private` |
| `pickupables` | `System.Collections.Generic.List`1[[Pickupable, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]` | `Private` |
| `pickupRange` | `System.Int32` | `Public` |
| `reachableCells` | `System.Collections.Generic.HashSet`1[[System.Int32, System.Private.CoreLib, Version=10.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]` | `Private` |
| `rotatable` | `Rotatable` | `Private` |
| `rotateSound` | `FMODUnity.EventReference` | `Private` |
| `rotateSoundName` | `System.String` | `Private` |
| `rotateSoundPlaying` | `System.Boolean` | `Private` |
| `rotation_complete` | `System.Boolean` | `Private` |
| `simRenderLoadBalance` | `System.Boolean` | `Family` |
| `stateMachineController` | `StateMachineController` | `Family` |
| `storage` | `Storage` | `Private` |
| `turn_rate` | `System.Single` | `Private` |
| `worker` | `StandardWorker` | `Private` |

## Properties
| Name | Type | Accessors |
| --- | --- | --- |
| `destroyCancellationToken` | `System.Threading.CancellationToken` | `get;` |
| `enabled` | `System.Boolean` | `get; set;` |
| `gameObject` | `UnityEngine.GameObject` | `get;` |
| `hideFlags` | `UnityEngine.HideFlags` | `get; set;` |
| `isActiveAndEnabled` | `System.Boolean` | `get;` |
| `isNull` | `System.Boolean` | `get;` |
| `isSpawned` | `System.Boolean` | `get;` |
| `name` | `System.String` | `get; set;` |
| `smi` | `SolidTransferArm+SMInstance` | `get;` |
| `tag` | `System.String` | `get; set;` |
| `transform` | `UnityEngine.Transform` | `get;` |
| `useGUILayout` | `System.Boolean` | `get; set;` |

## Methods
| Signature | Return Type | Attributes |
| --- | --- | --- |
| `AsyncUpdate()` | `System.Boolean` | `Private, HideBySig` |
| `Awake()` | `System.Void` | `Public, HideBySig` |
| `BatchUpdate(List`1 solid_transfer_arms, Single time_delta)` | `System.Void` | `Public, Static, HideBySig` |
| `BoxingTrigger(Int32 hash, Boolean data)` | `System.Void` | `Public, HideBySig` |
| `BoxingTrigger(Int32 hash, T data)` | `System.Void` | `Public, HideBySig` |
| `BroadcastMessage(String methodName, Object parameter, SendMessageOptions options)` | `System.Void` | `Public, HideBySig` |
| `BroadcastMessage(String methodName, Object parameter)` | `System.Void` | `Public, HideBySig` |
| `BroadcastMessage(String methodName)` | `System.Void` | `Public, HideBySig` |
| `BroadcastMessage(String methodName, SendMessageOptions options)` | `System.Void` | `Public, HideBySig` |
| `CancelInvoke()` | `System.Void` | `Public, HideBySig` |
| `CancelInvoke(String methodName)` | `System.Void` | `Public, HideBySig` |
| `CompareTag(String tag)` | `System.Boolean` | `Public, HideBySig` |
| `DropLeftovers()` | `System.Void` | `Private, HideBySig` |
| `Equals(Object other)` | `System.Boolean` | `Public, Virtual, HideBySig` |
| `Finalize()` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `FindFetchTarget(Storage destination, FetchChore chore)` | `Pickupable` | `Public, HideBySig` |
| `FindOrAdd()` | `T` | `Public, HideBySig` |
| `FindOrAdd(T& c)` | `System.Void` | `Public, HideBySig` |
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
| `GetHashCode()` | `System.Int32` | `Public, Virtual, HideBySig` |
| `GetInstanceID()` | `System.Int32` | `Public, HideBySig` |
| `GetScriptClassName()` | `System.String` | `Assembly, HideBySig` |
| `GetSMI()` | `StateMachine+Instance` | `Public, Virtual, HideBySig` |
| `GetType()` | `System.Type` | `Public, HideBySig` |
| `InitializeComponent()` | `System.Void` | `Public, HideBySig` |
| `Invoke(String methodName, Single time)` | `System.Void` | `Public, HideBySig` |
| `InvokeRepeating(String methodName, Single time, Single repeatRate)` | `System.Void` | `Public, HideBySig` |
| `IsCellReachable(Int32 cell)` | `System.Boolean` | `Public, HideBySig` |
| `IsInitialized()` | `System.Boolean` | `Public, HideBySig` |
| `IsInvoking()` | `System.Boolean` | `Public, HideBySig` |
| `IsInvoking(String methodName)` | `System.Boolean` | `Public, HideBySig` |
| `IsPickupableRelevantToMyInterests(KPrefabID prefabID, Int32 storage_cell)` | `System.Boolean` | `Private, HideBySig` |
| `IStateMachineTarget.get_gameObject()` | `UnityEngine.GameObject` | `Private, Final, Virtual, HideBySig, VtableLayoutMask` |
| `IStateMachineTarget.get_name()` | `System.String` | `Private, Final, Virtual, HideBySig, VtableLayoutMask` |
| `IStateMachineTarget.GetComponent()` | `ComponentType` | `Private, Final, Virtual, HideBySig, VtableLayoutMask` |
| `MarkDirty()` | `System.Void` | `Assembly, HideBySig` |
| `MemberwiseClone()` | `System.Object` | `FamORAssem, HideBySig` |
| `OnCleanUp()` | `System.Void` | `Family, Virtual, HideBySig` |
| `OnCmpDisable()` | `System.Void` | `Family, Virtual, HideBySig` |
| `OnCmpEnable()` | `System.Void` | `Family, Virtual, HideBySig` |
| `OnDestroy()` | `System.Void` | `Public, HideBySig` |
| `OnDisable()` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `OnEndChore(Object data)` | `System.Void` | `Private, HideBySig` |
| `OnForcedCleanUp()` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `OnLoadLevel()` | `System.Void` | `Family, Virtual, HideBySig, VtableLayoutMask` |
| `OnOperationalChanged(Object data)` | `System.Void` | `Private, HideBySig` |
| `OnPrefabInit()` | `System.Void` | `Family, Virtual, HideBySig` |
| `OnSpawn()` | `System.Void` | `Family, Virtual, HideBySig` |
| `PlaySound3D(String asset)` | `System.Void` | `Public, HideBySig` |
| `PosMax()` | `UnityEngine.Vector2` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `PosMin()` | `UnityEngine.Vector2` | `Public, Virtual, HideBySig, VtableLayoutMask` |
| `RenderEveryTick(Single dt)` | `System.Void` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Require()` | `T` | `Public, HideBySig` |
| `RotateArm(Vector3 target_dir, Boolean warp, Single dt)` | `System.Void` | `Private, HideBySig` |
| `SendMessage(String methodName, Object value)` | `System.Void` | `Public, HideBySig` |
| `SendMessage(String methodName)` | `System.Void` | `Public, HideBySig` |
| `SendMessage(String methodName, Object value, SendMessageOptions options)` | `System.Void` | `Public, HideBySig` |
| `SendMessage(String methodName, SendMessageOptions options)` | `System.Void` | `Public, HideBySig` |
| `SendMessageUpwards(String methodName, Object value, SendMessageOptions options)` | `System.Void` | `Public, HideBySig` |
| `SendMessageUpwards(String methodName, Object value)` | `System.Void` | `Public, HideBySig` |
| `SendMessageUpwards(String methodName)` | `System.Void` | `Public, HideBySig` |
| `SendMessageUpwards(String methodName, SendMessageOptions options)` | `System.Void` | `Public, HideBySig` |
| `SetArmAnim(ArmAnim new_anim)` | `System.Void` | `Private, HideBySig` |
| `SetArmRotation(Single rot)` | `System.Void` | `Private, HideBySig` |
| `SetRotateSoundParameter(Single arm_rot)` | `System.Void` | `Private, HideBySig` |
| `Sim()` | `System.Void` | `Private, HideBySig` |
| `Sim1000ms(Single dt)` | `System.Void` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Spawn()` | `System.Void` | `Public, HideBySig` |
| `Start()` | `System.Void` | `Public, HideBySig` |
| `StartCoroutine(String methodName)` | `UnityEngine.Coroutine` | `Public, HideBySig` |
| `StartCoroutine(String methodName, Object value)` | `UnityEngine.Coroutine` | `Public, HideBySig` |
| `StartCoroutine(IEnumerator routine)` | `UnityEngine.Coroutine` | `Public, HideBySig` |
| `StartCoroutine_Auto(IEnumerator routine)` | `UnityEngine.Coroutine` | `Public, HideBySig` |
| `StartRotateSound()` | `System.Void` | `Private, HideBySig` |
| `StopAllCoroutines()` | `System.Void` | `Public, HideBySig` |
| `StopCoroutine(IEnumerator routine)` | `System.Void` | `Public, HideBySig` |
| `StopCoroutine(Coroutine routine)` | `System.Void` | `Public, HideBySig` |
| `StopCoroutine(String methodName)` | `System.Void` | `Public, HideBySig` |
| `StopRotateSound()` | `System.Void` | `Private, HideBySig` |
| `Subscribe(Int32 hash, Action`1 handler)` | `System.Int32` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Subscribe(Int32 hash, Action`2 handler, Object handlerData)` | `System.Int32` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Subscribe(GameObject target, Int32 hash, Action`1 handler)` | `System.Int32` | `Public, HideBySig` |
| `Subscribe(GameObject target, Int32 hash, Action`2 handler, Object handlerData)` | `System.Int32` | `Public, HideBySig` |
| `Subscribe(Int32 hash, IntraObjectHandler`1 handler)` | `System.Int32` | `Public, HideBySig` |
| `ToString()` | `System.String` | `Public, Virtual, HideBySig` |
| `Trigger(Int32 hash, Object data)` | `System.Void` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Trigger(Int32 hash, T data)` | `System.Void` | `Public, HideBySig` |
| `TryGetComponent(Type type, Component& component)` | `System.Boolean` | `Public, HideBySig` |
| `TryGetComponent(T& component)` | `System.Boolean` | `Public, HideBySig` |
| `Unsubscribe(Int32 hash, Action`1 handler)` | `System.Void` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Unsubscribe(Int32 id)` | `System.Void` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Unsubscribe(Int32& id)` | `System.Void` | `Public, Final, Virtual, HideBySig, VtableLayoutMask` |
| `Unsubscribe(GameObject target, Int32 hash, Action`1 handler)` | `System.Void` | `Public, HideBySig` |
| `Unsubscribe(GameObject target, Int32 id)` | `System.Void` | `Public, HideBySig` |
| `Unsubscribe(GameObject target, Int32& id)` | `System.Void` | `Public, HideBySig` |
| `Unsubscribe(Int32 hash, IntraObjectHandler`1 handler, Boolean suppressWarnings)` | `System.Void` | `Public, HideBySig` |
| `UpdateArmAnim()` | `System.Void` | `Private, HideBySig` |

