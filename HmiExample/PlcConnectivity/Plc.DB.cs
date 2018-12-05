using System;

namespace ProductionEquipmentControlSoftware.PlcConnectivity
{
    public class DBX
    {
        /// <summary>
        /// DBX.DBX0.0
        /// </summary>
        public bool BitVariable0 { get; set; }

        /// <summary>
        /// DBX.DBX0.1
        /// </summary>
        public bool BitVariable1 { get; set; }

        /// <summary>
        /// DBX.DBX0.2
        /// </summary>
        public bool BitVariable2 { get; set; }

        /// <summary>
        /// DBX.DBW2
        /// </summary>
        public short IntVariable0 { get; set; }

        /// <summary>
        /// DBX.DBW4
        /// </summary>
        public short IntVariable1 { get; set; }

        /// <summary>
        /// DBX.DBD6
        /// </summary>
        public int DIntVariable0 { get; set; }

        /// <summary>
        /// DBX.DBD10
        /// </summary>
        public int DIntVariable1 { get; set; }

        /// <summary>
        /// DBX.DBD14
        /// </summary>
        public int DIntVariable2 { get; set; }

        /// <summary>
        /// DBX.DBD18
        /// </summary>
        public int DIntVariable3 { get; set; }

        /// <summary>
        /// DBX.DBD22
        /// </summary>
        public int DIntVariable4 { get; set; }
    }

    /// <summary>
    /// DBX1.
    /// </summary>
    public class DBCounter
    {
        public int PV { get; set; }

        public int CV { get; set; }
    }

    /// <summary>
    /// DBX2.
    /// </summary>
    public class DBTimer
    {
        public DateTime PT { get; set; }

        public DateTime ET { get; set; }

        public bool IN { get; set; }

        public bool Q { get; set; }
    }
}
