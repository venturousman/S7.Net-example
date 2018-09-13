#region Using
#endregion

namespace S7NetWrapper
{
    public class Tag
    {
        private string _itemName;
        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        private object _itemValue;
        public object ItemValue
        {
            get { return _itemValue; }
            set { _itemValue = value; }
        }

        public Tag()
        {

        }

        public Tag(string itemName)
        {
            this.ItemName = itemName;
        }

        public Tag(string itemName, object itemValue)
        {
            this.ItemName = itemName;
            this.ItemValue = itemValue;
        }
    }
}
