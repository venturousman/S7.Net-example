namespace ProductionEquipmentControlSoftware.PlcConnectivity
{
    public class PlcTags
    {
        //public const string BitVariable = "DB1.DBX0.0";
        //public const string IntVariable = "DB1.DBW2";
        //public const string DoubleVariable = "DB1.DBD4";
        //public const string DIntVariable = "DB1.DBD8";
        //public const string DwordVariable = "DB1.DBW12";

        /// <summary>
        /// On/Off machine
        /// </summary>
        public const string BitVariable0 = "DB{0}.DBX0.0";
        public const string BitVariable1 = "DB{0}.DBX0.1";
        public const string BitVariable2 = "DB{0}.DBX0.2";

        public const string IntVariable0 = "DB{0}.DBW2";
        public const string IntVariable1 = "DB{0}.DBW4";

        public const string DIntVariable0 = "DB{0}.DBD6";
        public const string DIntVariable1 = "DB{0}.DBD10";
        public const string DIntVariable2 = "DB{0}.DBD14";
        public const string DIntVariable3 = "DB{0}.DBD18";
        public const string DIntVariable4 = "DB{0}.DBD22";

        /*
        public const string BitVariable0 = "M2{0}.0";
        public const string BitVariable1 = "M2{0}.1";
        public const string BitVariable2 = "M2{0}.2";
        public const string BitVariable3 = "M2{0}.3";
        public const string BitVariable4 = "M2{0}.4";

        /// <summary>
        /// actual/expected quantity from counter
        /// </summary>
        public const string IntVariable = "DB1.DBW{0}";

        public const string DIntVariable1 = "MD2{0}1";
        public const string DIntVariable2 = "MD2{0}2";
        public const string DIntVariable3 = "MD2{0}3";
        */
    }
}
