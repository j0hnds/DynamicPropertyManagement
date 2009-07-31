
using System;

namespace DomainCore
{
    public abstract class BaseAttribute : Attribute
    {
        private static int populating = 0;
        
        private string name;
        private bool id;
        private bool dirty;

        public BaseAttribute(Domain domain, string name, bool id)
        {
            this.name = name;
            this.id = id;
            if (domain != null)
            {
                domain.AddAttribute(this);
            }
        }

        public static int BeginPopulation()
        {
            return populating++;
        }

        public static int EndPopulation()
        {
            return populating--;
        }

        public bool Populating
        {
            get { return populating != 0; }
        }

        public string Name
        {
            get { return name; }
        }

        public bool Id
        {
            get { return id; }
        }

        public bool Dirty
        {
            get { return dirty; }
            set
            {
                if (value)
                {
                    if (! Populating)
                    {
                        dirty = value;
                    }
                }
                else
                {
                    dirty = value;
                }
            }
        }

        public virtual object Value { get; set; }

        public virtual bool Empty 
        {
            get { return false; }
        }
    }

    public class DateTimeAttribute : BaseAttribute
    {
        private DateTime attrValue;

        public DateTimeAttribute(Domain domain, string name) :
            base(domain, name, false)
        {
        }

        override public object Value
        {
            get { return attrValue; }
            set
            {
                attrValue = (value == null) ? DateTime.MinValue : (DateTime) value;
                Dirty = true;
            }
        }

    }

    public class LongAttribute : BaseAttribute
    {
        private long attrValue;

        public LongAttribute(Domain domain, string name, bool id) : 
            base(domain, name, id)
        {
        }
        
        override public object Value
        {
            get { return attrValue; }
            set
            {
                if (value == null)
                {
                    // New value is null
                    if (attrValue != 0L)
                    {
                        attrValue = 0L;
                        Dirty = true;
                    }
                }
                else
                {
                    long newValue = Convert.ToInt64(value);
                    if (newValue != attrValue)
                    {
                        attrValue = newValue;
                        Dirty = true;
                    }
                }
            }
        }
    }
    
    public class StringAttribute : BaseAttribute
    {
        private string attrValue;
        
        public StringAttribute(Domain domain, string name, bool id) : 
            base(domain, name, id)
        {
            
        }

        public override bool Empty 
        {
            get 
            { 
                return attrValue == null || attrValue.Length == 0; 
            }
        }
        
        override public object Value
        {
            get { return attrValue; }
            set
            {
                if (attrValue == null)
                {
                    // Current value is null
                    if (value != null)
                    {
                        attrValue = value.ToString();
                        Dirty = true;
                    }
                }
                else
                {
                    // Current value is not null
                    if (! attrValue.Equals(value))
                    {
                        if (value != null)
                        {
                            attrValue = value.ToString();
                        }
                        else
                        {
                            attrValue = null;
                        }
                        
                        Dirty = true;
                    }
                }
            }
        }
    }
}
