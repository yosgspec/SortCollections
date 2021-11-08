using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XorShifts;

namespace Sorts{
	public class Sort{
		static XorShift xs=new XorShift.defaultSeed();

		public static Dictionary<string,Func<int[],int[]>> collection{get{
			var sorts=new List<Func<int[],int[]>>{
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
			};
			return sorts.ToDictionary(fn=>fn.Method.Name);
		}}

		public static Dictionary<string,Func<int[],Task<int[]>>> collectionAsync{get{
			var sorts=Sort.collection.ToDictionary(v=>v.Key,v=>
				new Func<int[],Task<int[]>>(l=>Task.Run(()=>v.Value(l))));
			sorts.Add(nameof(Sort.sleep),Sort.sleep);
			return sorts;
		}}

		public static int[] native(int[] lst0){
			var lst=(int[])lst0.Clone();
			Array.Sort(lst);
			return lst;
		}

		public static int[] bubble(int[] lst0){
			var lst=(int[])lst0.Clone();
			for(int i=0;i<lst.Length-1;i++)
				for(int n=1;n<lst.Length-i;n++)
					if(lst[n]<lst[n-1])
						(lst[n-1],lst[n])=(lst[n],lst[n-1]);
			return lst;
		}

		public static int[] select(int[] lst0){
			var lst=(int[])lst0.Clone();
			for(int i=0;i<lst.Length-1;i++)
				for(int n=i+1;n<lst.Length;n++)
					if(lst[n]<lst[i])
						(lst[i],lst[n])=(lst[n],lst[i]);
			return lst;
		}

		public static int[] insert(int[] lst0){
			var lst=(int[])lst0.Clone();
			for(int i=1;i<lst.Length;i++)
				for(int n=0;n<i;n++)
					if(lst[i]<lst[n]){
						var lsti=lst[i];
						for(int m=i;m>n;m--)
							lst[m]=lst[m-1];
						lst[n]=lsti;
						break;
					}
			return lst;
		}

		public static int[] gnome(int[] lst0){
			var lst=(int[])lst0.Clone();
			for(int i=1;i<lst.Length;i++)
				for(int n=0;n<i;n++)
					if(lst[i]<lst[n]){
						for(int m=i;m>n;m--)
							(lst[m],lst[m-1])=(lst[m-1],lst[m]);
						break;
					}
			return lst;
		}

		public static int[] shaker(int[] lst0){
			var lst=(int[])lst0.Clone();
			var first=0;
			var last=lst.Length-1;
			var spin=true;
			while(first<last){
				if(spin){
					for(int i=first;i<last;i++)
						if(lst[i+1]<lst[i])
							(lst[i],lst[i+1])=(lst[i+1],lst[i]);
					last--;
				}
				else{
					for(int i=last;i>first;i--)
						if(lst[i]<lst[i-1])
							(lst[i],lst[i-1])=(lst[i-1],lst[i]);
					first++;
				}
				spin=!spin;
			}
			return lst;
		}

		public static int[] comb(int[] lst0){
			var lst=(int[])lst0.Clone();
			var h=lst.Length;
			bool isSwap;
			do{
				if(1<h) h=h*10/13;
				isSwap=false;
				for(int i=0;i+h<lst.Length;i++){
					if(lst[i+h]<lst[i]){
						(lst[i],lst[i+h])=(lst[i+h],lst[i]);
						isSwap=true;
					}
				}
			}while(1<h || isSwap);
			return lst;
		}

		public static int[] fshell(int[] lst0){
			var lst=(int[])lst0.Clone();
			int h=0;
			while(h<=lst.Length/9) h=3*h+1;
			while(0<h){
				for(int i=h;i<lst.Length;i++)
					for(int n=i%h;n<=i;n+=h)
						if(lst[i]<lst[n]){
							var lsti=lst[i];
							for(int m=i;m>n;m-=h)
								lst[m]=lst[m-h];
							lst[n]=lsti;
						}
				h=h/3;
			};
			return lst;
		}

		public static int[] heap(int[] lst0){
			var heap=(int[])lst0.Clone();
			var last=heap.Length-1;

			void heapify(int i){
				int left=i*2+1;
				int right=i*2+2;
				int min=i;
				if(left<=last && heap[i]<heap[left]) min=left;
				if(right<=last && heap[min]<heap[right]) min=right;
				if(min!=i){
					(heap[i],heap[min])=(heap[min],heap[i]);
					heapify(min);
				}
			}

			for(int i=(last+1)/2-1;i>=0;i--)
				heapify(i);
			while(0<=last){
				(heap[0],heap[last])=(heap[last],heap[0]);
				last--;
				heapify(0);
			}
			return heap;
		}

