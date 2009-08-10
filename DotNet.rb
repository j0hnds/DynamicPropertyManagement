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

class NUnitConsole < BaseCommand

  def initialize assembly
    @assembly = assembly
  end

  def to_s
    "nunit-console #{@assembly}"
  end
end
