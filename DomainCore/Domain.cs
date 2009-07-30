
using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

namespace DomainCore
{
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

            // Get the type for the domain class (throw exception if fails)
            Type domainType = Type.GetType(domainClassName, true);

            // Get the type of the DAO class (throw exception if fails)
            Type daoType = Type.GetType(daoClassName, true);

            // Get and invoke the constructor for the DAO object
            Type[] daoConstructorTypes = {};
            ConstructorInfo ci = daoType.GetConstructor(daoConstructorTypes);
            object[] args = {};
            
            DomainDAO dao = ci.Invoke(args) as DomainDAO;

            // Get and invoke the constructor for the domain object
            Type[] domainConstructorTypes = { typeof(DomainDAO) };
            ci = domainType.GetConstructor(domainConstructorTypes);
            object[] args1 = { dao };

            domain = ci.Invoke(args1) as Domain;

            if (newObject)
            {
                // Must be marked as a new object.
                domain.NewObject = newObject;
            }

            return domain;
        }
    }
    
    public interface DomainDAO 
    {
        string DeleteSQL(Domain obj);
        string InsertSQL(Domain obj);
        string UpdateSQL(Domain obj);
        void Delete(Domain obj);
        void Insert(Domain obj);
        void Update(Domain obj);
    }

    public interface Attribute
    {
        string Name { get; }
        object Value { get; set; }
        bool Dirty { get; set; }
        bool Id { get; }
        bool Populating { get; }
    }

    public interface Relationship
    {
        string Name { get; }
        List<Domain> CollectedObjects { get; }
        bool Dirty { get; }
        void Save(object parentId);
        string SaveSQL(object parentId);
    }

    
    public class Domain
    {
        // The data access object associated with this domain object.
        private DomainDAO dao;
        private bool newObject;
        private bool forDelete;
        private Dictionary<string,Attribute> attributes;
        private Dictionary<string,Relationship> relationships;
        
        public Domain(DomainDAO dao)
        {
            this.dao = dao;
            newObject = false;
            forDelete = false;
            attributes = new Dictionary<string,Attribute>(5);
            relationships = new Dictionary<string,Relationship>(2);
        }

        public List<Domain> GetCollection(string name)
        {
            return GetRelationship(name).CollectedObjects;
        }

        public Relationship GetRelationship(string name)
        {
            return relationships[name];
        }

        public bool NewObject
        {
            get { return newObject; }
            set { newObject = value; }
        }

        public bool ForDelete
        {
            get { return forDelete; }
            set { forDelete = value; }
        }

        public void AddRelationship(Relationship rel)
        {
            relationships[rel.Name] = rel;
        }

        public void AddAttribute(Attribute att)
        {
            attributes[att.Name] = att;
        }

        public Attribute GetAttribute(string name)
        {
            return attributes[name];
        }

        public object GetValue(string attrName)
        {
            return attributes[attrName].Value;
        }

        public void SetValue(string attrName, object value)
        {
            attributes[attrName].Value = value;
        }

        public void Clean()
        {
            foreach (KeyValuePair<string, Attribute> kvp in attributes)
            {
                kvp.Value.Dirty = false;
            }
        }

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

        public Dictionary<string,Attribute> Attributes
        {
            get
            {
                return attributes;
            }
        }

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
