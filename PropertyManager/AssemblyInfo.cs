using System.Reflection;
using System.Runtime.CompilerServices;

// Configure log4net using the .config file
[assembly: log4net.Config.XmlConfigurator(Watch=true)]
// This will cause log4net to look for a configuration file
// called *.exe.config in the application base directory.
// The config file will be watched for changes.

// Information about this assembly is defined by the following attributes. 
// Change them to the values specific to your project.

[assembly: AssemblyTitle("PropertyManager")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// The assembly version has the format "{Major}.{Minor}.{Build}.{Revision}".
// The form "{Major}.{Minor}.*" will automatically update the build and revision,
// and "{Major}.{Minor}.{Build}.*" will update just the revision.

[assembly: AssemblyVersion("1.0.*")]

// The following attributes are used to specify the signing key for the assembly, 
// if desired. See the Mono documentation for more information about signing.

[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
