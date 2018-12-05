using S7.Net;
using S7NetWrapper;
using System;
using System.Collections.Generic;
using System.Net;

namespace ProductionEquipmentControlSoftware.PlcConnectivity
{
    class Plc
    {
        #region Singleton

        // For implementation refer to: http://geekswithblogs.net/BlackRabbitCoder/archive/2010/05/19/c-system.lazylttgt-and-the-singleton-design-pattern.aspx        
        private static readonly Lazy<Plc> _instance = new Lazy<Plc>(() => new Plc());

        public static Plc Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        #endregion

        #region Public properties

        public ConnectionStates ConnectionState
        {
            get
            {
                return plcDriver != null ? plcDriver.ConnectionState : ConnectionStates.Offline;
            }
        }

        //public DB1 Db1 { get; set; }

        public TimeSpan CycleReadTime { get; private set; }

        #endregion

        #region Private fields

        IPlcSyncDriver plcDriver;

        System.Timers.Timer timer = new System.Timers.Timer();

        public DateTime lastReadTime;

        #endregion

        #region Constructor

        private Plc()
        {
            //Db1 = new DB1();

            timer.Interval = 100; // ms
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
            lastReadTime = DateTime.Now;
        }

        #endregion

        #region Event handlers

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (plcDriver == null || plcDriver.ConnectionState != ConnectionStates.Online)
            {
                return;
            }

            timer.Enabled = false;
            CycleReadTime = DateTime.Now - lastReadTime;
            try
            {
                //RefreshTags();
            }
            finally
            {
                timer.Enabled = true;
                lastReadTime = DateTime.Now;
            }
        }

        #endregion

        #region Public methods

        public void Connect(string ipAddress)
        {
            if (!IsValidIp(ipAddress))
            {
                throw new ArgumentException("Ip address is not valid");
            }
            short rack = 0;
            short slot = 1; // The "Plcsim Rack / Slot" options are 0 and 1 for S7-1200 PLCs.
            plcDriver = new S7NetPlcDriver(CpuType.S71200, ipAddress, rack, slot);
            plcDriver.Connect();
        }

        public void Disconnect()
        {
            if (plcDriver == null || this.ConnectionState == ConnectionStates.Offline)
            {
                return;
            }
            plcDriver.Disconnect();
        }

        public List<Tag> Read(List<Tag> itemList)
        {
            if (plcDriver == null || plcDriver.ConnectionState != ConnectionStates.Online)
            {
                return new List<Tag>();
            }
            List<Tag> tagList = plcDriver.ReadItems(itemList);
            return tagList;
        }

        public void ReadClass(object sourceClass, int db)
        {
            if (plcDriver == null || plcDriver.ConnectionState != ConnectionStates.Online)
            {
                return;
            }
            plcDriver.ReadClass(sourceClass, db);
        }

        public void Write(string name, object value)
        {
            if (plcDriver == null || plcDriver.ConnectionState != ConnectionStates.Online)
            {
                return;
            }
            Tag tag = new Tag(name, value);
            List<Tag> tagList = new List<Tag>();
            tagList.Add(tag);
            plcDriver.WriteItems(tagList);
        }

        public void Write(List<Tag> tags)
        {
            if (plcDriver == null || plcDriver.ConnectionState != ConnectionStates.Online)
            {
                return;
            }
            plcDriver.WriteItems(tags);
        }

        #endregion        

        #region Private methods

        private bool IsValidIp(string addr)
        {
            IPAddress ip;
            bool valid = !string.IsNullOrEmpty(addr) && IPAddress.TryParse(addr, out ip);
            return valid;
        }

        private void RefreshTags()
        {
            //plcDriver.ReadClass(Db1, 1);
        }

        #endregion

    }
}
