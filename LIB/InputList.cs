using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LIB
{
    [Serializable]
    [XmlRoot("InputLists")]
    public class InputLists : List<InputList>
    {

    }

    [Serializable]
    [XmlRoot("InputList")]
    public class InputList
    {
        private string _name = String.Empty;
        private List<InputListItem> _items = new List<InputListItem>();

        public InputList()
        {
        }
        public InputList(string name)
        {
            Name = name;
        }

        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<InputListItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        [XmlAttribute]
        public string Name
        {
            get { return _name == null ? String.Empty : _name; }
            set { _name = value.Trim(); }
        }
    }

    [Serializable]
    [XmlRoot("Item")]
    public class InputListItem
    {
        private string _name = String.Empty;
        private string _text = String.Empty;
        private int _order = 0;

        public InputListItem()
        {
        }

        public InputListItem(string name, string text)
        {
            Name = name;
            Text = text;
        }
        public InputListItem(string name)
        {
            Name = name;
        }

        [XmlAttribute]
        public string Name
        {
            get { return _name == null ? String.Empty : _name; }
            set { _name = value.Trim(); }
        }
        [XmlAttribute]
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        [XmlText]
        public string Text
        {
            get { return _text == null ? String.Empty : _text; }
            set { _text = value.Trim(); }
        }
    }
}
