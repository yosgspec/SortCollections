"use strict";
import * as os from "os";

export class OS{
	public static readonly platform:string=process.platform;
	public static readonly osName:string=os.type().toString();
	public static readonly version:string=os.release();
	public static readonly fullName:string=`${OS.osName} ${OS.version}`;
	public static readonly arch:string=process.env.PROCESSOR_ARCHITEW6432||process.arch;
	public static readonly fullNameArch:string=`${OS.fullName} (${OS.arch})`;
}

export class Runtime{
	public static readonly lang:string="TypeScript";
	public static readonly runtime:string="Node.js";
	public static readonly version:string=process.version;
	public static readonly arch:string=process.arch;
	public static readonly fullName:string=`${Runtime.lang}/${Runtime.runtime} ${Runtime.version}`;
	public static readonly fullNameArch:string=`${Runtime.fullName} (${Runtime.arch})`;
}
