import sys
from os import path
import platform

class OS:
	@staticmethod
	def platform(): return sys.platform
	@staticmethod
	def osName(): return platform.system()
	@staticmethod
	def version(): return platform.version()
	@staticmethod
	def fullName(): return platform.platform()
	@staticmethod
	def arch(): return platform.machine()
	@staticmethod
	def fullNameArch(): return f"{OS.fullName()} ({OS.arch()})";

class Runtime:
	@staticmethod
	def lang(): return "Python"
	@staticmethod
	def runtime(): return path.splitext(path.basename(sys.executable))[0].replace("py","Py")
	@staticmethod
	def version(): return platform.python_version()
	@staticmethod
	def fullName(): return f"{Runtime.lang()}/{Runtime.runtime()} {Runtime.version()}"
	@staticmethod
	def arch(): return platform.architecture()[0]
	@staticmethod
	def fullNameArch(): return f"{Runtime.fullName()} ({Runtime.arch()})";
