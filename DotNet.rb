# Define a class that encapsulates how to build C# .NET files

#
# Base class for command definitions
#
class BaseCommand

  #
  # Executes the command by running the shell against the string
  # version of the object.
  def exec
    sh to_s
  end

end

#
# This class encapsulates the knowledge of how to execute the
# C# .NET compiler
#
class Gmcs < BaseCommand

  attr_accessor :packages, :debug, :libs, :references

  #
  # Constructs a new Gmcs object.
  #
  # targetType - The type of object to be created (library, exe, etc.)
  # outputName - The path of the output file created.
  # sourceFiles - The array of source files to be compiled.
  #
  def initialize targetType, outputName, sourceFiles
    @targetType = targetType
    @outputName = outputName
    @sourceFiles = sourceFiles
    @packages = []
    @libs = []
    @references = []
    @debug = false
  end

  def to_s
    s = "gmcs -out:#{@outputName} -target:#{@targetType} "
    s << "-lib:#{@libs.join(',')} " if @libs.length > 0
    s << "-r:#{@references.join(',')} " if @references.length > 0
    s << "-pkg:#{@packages.join(',')} " if @packages.length > 0
    s << "-debug " if @debug
    s << @sourceFiles.join(' ')
    return s
  end

end

# 
# This class encapsulates the ability to run the NUnit Console application
#
class NUnitConsole < BaseCommand

  attr_accessor :xml

  def initialize assembly
    @assembly = assembly
    @xml = nil
  end

  def to_s
    s = "nunit-console #{@assembly}"
    s << " -nologo"
    s << " -xml=#{@xml} " unless @xml.nil?
    return s
  end
end

#
# Creates a tuple that contains the name of the build directory
# and the test directory respectively.
#
def create_output_directories(debug_flag)
  dir = (debug_flag) ? 'Debug' : 'Release'
  return "bin/#{dir}", "bin/test/#{dir}"
end
