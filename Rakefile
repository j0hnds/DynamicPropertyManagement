# -*- Mode: ruby -*-
#
# Rakefile
#
# This rake file understands how to build/clean the PropertyManager project.
#
require 'rake/clean'

# The list of projects that contribute to the overall application.
PROJECTS = [ "ControlWrappers",
             "CronUtils",
             "DAOCore",
             "DomainCore",
             "DynPropertyDomain",
             "PropertyManager",
             "STUtils"
           ]

# The list of targets supported by each of the project builds.
TARGETS = [ "deploy",
            "clean",
            "clobber",
            "test"
          ]

# The directory where artifacts will be placed after the builds are
# complete
LIB_DIR = 'lib'

# The directory where the package will be placed
PACKAGE_DIR = 'package'

# The name of the package (not counting extension)
PACKAGE_NAME = 'PropertyManager'

# When asked to clobber, remove the lib and the package directories.
CLOBBER.include(PACKAGE_DIR,LIB_DIR)

# The debug flag for the compiler
DEBUG_FLAG = true
            
# Define the default task to perform a deploy.
task :default => [ :deploy ]

# Make sure that the lib directory is where it needs to be
task :deploy_init do
  mkdir_p LIB_DIR if not File.exists?(LIB_DIR)
end

# Package the final result
task :package => [ :init_package, :deploy ] do
  print "#######################################################\n"
  pkdir = "#{PACKAGE_DIR}/#{PACKAGE_NAME}"
  # Create the package directory
  mkdir_p pkdir if not File.exists?(pkdir)
  tarball = "#{PACKAGE_DIR}/#{PACKAGE_NAME}.tar.gz"
  # Remove the tarball if it already exists
  rm tarball if not File.exists?(tarball)
  # Copy the lib files to underneath the package directory. This 
  # provides the directory structure we want for the tar ball.
  sh "cp -r #{LIB_DIR}/* #{pkdir}"
  # Tar up the distribution into the appropriate structure.
  tar_cmd = "tar czf #{tarball}"
  tar_cmd << " --directory #{PACKAGE_DIR}"
  tar_cmd << " #{PACKAGE_NAME}"
  sh tar_cmd
end

# Make sure that the package directory is ready to go.
task :init_package do
  mkdir_p PACKAGE_DIR if not File.exists?(PACKAGE_DIR)
end

# Define all the project dependencies
task "ControlWrappers.deploy" => [ :deploy_init,
                                   "DomainCore.deploy", 
                                   "STUtils.deploy",
                                   "CronUtils.deploy"]

task "DAOCore.deploy" => [ :deploy_init,
                           "DomainCore.deploy" ]

task "DynPropertyDomain.deploy" => [ :deploy_init,
                                     "DomainCore.deploy",
                                     "DAOCore.deploy",
                                     "CronUtils.deploy" ]

task "PropertyManager.deploy" => [ :deploy_init,
                                   "DomainCore.deploy",
                                   "DAOCore.deploy",
                                   "STUtils.deploy",
                                   "ControlWrappers.deploy",
                                   "DynPropertyDomain.deploy",
                                   "CronUtils.deploy" ]
task "DomainCore.deploy" => [ :deploy_init ]

task "STUtils.deploy" => [ :deploy_init ]

task "CronUtils.deploy" => [ :deploy_init ]

# The deploy target will build the PropertyManager. The dependencies
# defined above will determine what else has to be built.
task :deploy => [ "PropertyManager.deploy" ]

# Define the dependencies for the test task. This will ensure that
# the test task is run for all projects.
task :test => PROJECTS.collect { |p| "#{p}.test" }

# Define the dependencies for the clean task. This will ensure that
# the clean task is run for all projects.
task :clean => PROJECTS.collect { |p| "#{p}.clean" }

# Define the dependencies for the clobber task. This will ensure that
# the clobber task is run for all projects.
task :clobber => PROJECTS.collect { |p| "#{p}.clobber" }

#
# Invokes rake in the specified directory with the specified
# target
#
def invokeRake(directory, target)
  cmd = "(cd #{directory}; rake #{target} LIB_DIR=../#{LIB_DIR} DEBUG_FLAG=#{DEBUG_FLAG})"
  sh cmd
end

#
# Constructs a new task for the project/target combination.
# The target will invoke a rake target on the subproject
#
def createTask(project, target)
  task "#{project}.#{target}" do
    print "#######################################################\n"
    invokeRake project, target
  end
end

# Define all the project-level rake tasks
PROJECTS.each do |prj|
  TARGETS.each do |tgt|
    createTask prj, tgt
  end
end
