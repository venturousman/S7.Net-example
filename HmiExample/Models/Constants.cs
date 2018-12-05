using System;

namespace ProductionEquipmentControlSoftware
{
    public sealed class Constants
    {
        public static readonly string ApplicationName = "Production Equipment Control";
        public static readonly string ApplicationCommonErrorMessage = "Unhandled error has occurred in your application";

        public static readonly string MoldLife = "MoldLife"; // tuoi tho khuon
        public static readonly string MaxCycleTime = "MaxCycleTime"; // cycletime toi da

        public static readonly DateTime DefaultStartDate = new DateTime(2018, 10, 10, 0, 0, 0);
    }
}
