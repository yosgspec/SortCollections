"use strict";
const {setTimeout}=require("timers/promises");
import {XorShift} from "./XorShift";
const xs=new XorShift.defaultSeed();

export class Sort{
	public static get collection():Map<string,{(l:number[]):number[]}>{
		const sorts=[
			Sort.native,
			Sort.bubble,
			Sort.select,
			Sort.insert,
			Sort.gnome,
			Sort.shaker,
			Sort.comb,
			Sort.fshell,
			Sort.heap,
			Sort.merge,
			Sort.quick,
			Sort.bucket,
			Sort.radix,
			Sort.pigeon,
			Sort.bogo
		];
		return new Map<string,{(l:number[]):number[]}>(sorts.map(fn=>[fn.name,fn]));
	}

	public static get collectionAsync():Map<string,{(l:number[]):Promise<number[]>}>{
		const sorts=new Map<string,{(l:number[]):Promise<number[]>}>(
			[...Sort.collection].map(([name,fn])=>[name,l=>Promise.resolve(fn(l))]));
		sorts.set(Sort.sleep.name,Sort.sleep);
		return sorts;
	}

	public static native(lst0:number[]):number[]{
		return [...lst0].sort((a,b)=>a-b);
	}

	public static bubble(lst0:number[]):number[]{
		const lst=[...lst0];
		for(let i=0;i<lst.length-1;i++)
			for(let n=1;n<lst.length-i;n++)
				if(lst[n]<lst[n-1])
					[lst[n-1],lst[n]]=[lst[n],lst[n-1]];
		return lst;
	}

	public static select(lst0:number[]):number[]{
		const lst=[...lst0];
		for(let i=0;i<lst.length-1;i++)
			for(let n=i+1;n<lst.length;n++)
				if(lst[n]<lst[i])
					[lst[i],lst[n]]=[lst[n],lst[i]];
		return lst;
	}

	public static insert(lst0:number[]):number[]{
		const lst=[...lst0];
		for(let i=1;i<lst.length;i++)
			for(let n=0;n<i;n++)
				if(lst[i]<lst[n]){
					let lsti=lst[i];
					for(let m=i;m>n;m--)
						lst[m]=lst[m-1];
					lst[n]=lsti;
					break;
				}
		return lst;
	}

	public static gnome(lst0:number[]):number[]{
		const lst=[...lst0];
		for(let i=1;i<lst.length;i++)
			for(let n=0;n<i;n++)
				if(lst[i]<lst[n]){
					for(let m=i;m>n;m--)
						[lst[m],lst[m-1]]=[lst[m-1],lst[m]];
					break;
				}
		return lst;
	}

	public static shaker(lst0:number[]):number[]{
		const lst=[...lst0];
		var first=0;
		var last=lst.length-1;
		var spin=true;
		while(first<last){
			if(spin){
				for(let i=first;i<last;i++)
					if(lst[i+1]<lst[i])
						[lst[i],lst[i+1]]=[lst[i+1],lst[i]];
				last--;
			}
			else{
				for(let i=last;i>first;i--)
					if(lst[i]<lst[i-1])
						[lst[i],lst[i-1]]=[lst[i-1],lst[i]];
				first++;
			}
			spin=!spin;
		}
		return lst;
	}

	public static comb(lst0:number[]):number[]{
		const lst=[...lst0];
		var h=lst.length;
		var isSwap:boolean;
		do{
			if(1<h) h=0|h*10/13;
			isSwap=false;
			for(let i=0;i+h<lst.length;i++){
				if(lst[i+h]<lst[i]){
					[lst[i],lst[i+h]]=[lst[i+h],lst[i]];
					isSwap=true;
				}
			}
		}while(1<h || isSwap);
		return lst;
	}

	public static fshell(lst0:number[]){
		const lst=[...lst0];
		var h=0;
		while(h<=lst.length/9) h=3*h+1;
		while(0<h){
			for(let i=h;i<lst.length;i++)
				for(let n=i%h;n<=i;n+=h)
					if(lst[i]<lst[n]){
						let lsti=lst[i];
						for(let m=i;m>n;m-=h)
							lst[m]=lst[m-h];
						lst[n]=lsti;
					}
			h=0|h/3;
		};
		return lst;
	}

