using System;
using System.Collections.Generic;
using System.IO;

namespace SortRecorders{
	public class SortRecorder{
		const string recorder="./record.csv";
		const string titles="RUNTIMES, TIMES, MS, TIMES/S, N, COMPLITE,\n";

		public readonly string runtimes;
		public readonly string platform;
		public readonly string sortName;
		public readonly int times;
		public readonly int ms;
		public readonly double timesps;
		public readonly int n;
		public readonly bool complited;

		public SortRecorder(string runtimes,string platform,string sortName,int times,int ms,int n,bool complited){
			this.runtimes=runtimes;
			this.platform=platform;
			this.sortName=sortName;
			this.times=times;
			this.ms=ms;
			this.timesps=(double)times/ms*1E3;
			this.n=n;
			this.complited=complited;
		}
		public void display(){
			Console.WriteLine($@"{
				this.sortName.PadRight(7)}: {
				(""+this.times).PadLeft(6)}times / {
				(""+this.ms).PadLeft(5)}ms -> {
				this.timesps,7:f1}times/s N={
				(""+this.n).PadRight(5)} {(
				this.complited? "OK!": "Wrong...")}");
		}

		public static void recordsAll(List<SortRecorder> records){
			if(!File.Exists(recorder)){
				File.WriteAllText(recorder,titles);
			}
			using(var f=new StreamWriter(recorder,true)){
				foreach(var r in records){
					f.Write($@"{
						r.runtimes}, {
						r.platform}, {
						r.sortName}, {
						r.times}, {
						r.ms}, {
						r.timesps}, {
						r.complited}"+"\n");
				}
			}
		}
	}
}
