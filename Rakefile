# -*- Mode: ruby -*-
#
# Rakefile
#
# This rake file understands how to build/clean the PropertyManager project.
#
PROJECTS = [ "ControlWrappers",
             "CronUtils",
             "DAOCore",
             "DomainCore",
             "DynPropertyDomain",
             "PropertyManager",
             "STUtils"
           ]

TARGETS = [ "deploy",
            "clean",
            "clobber",
            "test"
          ]
            

task :default => [ :deploy ]

# Define all the project dependencies
task "ControlWrappers.deploy" => [ "DomainCore.deploy", 
                                   "STUtils.deploy",
                                   "CronUtils.deploy"]

task "DAOCore.deploy" => [ "DomainCore.deploy" ]

task "DynPropertyDomain.deploy" => [ "DomainCore.deploy",
                                     "DAOCore.deploy",
                                     "CronUtils.deploy" ]

task "PropertyManager.deploy" => [ "DomainCore.deploy",
                                   "DAOCore.deploy",
                                   "STUtils.deploy",
                                   "ControlWrappers.deploy",
                                   "DynPropertyDomain.deploy",
                                   "CronUtils.deploy" ]

task :deploy => [ "PropertyManager.deploy" ]

task :test => PROJECTS.collect { |p| "#{p}.test" }

task :clean => PROJECTS.collect { |p| "#{p}.clean" }

task :clobber => PROJECTS.collect { |p| "#{p}.clobber" }

#
# Invokes rake in the specified directory with the specified
# target
#
def invokeRake(directory, target)
  cmd = "(cd #{directory}; rake #{target})"
  # print "#{cmd}\n"
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
