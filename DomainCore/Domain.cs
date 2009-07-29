
using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

namespace DomainCore
{
    public class DomainFactory
    {
        private static DomainFactory instance = null;

        private static string domainNamespace = null;

        private static string daoNamespace = null;

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

        public static string DomainNamespace 
        {
            get { return domainNamespace; }
            set { domainNamespace = value; }
        }

        public static string DAONamespace
        {
            get { return daoNamespace; }
            set { daoNamespace = value; }
        }

        public static Domain Create(string domainName)
        {
            return Create(domainName, true);
        }

        public static Domain Create(string domainName, bool newObject)
        {
            return Instance.CreateDomain(domainName, newObject);
        }
            

        private DomainFactory()
        {
        }

        private Domain CreateDomain(string domainName, bool newObject)
        {
            Domain domain = null;

            // Assert the configuration.
            if (domainNamespace == null)
            {
                throw new Exception("Domain Namespace is not configured for DomainFactory");
            }
            if (daoNamespace == null)
            {
                throw new Exception("DAO Namespace is not configured for DomainFactory");
            }

            // Build the class names we want
            string domainClassName = String.Format("{0}.{1}", domainNamespace, domainName);
            string daoClassName = String.Format("{0}.{1}DAO", daoNamespace, domainName);

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
        string DeleteSQL(object obj);
        string InsertSQL(object obj);
        string UpdateSQL(object obj);
        void Delete(object obj);
        void Insert(object obj);
        void Update(object obj);
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
