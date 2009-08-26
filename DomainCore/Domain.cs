
using System;
using System.Reflection;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace DomainCore
{
    /// <summary>
    /// This delegate is used by subscribers to register interest in AttributeValueChanged
    /// events.
    /// </summary>
    public delegate void AttributeValueChangeHandler(string attributeName, object oldValue, object newValue);
    
    /// <summary>
    /// Factory class to instantiate domain objects.
    /// </summary>
    public class DomainFactory
    {
        // The single instance of the DomainFactory
        private static DomainFactory instance = null;

        // The namespace in which we expect to find domain objects.
        private static string domainNamespace = null;

        // The namespace in which we expect to find dao objects.
        private static string daoNamespace = null;

        // The assembly in which to find the domain objects
        private static string domainAssembly = null;

        // The assembly in which to find the DAO objects.
        private static string daoAssembly = null;

        /// <value>
        /// Static readonly property for the singleton instance of the DomainFactory.
        /// </value>
        public static DomainFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DomainFactory();
                }

                return instance;
            }
        }

        /// <value>
        /// Static read/write property for the namespace to use for domain objects. This value will be prepended
        /// to the domain class name to obtain the fully-qualified class name. If not specified, no namespace is
        /// prepended.
        /// </value>
        public static string DomainNamespace 
        {
            get { return domainNamespace; }
            set { domainNamespace = value; }
        }

        /// <value>
        /// Static read/write property for the assembly which contains the domain objects.
        /// </value>
        public static string DomainAssembly
        {
            get { return domainAssembly; }
            set { domainAssembly = value; }
        }

        /// <value>
        /// Static read/write property for the namespace to use for DAO objects. This value will be prepended to the
        /// DAO class name to obtain the fully-qualified class name. If not specified, no namespace is prepended.
        /// </value>
        public static string DAONamespace
        {
            get { return daoNamespace; }
            set { daoNamespace = value; }
        }

        /// <value>
        /// Static read/write property for the assembly where the DAOs are located.
        /// </value>
        public static string DAOAssembly
        {
            get { return daoAssembly; }
            set { daoAssembly = value; }
        }

        /// <summary>
        /// Returns the DAO for the specified domain name.
        /// </summary>
        /// <param name="domainName">
        /// The name of the domain for which the DAO is to be returned.
        /// </param>
        /// <returns>
        /// Reference to the DAO. An exception is thrown if there is a problem
        /// finding the DAO.
        /// </returns>
        public static DomainDAO GetDAO(string domainName)
        {
            return Instance.GetDomainDAO(domainName);
        }

        /// <summary>
        /// Constructs a new domain object set as a NewObject.
        /// </summary>
        /// <param name="domainName">
        /// The name of the domain object to create. If no namespace is set on the factory, this should be the
        /// fully qualified name of the domain class name.
        /// </param>
        /// <returns>
        /// Instance of the specified domain object with the NewObject attribute set to true.
        /// </returns>
        public static Domain Create(string domainName)
        {
            return Create(domainName, true);
        }

        /// <summary>
        /// Constructs a new domain object whose new flag is specified.
        /// </summary>
        /// <param name="domainName">
        /// The name of the domain object to create. If no namespace is set on the factory, this should be the
        /// fully qualified name of the domain class name.
        /// </param>
        /// <param name="newObject">
        /// true if the newly constructed domain object is to be set as a new object.
        /// </param>
        /// <returns>
        /// An instance of the specified domain object.
        /// </returns>
        public static Domain Create(string domainName, bool newObject)
        {
            return Instance.CreateDomain(domainName, newObject);
        }
            

        /// <summary>
        /// Private constructor to keep others from instantiating this class.
        /// </summary>
        private DomainFactory()
        {
        }

        /// <summary>
        /// Constructs a new domain object whose new flag is specified.
        /// </summary>
        /// <param name="domainName">
        /// The name of the domain object to create. If no namespace is set on the factory, this should be the
        /// fully qualified name of the domain class name.
        /// </param>
        /// <param name="newObject">
        /// true if the newly constructed domain object is to be set as a new object.
        /// </param>
        /// <returns>
        /// An instance of the specified domain object.
        /// </returns>
        private Domain CreateDomain(string domainName, bool newObject)
        {
            Domain domain = null;

            // Build the class names we want
            string domainClassName = null;
            if (domainNamespace == null)
            {
                domainClassName = domainName;
            }
            else
            {
                domainClassName = String.Format("{0}.{1}", domainNamespace, domainName);
            }
            if (domainAssembly != null)
            {
                domainClassName += String.Format(",{0}", domainAssembly);
            }
            
            // Get the type for the domain class (throw exception if fails)
            Type domainType = Type.GetType(domainClassName, true);

            DomainDAO dao = GetDomainDAO(domainName);

            // Get and invoke the constructor for the domain object
            Type[] domainConstructorTypes = { typeof(DomainDAO) };
            ConstructorInfo ci = domainType.GetConstructor(domainConstructorTypes);
            object[] args1 = { dao };

            domain = ci.Invoke(args1) as Domain;

            if (newObject)
            {
                // Must be marked as a new object.
                domain.NewObject = newObject;
            }

            return domain;
        }

        /// <summary>
        /// Returns the DAO for the specified domain name.
        /// </summary>
        /// <param name="domainName">
        /// The name of the domain for which the DAO is to be returned.
        /// </param>
        /// <returns>
        /// Reference to the DAO. An exception is thrown if there is a problem
        /// finding the DAO.
        /// </returns>
        private DomainDAO GetDomainDAO(string domainName)
        {
            string daoClassName = null;
            if (daoNamespace == null)
            {
                daoClassName = String.Format("{0}DAO", domainName);
            }
            else
            {
                // Remove any name qualification from the domain before prepending
                // the dao namespace.
                int lastDot = domainName.LastIndexOf('.');
                if (lastDot >= 0)
                {
                    domainName = domainName.Substring(lastDot + 1);
                }
                daoClassName = String.Format("{0}.{1}DAO", daoNamespace, domainName);
            }
            if (daoAssembly != null)
            {
                daoClassName += String.Format(",{0}", daoAssembly);
            }
            
            // Get the type of the DAO class (throw exception if fails)
            Type daoType = Type.GetType(daoClassName, true);

            // Get and invoke the constructor for the DAO object
            Type[] daoConstructorTypes = {};
            ConstructorInfo ci = daoType.GetConstructor(daoConstructorTypes);
            object[] args = {};
            
            return ci.Invoke(args) as DomainDAO;
        }
    }

    /// <summary>
    /// Defines the interface a DAO must have to support data access methods
    /// for a domain object.
    /// </summary>
    public interface DomainDAO 
    {
        /// <summary>
        /// Constructs the SQL DELETE statement for the domain object.
        /// </summary>
        /// <param name="obj">
        /// Reference to the domain object for which to construct the DELETE statement.
        /// </param>
        /// <returns>
        /// SQL DELETE statement for the domain object.
        /// </returns>
        string DeleteSQL(Domain obj);
        /// <summary>
        /// Constructs the SQL INSERT statement for the domain object.
        /// </summary>
        /// <param name="obj">
        /// Reference to the domain object for which to construct the INSERT statement.
        /// </param>
        /// <returns>
        /// SQL INSERT statement for the domain object.
        /// </returns>
        string InsertSQL(Domain obj);
        /// <summary>
        /// Constructs the SQL UPDATE statement for the domain object.
        /// </summary>
        /// <param name="obj">
        /// Reference to the domain object for which to construct the UPDATE statement.
        /// </param>
        /// <returns>
        /// SQL UPDATE statement for the domain object.
        /// </returns>
        string UpdateSQL(Domain obj);
        /// <summary>
        /// Performs the actual DELETE on the domain object on the backing store.
        /// </summary>
        /// <param name="obj">
        /// Reference to the domain object to delete.
        /// </param>
        void Delete(Domain obj);
        /// <summary>
        /// Performs the actual INSERT on the domain object on the backing store.
        /// </summary>
        /// <param name="obj">
        /// Reference to the domain object to insert.
        /// </param>
        void Insert(Domain obj);
        /// <summary>
        /// Performs the actual UPDATE on the domain object on the backing store.
        /// </summary>
        /// <param name="obj">
        /// Reference to the domain object to update.
        /// </param>
        void Update(Domain obj);
        /// <summary>
        /// Returns a domain object from backing store by specifying the unique identifier.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the domain object.
        /// </param>
        /// <returns>
        /// The domain object returned from backing store.
        /// </returns>
        Domain GetObject(object id);
        /// <summary>
        /// Returns a list of domain objects based on the parameters passed in.
        /// </summary>
        /// <param name="argsRest">
        /// The optional list of parameters to send to the method.
        /// </param>
        /// <returns>
        /// A list of domain objects that satisfy the arguments.
        /// </returns>
        List<Domain> Get(params object[] argsRest);
    }

    /// <summary>
    /// The methods and properties required of a domain attribute.
    /// </summary>
    public interface Attribute
    {
        /// <value>
        /// The name of the attribute.
        /// </value>
        string Name { get; }
        /// <value>
        /// The attribute value.
        /// </value>
        object Value { get; set; }
        /// <value>
        /// If <c>true</c>, the value of the attribute has changed since the
        /// last time it was populated.
        /// </value>
        bool Dirty { get; set; }
        /// <value>
        /// If <c>true</c>, indicates that this attribute is the unique identifier
        /// of the domain object.
        /// </value>
        bool Id { get; }
        /// <value>
        /// If <c>true</c>, indicates that the values of attributes are being populated.
        /// </value>
        bool Populating { get; }
        /// <value>
        /// If <c>true</c>, indicates that the attribute value is considered to be "empty".
        /// </value>
        bool Empty { get; }
        /// <summary>
        /// Reverts the value of the attribute to the last-saved-value.
        /// </summary>
        void Revert();
    }

    /// <summary>
    /// Defines the required interface of a domain relationship.
    /// </summary>
    public interface Relationship
    {
        /// <value>
        /// Name of the relationship
        /// </value>
        string Name { get; }
        /// <value>
        /// The collection of related objects not including deleted objects.
        /// </value>
        List<Domain> CollectedObjects { get; }
        /// <value>
        /// If <c>true</c>, indicates that one or more of the related objects in the
        /// relationship are "Dirty"
        /// </value>
        bool Dirty { get; }
        /// <summary>
        /// Saves the objects in the relationship.
        /// </summary>
        /// <param name="parentId">
        /// The unique identifier of the owner of the relationship to make
        /// sure that the child objects have the parent object id.
        /// </param>
        void Save(object parentId);
        /// <summary>
        /// Creates the SQL statements that would be issued if the relationship
        /// were to be saved.
        /// </summary>
        /// <param name="parentId">
        /// The unique identifier of the owner of the relationship.
        /// </param>
        /// <returns>
        /// SQL statement(s) necessary to perform and required persistence of the
        /// objects in the relationship.
        /// </returns>
        string SaveSQL(object parentId);
        /// <summary>
        /// Reverts the collection and any collected objects in the relationship.
        /// </summary>
        void Revert();
    }


    /// <summary>
    /// A domain object is essentially the same as a value object except that
    /// it incorporates a certain amount of intelligence about itself and its
    /// surrounding objects.
    /// </summary>
    public class Domain
    {
        /// <summary>
        /// The data access object associated with this domain object.
        /// </summary>
        private DomainDAO dao;
        /// <summary>
        /// If <c>true</c>, indicates that this object has not yet been saved to
        /// backing store.
        /// </summary>
        private bool newObject;
        /// <summary>
        /// If <c>true</c>, indicates that this object has been marked for deletion
        /// from backing store.
        /// </summary>
        private bool forDelete;
        /// <summary>
        /// A map of the attributes associated with this domain. The key to
        /// the map is the name of the attribute.
        /// </summary>
        private Dictionary<string,Attribute> attributes;
        /// <summary>
        /// A map of the relationships associated with this domain. The key to
        /// the map is the name of the relationship.
        /// </summary>
        private Dictionary<string,Relationship> relationships;

        /// <summary>
        /// Constructs a new domain object.
        /// </summary>
        /// <param name="dao">
        /// Reference to the DAO for this domain object.
        /// </param>
        public Domain(DomainDAO dao)
        {
            this.dao = dao;
            newObject = false;
            forDelete = false;
            attributes = new Dictionary<string,Attribute>(5);
            relationships = new Dictionary<string,Relationship>(2);
        }

        /// <value>
        /// The DAO associated with this domain object.
        /// </value>
        public DomainDAO DAO
        {
            get { return this.dao; }
        }

        /// <summary>
        /// Revert the values of the attributes and relationships
        /// to their state as of the last save.
        /// </summary>
        public void Revert()
        {
            if (! NewObject)
            {
                if (ForDelete)
                {
                    ForDelete = false;
                }
                
                foreach (Attribute attr in attributes.Values)
                {
                    attr.Revert();
                }
    
                foreach (Relationship rel in relationships.Values)
                {
                    rel.Revert();
                }
            }
        }

        /// <summary>
        /// Returns the collection of related objects.
        /// </summary>
        /// <param name="name">
        /// The name of the relationship for which to return the objects.
        /// </param>
        /// <returns>
        /// The list of related domain objects.
        /// </returns>
        public List<Domain> GetCollection(string name)
        {
            return GetRelationship(name).CollectedObjects;
        }

        /// <summary>
        /// Returns the specified relationship.
        /// </summary>
        /// <param name="name">
        /// The name of the relationship
        /// </param>
        /// <returns>
        /// Reference to the relationship.
        /// </returns>
        public Relationship GetRelationship(string name)
        {
            return relationships[name];
        }

        /// <summary>
        /// Marks all the attributes of this domain to be not Dirty.
        /// </summary>
        public void Clean()
        {
            foreach (Attribute attr in attributes.Values)
            {
                attr.Dirty = false;
            }
        }

        /// <value>
        /// If <c>true</c>, indicates that this object has not yet been
        /// saved to backing store.
        /// </value>
        public bool NewObject
        {
            get { return newObject; }
            set { newObject = value; }
        }

        /// <value>
        /// If <c>true</c>, indicates that this object has been marked for deletion
        /// from backing store.
        /// </value>
        public bool ForDelete
        {
            get { return forDelete; }
            set { forDelete = value; }
        }

        /// <summary>
        /// Adds a relationship to this domain.
        /// </summary>
        /// <param name="rel">
        /// The relationship to add.
        /// </param>
        public void AddRelationship(Relationship rel)
        {
            relationships[rel.Name] = rel;
        }

        /// <summary>
        /// Adds an attribute to this domain.
        /// </summary>
        /// <param name="att">
        /// The attribute to add.
        /// </param>
        public void AddAttribute(Attribute att)
        {
            attributes[att.Name] = att;
        }

        /// <summary>
        /// Retrieves the specified attribute.
        /// </summary>
        /// <param name="name">
        /// The name of the attribute
        /// </param>
        /// <returns>
        /// The attribute of the specified name.
        /// </returns>
        public Attribute GetAttribute(string name)
        {
            return attributes[name];
        }

        /// <summary>
        /// Retrieves the value of the specified attribute.
        /// </summary>
        /// <param name="attrName">
        /// The name of the attribute.
        /// </param>
        /// <returns>
        /// The current value of the specified attribute.
        /// </returns>
        public object GetValue(string attrName)
        {
            return attributes[attrName].Value;
        }

        /// <summary>
        /// Sets the value of the specified attribute.
        /// </summary>
        /// <param name="attrName">
        /// The name of the attribute.
        /// </param>
        /// <param name="value">
        /// The value to which the attribute should be set.
        /// </param>
        public void SetValue(string attrName, object value)
        {
            attributes[attrName].Value = value;
        }

        /// <value>
        /// If <c>true</c>, indicates that at least one attribute is Dirty.
        /// </value>
        private bool AttributesDirty
        {
            get
            {
                bool dirty = false;

                foreach (Attribute attr in attributes.Values)
                {
                    if (attr.Dirty)
                    {
                        dirty = true;
                        break;
                    }
                }

                return dirty;
            }
        }

        /// <value>
        /// If <c>true</c>, indicates that the state of the domain object
        /// requires saving.
        /// </value>
        public bool Dirty 
        {
            get
            {
                bool dirty = NewObject || ForDelete;

                if (! dirty)
                {
                    foreach (Attribute attr in attributes.Values)
                    {
                        if (attr.Dirty)
                        {
                            dirty = true;
                            break;
                        }
                    }
                    if (! dirty)
                    {
                        foreach (Relationship rel in relationships.Values)
                        {
                            if (rel.Dirty)
                            {
                                dirty = true;
                                break;
                            }
                        }
                    }
                }

                return dirty;
            }
        }

        /// <value>
        /// The map of attribute on the domain.
        /// </value>
        public Dictionary<string,Attribute> Attributes
        {
            get
            {
                return attributes;
            }
        }

        /// <value>
        /// The attribute marked as the Id attribute for the domain.
        /// </value>
        public Attribute IdAttribute
        {
            get
            {
                Attribute idAttr = null;

                foreach (Attribute attr in attributes.Values)
                {
                    if (attr.Id)
                    {
                        idAttr = attr;
                        break;
                    }
                }

                return idAttr;
            }
        }

        /// <summary>
        /// Saves the state of the domain object to the backing store.
        /// </summary>
        public void Save()
        {
            // Only do something if it is necessary
            if (Dirty)
            {
                // Dirty; figure out what needs to be done and do it.
                if (ForDelete)
                {
                    // Call the dao method to delete the object.
                    dao.Delete(this);
                }
                else if (NewObject)
                {
                    // Call the dao method to insert the object.
                    dao.Insert(this);
                }
                else if (AttributesDirty)
                {
                    // Must have been modified, update the object using the dao
                    dao.Update(this);
                }

                object parentId = IdAttribute.Value;

                // Now that we know this object is saved, deal with the relationships
                foreach (Relationship rel in relationships.Values)
                {
                    rel.Save(parentId);
                }
            }
        }

        /// <summary>
        /// Returns the SQL that would be issued if the state of the object were to
        /// be saved to backing store.
        /// </summary>
        /// <returns>
        /// SQL statement(s) required to save the domain object to backing store.
        /// </returns>
        public string SaveSQL()
        {
            StringBuilder sql = new StringBuilder();

            // Only do something if it is necessary
            if (Dirty)
            {
                // Dirty; figure out what needs to be done and do it.
                if (ForDelete)
                {
                    // Call the dao method to delete the object.
                    sql.Append(dao.DeleteSQL(this));
                }
                else if (NewObject)
                {
                    // Call the dao method to insert the object.
                    sql.Append(dao.InsertSQL(this));
                }
                else if (AttributesDirty)
                {
                    // Must have been modified, update the object using the dao
                    sql.Append(dao.UpdateSQL(this));
                }

                object parentId = IdAttribute.Value;

                // Now that we know this object is saved, deal with the relationships
                foreach (Relationship rel in relationships.Values)
                {
                    if (sql.Length > 0)
                    {
                        sql.Append(";");
                    }
                    sql.Append(rel.SaveSQL(parentId));
                }
            }

            return sql.ToString();
        }

    }
}
