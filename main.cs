using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using XorShifts;
using Sorts;
using GetPlatform;
using SortRecorders;

class Program{
	static async Task Main(){
		Console.WriteLine($@"
----------------Sort Collections Benchmark-----------------
**RUNTIMES** {Runtime.fullNameArch}
**PLATFORM** {OS.fullNameArch}
-----------------------------------------------------------");

		var sw=new Stopwatch();
		const int msLimit=5000;
		var nSwitch=new Dictionary<string,int>{
			{"*", 10000},
			{"bogo", 10},
			{"sleep", 100}
		};
		var records=new List<SortRecorder>{};

		foreach(var (name,sort) in Sort.collectionAsync){
			int n=nSwitch[nSwitch.ContainsKey(name)? name: "*"];
			int[] lst=Enumerable.Range(-n/2,n).ToArray();
			var lstAns=(int[])lst.Clone();
			var xs=new XorShift.defaultSeed();
			lst=xs.shuffle(lst);
			int times=0;
			int ms=0;
			int[] lstSorted={};
			sw.Start();
			while(ms<msLimit){
				lstSorted=await sort(lst);
				ms=(int)sw.ElapsedMilliseconds;
				times++;
			}
			sw.Reset();
			bool complited=lstAns.SequenceEqual(lstSorted);
			var record=new SortRecorder(
				Runtime.fullNameArch,OS.fullNameArch,name,times,ms,n,complited);
			record.display();
			records.Add(record);
		}
		SortRecorder.recordsAll(records);
		Console.WriteLine(
@"-----------------------------------------------------------
DONE.");

	}
}
