using static STRINGS.UI;

namespace MoveThisHere
{
    public class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class HAULINGPOINT
                {
                    public static LocString NAME = FormatAsLink("Hauling Point", HaulingPointConfig.Id);
                    public static LocString DESC = "Relocate selected items here, then deconstruct to drop them on the ground.";
                    public static LocString EFFECT = "A temporary designation to bring items to a specific place.";
                }
            }

            public class BUTTONS
            {
                public class HAULINGPOINT
                {
                    // Remove
                    public static LocString REMOVE = "Remove";
                    public static LocString REMOVE_TOOLTIP = "Remove this hauling point and drop all items here";

                    // Auto-Drop (Deconstruct)
                    public static LocString AUTO_DROP_ON = "Enable Auto-Deconstruct";
                    public static LocString AUTO_DROP_ON_TOOLTIP = "If enabled, automatically deconstruct hauling point and drop items when storage is full";
                    public static LocString AUTO_DROP_OFF = "Disable Auto-Deconstruct";
                    public static LocString AUTO_DROP_OFF_TOOLTIP = "Cancel automatic deconstructing when full";

                    // Auto-Eject
                    public static LocString AUTO_EJECT_ON = "Enable Auto-Eject";
                    public static LocString AUTO_EJECT_ON_TOOLTIP = "If enabled, automatically drop all items on the ground and keep hauling point when storage is full";
                    public static LocString AUTO_EJECT_OFF = "Disable Auto-Eject";
                    public static LocString AUTO_EJECT_OFF_TOOLTIP = "Cancel automatic ejecting when full";

                    // Manual Eject
                    public static LocString MANUAL_EJECT = "Eject Contents";
                    public static LocString MANUAL_EJECT_TOOLTIP = "Drop all stored items onto the ground immediately without deconstructing the Hauling Point";

                    // Auto-Spill
                    public static LocString AUTO_SPILL_ON = "Enable Auto-Spill";
                    public static LocString AUTO_SPILL_ON_TOOLTIP = "If enabled, automatically spill liquids and gasses instead of dropping bottles when hauling point is removed";
                    public static LocString AUTO_SPILL_OFF = "Disable Auto-Spill";
                    public static LocString AUTO_SPILL_OFF_TOOLTIP = "Drop bottles of liquids and gasses rather than spilling into the world";

                    // Allow Item Removal
                    public static LocString ALLOW_ITEM_REMOVAL_ON = "Allow Retrieve (Dupes/Sweepers)";
                    public static LocString ALLOW_ITEM_REMOVAL_ON_TOOLTIP = "Allow Duplicants and Auto-Sweepers to retrieve items from this hauling point. Warning: May cause transport loops if filter is active!";
                    public static LocString ALLOW_ITEM_REMOVAL_OFF = "Disallow Retrieve (Dupes/Sweepers)";
                    public static LocString ALLOW_ITEM_REMOVAL_OFF_TOOLTIP = "Lock all items inside this hauling point. Duplicants and Auto-Sweepers will not be able to retrieve them.";

                    // Auto-Bottle
                    public static LocString AUTO_BOTTLE_ON = "Enable Auto-Bottle";
                    public static LocString AUTO_BOTTLE_ON_TOOLTIP = "If enabled, Duplicants will bottle liquids and gases to deliver to this hauling point";
                    public static LocString AUTO_BOTTLE_OFF = "Disable Auto-Bottle";
                    public static LocString AUTO_BOTTLE_OFF_TOOLTIP = "If disabled, Duplicants will no longer bottle liquids and gases to deliver to this hauling point";
                    
                    // Details Screen Status Strings
                    public static LocString STATUS_AUTO_EJECT = "Auto-Eject on full";
                    public static LocString STATUS_AUTO_DECONSTRUCT = "Auto-Deconstruct on full";
                    public static LocString STATUS_RETRIEVAL = "Retrieval by Dupes/Sweepers";
                    public static LocString ENABLED = "Enabled";
                    public static LocString DISABLED = "Disabled";
                    public static LocString ALLOWED = "Allowed";
                    public static LocString DISALLOWED = "Disallowed";
                }
            }
        }
    }
}
