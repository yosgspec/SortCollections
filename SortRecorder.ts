"use strict";
import {promises as fs,existsSync} from "fs";

const recorder:string="./record.csv";
const titles:string="RUNTIMES, PLATFORM, TIMES, MS, TIMES/S, N, COMPLITE,\n";

export class SortRecorder{
	public readonly runtimes:string;
	public readonly platform:string;
	public readonly sortName:string;
	public readonly times:number;
	public readonly ms:number;
	public readonly timesps:number;
	public readonly n:number;
	public readonly complited:boolean;

	constructor(runtimes:string,platform:string,sortName:string,times:number,ms:number,n:number,complited:boolean){
		this.runtimes=runtimes;
		this.platform=platform;
		this.sortName=sortName;
		this.times=0|times;
		this.ms=0|ms;
		this.timesps=times/ms*1E3;
		this.n=0|n;
		this.complited=complited;
	}

	public display(){
		console.log(`${
			this.sortName.padEnd(7)}: ${
			(""+this.times).padStart(6)}times / ${
			(""+this.ms).padStart(5)}ms -> ${
			this.timesps.toFixed(1).padStart(7)}times/s N=${
			(""+this.n).padEnd(5)} ${
			this.complited? "OK!": "Wrong..."}`);
	}

	public static async recordsAll(records:SortRecorder[]){
		try{
			await fs.lstat(recorder);
		}
		catch{
			await fs.writeFile(recorder,titles);
		}
		fs.open(recorder,"a").then(f=>{
			for(let r of records){
				f.write(`${
					r.runtimes}, ${
					r.platform}, ${
					r.sortName}, ${
					r.times}, ${
					r.ms}, ${
					r.timesps}, ${
					r.complited}`+"\n");
			}
			f.close();
		});
	}
}