	public static heap(lst0:number[]):number[]{
		var heap=[...lst0];
		var last=heap.length-1;

		function heapify(i:number){
			let left=i*2+1;
			let right=i*2+2;
			let min=i;
			if(left<=last && heap[i]<heap[left]) min=left;
			if(right<=last && heap[min]<heap[right]) min=right;
			if(min!=i){
				[heap[i],heap[min]]=[heap[min],heap[i]];
				heapify(min);
			}
		}

		for(let i=0|(last+1)/2-1;i>=0;i--)
			heapify(i);
		while(0<=last){
			[heap[0],heap[last]]=[heap[last],heap[0]];
			last--;
			heapify(0);
		}
		return heap;
	}

	public static merge(lst0:number[]):number[]{
		function merge(lst:number[]):number[]{
			if(lst.length<=1) return lst;
			var sep=0|lst.length/2;
			var lstA=merge(lst.slice(0,sep));
			var lstB=merge(lst.slice(sep));
			var a=0,b=0;
			for(let i=0;i<lst.length;i++)
				if(lstB.length<=b || a<lstA.length && lstA[a]<lstB[b])
					lst[i]=lstA[a++];
				else
					lst[i]=lstB[b++];
			return lst;
		}
		return merge([...lst0]);
	}

	public static quick(lst0:number[]):number[]{
		const lst=[...lst0];
		const median:{(a:number,b:number,c:number):number}=(x,y,z)=>
			(y<x&&x<z || z<x&&x<y)? x:
			(x<y&&y<z || z<y&&y<x)? y: z;
		function quick(first0:number,last0:number){
			var pivot=median(lst[first0],lst[last0],lst[0|(first0+last0)/2]);
			var first=first0,last=last0;
			for(;;){
				while(lst[first]<pivot) first++;
				while(pivot<lst[last]) last--;
				if(last<=first) break;
				[lst[first],lst[last]]=[lst[last],lst[first]];
				first++;last--;
			}
			if(first0<first-1) quick(first0,first-1);
			if(last+1<last0) quick(last+1,last0);
		}
		quick(0,lst.length-1);
		return lst;
	}

	public static bucket(lst0:number[]):number[]{
		var offset=Math.min(...lst0);
		if(0<=offset) offset=0;
		var bucketMax=Math.max(...lst0)+1-offset;
		const bucket:number[][]=Array.from(Array(bucketMax),i=>[]);

		for(let i of lst0)
			bucket[i-offset].push(i);

		const lst:number[]=[];
		for(let b of bucket)
			b.forEach(i=>lst.push(i));
		return lst;
	}

	public static radix(lst0:number[]):number[]{
		const digit:number=32;
		const createBucket:{():number[][]}=()=>Array.from(Array(2),i=>[]);
		var bucket=createBucket();
		bucket[0]=lst0;

		for(let d=0;d<digit;d++){
			const tmp=createBucket();
			for(let b of bucket){
				for(let i of b){
					tmp[0|(0!=(i&(1<<d))) as any as number].push(i);
				}
			}
			bucket=tmp;
		}

		bucket.reverse();
		const lst:number[]=[];
		for(let b of bucket)
			b.forEach(i=>lst.push(i));
		return lst;
	}

	public static pigeon(lst0:number[]):number[]{
		var offset=Math.min(...lst0);
		if(0<=offset) offset=0;
		var pigeonMax=Math.max(...lst0)+1-offset;
		const pigeon:number[]=Array(pigeonMax).fill(0);

		for(let i of lst0)
			pigeon[i-offset]++;

		const lst:number[]=[];
		for(let i=0;i<pigeonMax;i++)
			for(let n=0;n<pigeon[i];n++) lst.push(i+offset);
		return lst;
	}

	public static bogo(lst0:number[]):number[]{
		restart:for(;;){
			const lst=xs.shuffle(lst0);
			for(let i=0;i<lst0.length-1;i++){
				if(lst[i+1]<lst[i]) continue restart;
			}
			return lst;
		}
	}

	public static async sleep(lst0:number[]):Promise<number[]>{
		var offset=Math.min(...lst0);
		if(0<=offset) offset=0;
		const lst:number[]=[];
		const lstP=lst0.map(async i=>{
			await setTimeout((i-offset)*50);
			lst.push(i);
		});
		await Promise.all(lstP);
		return lst;
	}
}
