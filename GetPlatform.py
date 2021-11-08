import sys
from os import path
import platform

class OS:
	@staticmethod
	def platform()->str: return sys.platform
	@staticmethod
	def osName()->str: return platform.system()
	@staticmethod
	def version()->str: return platform.version()
	@staticmethod
	def fullName()->str: return platform.platform()
	@staticmethod
	def arch()->str: return platform.machine()
	@staticmethod
	def fullNameArch()->str: return f"{OS.fullName()} ({OS.arch()})";

class Runtime:
	@staticmethod
	def lang()->str: return "Python"
	@staticmethod
	def runtime()->str: return path.splitext(path.basename(sys.executable))[0].replace("py","Py")
	@staticmethod
	def version()->str: return platform.python_version()
	@staticmethod
	def fullName()->str: return f"{Runtime.lang()}/{Runtime.runtime()} {Runtime.version()}"
	@staticmethod
	def arch()->str: return platform.architecture()[0]
	@staticmethod
	def fullNameArch()->str: return f"{Runtime.fullName()} ({Runtime.arch()})";
