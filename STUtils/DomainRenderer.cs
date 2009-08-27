
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using DomainCore;
using Antlr.StringTemplate;
using log4net;

namespace STUtils
{
    
    /// <summary>
    /// This class is responsible for constructing various named renditions
    /// of domain objects.
    /// </summary>
    public class DomainRenderer
    {
        /// <summary>
        /// The one-and-only instance of the DomainRenderer.
        /// </summary>
        private static DomainRenderer instance = null;
        /// <summary>
        /// The location of the base templates to be used by the system.
        /// </summary>
        private const string BASE_TEMPLATES_GROUP = "Templates/BaseTemplates";
        /// <summary>
        /// The configuration entry name for using relative template location.
        /// </summary>
        private const string RELATIVE_CONFIG = "UseRelativeTemplatesLocation";
        /// <summary>
        /// The configuration entry for the templates location.
        /// </summary>
        private const string TEMPLATE_LOCATION_CONFIG = "TemplatesLocation";
        /// <summary>
        /// The string template to use to construct a template name to load.
        /// </summary>
        private const string BASE_TEMPLATE_FORMAT = "Templates/{0}{1}";

        /// <summary>
        /// Reference to the loaded base template group.
        /// </summary>
        private StringTemplateGroup baseTemplateGroup;

        /// <summary>
        /// Map of the loaded template groups. The key to the mapping is
        /// the path to the group.
        /// </summary>
        private Dictionary<string,StringTemplateGroup> templateGroups;

        /// <summary>
        /// The date renderer to use.
        /// </summary>
        private DateRenderer dateRenderer;

        /// <summary>
        /// The logger to use.
        /// </summary>
        protected ILog log;

        /// <summary>
        /// Constructs a new Domain renderer object.
        /// </summary>
        private DomainRenderer()
        {
            log = LogManager.GetLogger(typeof(DomainRenderer));

            CommonGroupLoader loader = null;

            string location = null;
            
            if (ConfigurationManager.AppSettings[RELATIVE_CONFIG].Equals("true"))
            {
                location = Path.GetDirectoryName(Assembly.GetAssembly(GetType()).CodeBase);
                if (location.IndexOf("file:") == 0)
                {
                    location = location.Substring(6);
                }
                log.InfoFormat("Obtaining Templates from the Relative location: {0}", location);
            }
            else
            {
                location = ConfigurationManager.AppSettings[TEMPLATE_LOCATION_CONFIG];
                log.InfoFormat("Obtaining Templates from this location: {0}", location);
            }

            loader = new CommonGroupLoader(new ConsoleErrorListener(), location);
            
            StringTemplateGroup.RegisterGroupLoader(loader);

            baseTemplateGroup = StringTemplateGroup.LoadGroup(BASE_TEMPLATES_GROUP);

            templateGroups = new Dictionary<string, StringTemplateGroup>();

            dateRenderer = new DateRenderer();
            
        }

        /// <value>
        /// The one-and-only instance of the DomainRenderer.
        /// </value>
        public static DomainRenderer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DomainRenderer();
                }

                return instance;
            }
        }

        /// <summary>
        /// Renders the domain object using the specific type (template/maco)
        /// </summary>
        /// <param name="domain">
        /// Reference to the domain object to render.
        /// </param>
        /// <param name="type">
        /// The macro to use to render the domain object.
        /// </param>
        /// <returns>
        /// The rendered text.
        /// </returns>
        public static string Render(Domain domain, string type)
        {
            return Instance.RenderObject(domain, type);
        }

        /// <value>
        /// The template group mapped to a domain name.
        /// </value>
        private StringTemplateGroup this[string domainName]
        {
            get
            {
                StringTemplateGroup stGroup = null;

                string templateName = string.Format("Templates/{0}", domainName);
                
                if (templateGroups.ContainsKey(templateName))
                {
                    log.DebugFormat("Re-using preloaded template: {0}", templateName);
                    stGroup = templateGroups[templateName];
                }
                else
                {
                    log.DebugFormat("Loading and caching template: {0}", templateName);
                    stGroup = StringTemplateGroup.LoadGroup(templateName, baseTemplateGroup);
                    templateGroups[templateName] = stGroup;
                }

                return stGroup;
            }
        }

        /// <summary>
        /// Renders the specified domain object using the specified template.
        /// </summary>
        /// <param name="domain">
        /// Reference to the domain object to render.
        /// </param>
        /// <param name="type">
        /// The name of the macro to render the object.
        /// </param>
        /// <returns>
        /// The rendered text.
        /// </returns>
        public string RenderObject(Domain domain, string type)
        {
            StringTemplateGroup stGroup = this[domain.GetType().Name];

            StringTemplate st = stGroup.GetInstanceOf(type);

            st.RegisterAttributeRenderer(typeof(DateTime), dateRenderer);

            st.SetAttribute("domain", domain);

            return st.ToString();
        }
    }
}
