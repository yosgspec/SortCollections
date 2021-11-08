"use strict";
import {XorShift} from "./XorShift";
import {Sort} from "./SortCollection";
import {OS,Runtime} from "./GetPlatform";
import {SortRecorder} from "./SortRecorder";


(async function(){
	console.log(`
----------------Sort Collections Benchmark-----------------
**RUNTIMES** ${Runtime.fullNameArch}
**PLATFORM** ${OS.fullNameArch}
-----------------------------------------------------------`);
	const msLimit=5000;
	const nSwitch:{[s:string]:number}={
		"*": 10000,
		"bogo": 10,
		"sleep": 100
	};
	const records:SortRecorder[]=[];

	for(let [name,sort] of Sort.collectionAsync){
		let n=nSwitch[name in nSwitch? name: "*"];
		let lst:number[]=[...Array(n)].map((_,i)=>i-(0|n/2));
		const lstAns=[...lst];
		const xs=new XorShift.defaultSeed();
		lst=xs.shuffle(lst);
		let times=0;
		let ms=0;
		let lstSorted:number[]=[];
		performance.mark("sw");
		while(ms<msLimit){
			lstSorted=await sort(lst);
			ms=0|performance.measure("","sw").duration;
			times++;
		}
		performance.clearMarks("sw");
		let complited:boolean=JSON.stringify(lstAns)==JSON.stringify(lstSorted);
		const record=new SortRecorder(
			Runtime.fullNameArch,OS.fullNameArch,name,times,ms,n,complited);
		record.display();
		records.push(record);
	}
	SortRecorder.recordsAll(records);
	console.log(
`--------------------------------------------------
DONE.`);

})();

