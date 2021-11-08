using System;
using System.Runtime.InteropServices;

namespace GetPlatform{
	class OS{
		public static readonly string platform=Environment.OSVersion.Platform.ToString();
		public static readonly string version=Environment.OSVersion.Version.ToString();
		public static readonly string fullName=Environment.OSVersion.VersionString;
		public static readonly string osName=OS.fullName.Substring(0,OS.fullName.IndexOf(OS.version)).Trim();
		public static readonly string arch=RuntimeInformation.OSArchitecture.ToString();
		public static readonly string fullNameArch=$"{OS.fullName} ({OS.arch})";
	}

	class Runtime{
		private static string rf=RuntimeInformation.FrameworkDescription;
		public const string lang="C#";
		public static readonly string runtime=rf.Substring(0,rf.LastIndexOf(" ")).Trim();
		public static readonly string version=rf.Substring(rf.LastIndexOf(" ")).Trim();
		public static readonly string fullName=$"{Runtime.lang}/{Runtime.rf}";
		public static readonly string arch=RuntimeInformation.ProcessArchitecture.ToString();
		public static readonly string fullNameArch=$"{Runtime.fullName} ({Runtime.arch})";
	}
}