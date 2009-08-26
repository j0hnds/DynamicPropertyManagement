
using System;

namespace DomainCore
{
    /// <summary>
    /// Abstract base class for a domain attribute.
    /// </summary>
    public abstract class BaseAttribute : Attribute
    {
        /// <summary>
        /// A value greater than 0 indicates that the system is populating the
        /// values of attributes.
        /// </summary>
        private static int populating = 0;

        /// <summary>
        /// The name of the attribute
        /// </summary>
        private string name;
        /// <summary>
        /// If <c>true</c>, indicates that this attribute instance represents the
        /// unique identifier of a domain object.
        /// </summary>
        private bool id;
        /// <summary>
        /// If <c>true</c>, indicates that this attribute has had a value change since
        /// the last time it was populated.
        /// </summary>
        private bool dirty;

        /// <summary>
        /// Constructs a new BaseAttribute object.
        /// </summary>
        /// <param name="domain">
        /// A reference to the domain object that owns this attribute.
        /// </param>
        /// <param name="name">
        /// The name of the attribute.
        /// </param>
        /// <param name="id">
        /// If <c>true</c>, indicates that this attribute is the ID attribute of the
        /// domain object.
        /// </param>
        public BaseAttribute(Domain domain, string name, bool id)
        {
            this.name = name;
            this.id = id;
            if (domain != null)
            {
                domain.AddAttribute(this);
            }
        }

        /// <summary>
        /// This event is fired when the attribute value is changed.
        /// </summary>
        /// <remarks>
        /// Will not be fired when attributes are being populated.
        /// </remarks>
        public event AttributeValueChangeHandler AttributeValueChanged;

        /// <summary>
        /// Notifies all subscribers of an AttributeValueChanged event that the
        /// value has changed.
        /// </summary>
        /// <param name="oldValue">
        /// The original value of the attribute.
        /// </param>
        /// <param name="newValue">
        /// The new value of the attribute.
        /// </param>
        protected void OnAttributeValueChanged(object oldValue, object newValue)
        {
            AttributeValueChangeHandler handler = AttributeValueChanged;

            if (handler != null && ! Populating)
            {
                handler(name, oldValue, newValue);
            }
        }

        /// <summary>
        /// Increments the populating flag to indicate that we are currently
        /// populating attribute values.
        /// </summary>
        /// <returns>
        /// The value of the population flag prior to incrementing.
        /// </returns>
        public static int BeginPopulation()
        {
            return populating++;
        }

        /// <summary>
        /// Decrements the populating flag to indicating that we are no longer
        /// populating attribute values.
        /// </summary>
        /// <returns>
        /// The value of the population flag prior to decrementing.
        /// </returns>
        public static int EndPopulation()
        {
            return populating--;
        }

        /// <value>
        /// If <c>true</c>, indicates that we are currently populating attribute
        /// values.
        /// </value>
        public bool Populating
        {
            get { return populating != 0; }
        }

        /// <value>
        /// The name of the attribute.
        /// </value>
        public string Name
        {
            get { return name; }
        }

        /// <value>
        /// If <c>true</c>, indicates that this attribute value represents the
        /// unique identifier of a domain object.
        /// </value>
        public bool Id
        {
            get { return id; }
        }

        /// <value>
        /// If <c>true</c>, indicates that the value of this attribute has been
        /// modified since originally populated.
        /// </value>
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

        /// <value>
        /// The value of the attribute.
        /// </value>
        public virtual object Value { get; set; }

        /// <value>
        /// If <c>true</c>, indicates that the attribute value is considered
        /// "empty".
        /// </value>
        public virtual bool Empty 
        {
            get { return false; }
        }

        /// <summary>
        /// Reverts the attribute value to the last populated value.
        /// </summary>
        public virtual void Revert()
        {
        }
    }

    /// <summary>
    /// A domain attribute for holding DateTime values.
    /// </summary>
    public class DateTimeAttribute : BaseAttribute
    {
        /// <summary>
        /// The current value of the attribute.
        /// </summary>
        private DateTime attrValue;
        /// <summary>
        /// The value of the attribute at the time of population.
        /// </summary>
        private DateTime previousValue;

