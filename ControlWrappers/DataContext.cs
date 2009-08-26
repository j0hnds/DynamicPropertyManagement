
using System;
using System.Collections;
using System.Collections.Generic;
using DomainCore;

namespace ControlWrappers
{
    /// <summary>
    /// Delegate to use to register interest in ContextChanged events.
    /// </summary>
    public delegate void ContextChangeHandler(string contextName, string itemName);

    /// <summary>
    /// A container for 0-or more domain objects or collections that are to be made
    /// available for data binding.
    /// </summary>
    /// <remarks>
    /// You can sort of think of a DataContext as a named Hashmap.
    /// </remarks>
    public class DataContext
    {
        /// <summary>
        /// The name of the data context.
        /// </summary>
        private string name;
        /// <summary>
        /// The hash table that holds the contents of the data context.
        /// </summary>
        private Hashtable contents;

        /// <summary>
        /// Constructs a new DataContext object with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the data context.
        /// </param>
        public DataContext(string name)
        {
            this.name = name;
            contents = new Hashtable();
        }

        /// <value>
        /// Event fired when the contents of the data context have changed.
        /// </value>
        public event ContextChangeHandler ContextChanged;

        /// <value>
        /// The name of the data context.
        /// </value>
        public string Name
        {
            get { return name; }
            set 
            { 
                name = value;
                OnContextChanged(null);
            }
        }

        /// <summary>
        /// Adds a new object to the data context.
        /// </summary>
        /// <param name="name">
        /// The name by which the new object should be registered in the context.
        /// </param>
        /// <param name="obj">
        /// The object to place in the context.
        /// </param>
        public void AddObject(string name, object obj)
        {
            contents[name] = obj;
            OnContextChanged(name);
        }

        /// <summary>
        /// Adds a new domain object to the data context.
        /// </summary>
        /// <remarks>
        /// The name with which the domain object is registered is the Class
        /// name of the domain object (not fully qualified).
        /// </remarks>
        /// <param name="obj">
        /// Reference to the domain object to be registered in the context.
        /// </param>
        public void AddObject(Domain obj)
        {
            AddObject(obj.GetType().Name, obj);
        }

        /// <summary>
        /// Removes the specified object from the context.
        /// </summary>
        /// <param name="name">
        /// The registered name of the object to remove.
        /// </param>
        public void RemoveObject(string name)
        {
            if (contents.ContainsKey(name))
            {
                contents.Remove(name);
                OnContextChanged(name);
            }
        }

        /// <value>
        /// The object registered with the specified name.
        /// </value>
        public object this[string name]
        {
            get
            {
                return contents[name];
            }
            set
            {
                contents[name] = value;
            }
        }

        /// <summary>
        /// Helper method to notify subscribers to the ContextChanged method that
        /// a change has occurred.
        /// </summary>
        /// <param name="item">
        /// The name of the object involved in the change.
        /// </param>
        private void OnContextChanged(string item)
        {
            ContextChangeHandler handler = ContextChanged;

            if (handler != null)
            {
                handler(name, item);
            }
        }

        /// <summary>
        /// Returns the Domain collection with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the collection registered in the context.
        /// </param>
        /// <returns>
        /// The domain collection.
        /// </returns>
        public List<Domain> GetCollection(string name)
        {
            return this[name] as List<Domain>;
        }

        /// <summary>
        /// Returns a reference to the domain with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the domain object to retrieve.
        /// </param>
        /// <returns>
        /// Reference to the specified domain. <c>null</c> if there is no
        /// domain registered by that name.
        /// </returns>
        public Domain GetDomain(string name)
        {
            return this[name] as Domain;
        }
    }
}
