using System;

namespace UserBehavior.Objects
{
    [Serializable]
    public class Type
    {
        public string Name { get; set; }

        public Type(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