        /// <summary>
        /// Constructs a new DateTime attribute.
        /// </summary>
        /// <param name="domain">
        /// Reference to the domain object that owns this attribute.
        /// </param>
        /// <param name="name">
        /// The name of the attribute.
        /// </param>
        public DateTimeAttribute(Domain domain, string name) :
            base(domain, name, false)
        {
        }

        override public object Value
        {
            get { return attrValue; }
            set
            {
                if (! Dirty)
                {
                    // Save the old value for posterity
                    previousValue = attrValue;
                }
                object oldValue = attrValue;
                object newValue = value;
                attrValue = (value == null) ? DateTime.MinValue : (DateTime) value;
                Dirty = true;
                OnAttributeValueChanged(oldValue, newValue);
            }
        }

        public override void Revert ()
        {
            if (Dirty)
            {
                attrValue = previousValue;
                Dirty = false;
            }
        }

    }

    /// <summary>
    /// A domain attribute for holding a long (int64) value.
    /// </summary>
    public class LongAttribute : BaseAttribute
    {
        /// <summary>
        /// The current value of the attribute.
        /// </summary>
        private long attrValue;
        /// <summary>
        /// The value of the attribute at the time of the last population.
        /// </summary>
        private long previousValue;

        /// <summary>
        /// Constructs a new LongAttribute.
        /// </summary>
        /// <param name="domain">
        /// Reference to the domain object that owns the attribute.
        /// </param>
        /// <param name="name">
        /// The name of the attribute.
        /// </param>
        /// <param name="id">
        /// <c>true</c> if this attribute represents the unique identifier for
        /// a domain object.
        /// </param>
        public LongAttribute(Domain domain, string name, bool id) : 
            base(domain, name, id)
        {
        }
        
        override public object Value
        {
            get { return attrValue; }
            set
            {
                if (! Dirty)
                {
                    previousValue = attrValue;
                }
                object oldValue = attrValue;
                object newerValue = value;
                if (value == null)
                {
                    // New value is null
                    if (attrValue != 0L)
                    {
                        attrValue = 0L;
                        Dirty = true;
                        OnAttributeValueChanged(oldValue, newerValue);
                    }
                }
                else
                {
                    long newValue = Convert.ToInt64(value);
                    if (newValue != attrValue)
                    {
                        attrValue = newValue;
                        Dirty = true;
                        OnAttributeValueChanged(oldValue, newerValue);
                    }
                }
            }
        }

        public override void Revert ()
        {
            if (Dirty)
            {
                attrValue = previousValue;
                Dirty = false;
            }
        }
    }

    /// <summary>
    /// A domain attribute for holding a string value.
    /// </summary>
    public class StringAttribute : BaseAttribute
    {
        /// <summary>
        /// The current value of the attribute.
        /// </summary>
        private string attrValue;
        /// <summary>
        /// The value of the attribute at the time of the last population.
        /// </summary>
        private string previousValue;

        /// <summary>
        /// Constructs a new StringAttribute object.
        /// </summary>
        /// <param name="domain">
        /// Reference to the domain object that owns this attribute.
        /// </param>
        /// <param name="name">
        /// The name of the attribute.
        /// </param>
        /// <param name="id">
        /// <c>true</c> if this attribute represents the unique identifier for the
        /// domain object.
        /// </param>
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
                object oldValue = attrValue;
                object newValue = value;

                if (! Dirty)
                {
                    previousValue = attrValue;
                }
                
                if (attrValue == null)
                {
                    // Current value is null
                    if (value != null)
                    {
                        attrValue = value.ToString();
                        Dirty = true;
                        OnAttributeValueChanged(oldValue, newValue);
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
                        OnAttributeValueChanged(oldValue, newValue);
                    }
                }
            }
        }

        public override void Revert ()
        {
            if (Dirty)
            {
                attrValue = previousValue;
                Dirty = false;
            }
        }
    }
}