		public static int[] merge(int[] lst0){
			int[] merge(int[] lst){
				if(lst.Length<=1) return lst;
				var sep=lst.Length/2;
				var lstA=merge(lst[0..sep]);
				var lstB=merge(lst[sep..]);
				int a=0,b=0;
				for(int i=0;i<lst.Length;i++){
					if(lstB.Length<=b || a<lstA.Length && lstA[a]<lstB[b])
						lst[i]=lstA[a++];
					else
						lst[i]=lstB[b++];
				}
				return lst;
			}
			return merge((int[])lst0.Clone());
		}

		public static int[] quick(int[] lst0){
			var lst=(int[])lst0.Clone();
			Func<int,int,int,int> median=(x,y,z)=>
				(y<x&&x<z || z<x&&x<y)? x:
				(x<y&&y<z || z<y&&y<x)? y: z;
			void quick(int first0,int last0){
				var pivot=median(lst[first0],lst[last0],lst[(first0+last0)/2]);
				int first=first0,last=last0;
				for(;;){
					while(lst[first]<pivot) first++;
					while(pivot<lst[last]) last--;
					if(last<=first) break;
					(lst[first],lst[last])=(lst[last],lst[first]);
					first++;last--;
				}
				if(first0<first-1) quick(first0,first-1);
				if(last+1<last0) quick(last+1,last0);
			}
			quick(0,lst0.Length-1);
			return lst;
		}

		public static int[] bucket(int[] lst0){
			var offset=lst0.Min();
			if(0<=offset) offset=0;
			var bucketMax=lst0.Max()+1-offset;
			List<int>[] bucket=Enumerable.Repeat(0,bucketMax).Select(v=>new List<int>{}).ToArray();

			foreach(var i in lst0)
				bucket[i-offset].Add(i);

			var lst=new List<int>{};
			foreach(var b in bucket)
				b.ForEach(i=>lst.Add(i));
			return lst.ToArray();
		}

		public static int[] radix(int[] lst0){
			const int digit=32;
			Func<List<int>[]> createBucket=()=>Enumerable.Repeat(0,2).Select(v=>new List<int>{}).ToArray();
			var bucket=createBucket();
			bucket[0]=lst0.ToList();

			for(int d=0;d<digit;d++){
				List<int>[] tmp=createBucket();
				foreach(var b in bucket){
					foreach(var i in b){
						tmp[Convert.ToInt32(0!=(i&(1<<d)))].Add(i);
					}
				}
				bucket=tmp;
			}

			Array.Reverse(bucket);
			var lst=new List<int>{};
			foreach(var b in bucket)
				b.ForEach(i=>lst.Add(i));
			return lst.ToArray();
		}

		public static int[] pigeon(int[] lst0){
			var offset=lst0.Min();
			if(0<=offset) offset=0;
			var pigeonMax=lst0.Max()+1-offset;
			int[] pigeon=Enumerable.Repeat(0,pigeonMax).ToArray();

			foreach(var i in lst0)
				pigeon[i-offset]++;

			var lst=new List<int>{};
			for(int i=0;i<pigeonMax;i++)
				for(int n=0;n<pigeon[i];n++) lst.Add(i+offset);
			return lst.ToArray();
		}

		public static int[] bogo(int[] lst0){
			for(;;){restart:
				var lst=xs.shuffle(lst0);
				for(int i=0;i<lst0.Length-1;i++){
					if(lst[i+1]<lst[i]) goto restart;
				}
				return lst;
			}
		}

		public static async Task<int[]> sleep(int[] lst0){
			int ioMin;
			ThreadPool.GetMinThreads(out _,out ioMin);
			ThreadPool.SetMinThreads(lst0.Length,ioMin);
			var offset=lst0.Min();
			if(0<=offset) offset=0;
			var lst=new List<int>{};
			var lstT=lst0.Select(async i=>{
				await Task.Delay((i-offset)*50);
				lst.Add(i);
			});
			await Task.WhenAll(lstT);
			return lst.ToArray();
		}
	}
}
