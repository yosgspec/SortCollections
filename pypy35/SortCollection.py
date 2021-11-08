import asyncio
import random
from XorShift import XorShift
xs=XorShift.defaultSeed()

class Sort:
	def collection():
		sorts=[
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
		]
		return {fn.__name__: fn for fn in sorts}

	@staticmethod
	def collectionAsync():
		sorts={}
		def toAsync(fn):
			async def toAsync(l):
				return fn(l)
			return toAsync
		sorts={name: toAsync(fn)
			for name,fn in Sort.collection().items()}
		sorts["comb"]=toAsync(Sort.comb)
		sorts[Sort.sleep.__name__]=Sort.sleep
		return sorts

	@staticmethod
	def native(lst0):
		return sorted(lst0)

	@staticmethod
	def bubble(lst0):
		lst=lst0.copy()
		for i in range(len(lst)-1):
			for n in range(1,len(lst)-i):
				if lst[n]<lst[n-1]:
					lst[n-1],lst[n]=lst[n],lst[n-1]
		return lst

	@staticmethod
	def select(lst0):
		lst=lst0.copy()
		for i in range(len(lst)-1):
			for n in range(i+1,len(lst)):
				if lst[n]<lst[i]:
					lst[i],lst[n]=lst[n],lst[i]
		return lst

	@staticmethod
	def insert(lst0):
		lst=lst0.copy()
		for i in range(1,len(lst)):
			for n in range(i):
				if lst[i]<lst[n]:
					lsti=lst[i]
					for m in range(i,n,-1):
						lst[m]=lst[m-1]
					lst[n]=lsti
					break
		return lst

	@staticmethod
	def gnome(lst0):
		lst=lst0.copy()
		for i in range(1,len(lst)):
			for n in range(i):
				if lst[i]<lst[n]:
					for m in range(i,n,-1):
						lst[m],lst[m-1]=lst[m-1],lst[m]
					break
		return lst

	@staticmethod
	def shaker(lst0):
		lst=lst0.copy()
		first=0
		last=len(lst)-1
		spin=True
		while first<last:
			if spin:
				for i in range(first,last):
					if lst[i+1]<lst[i]:
						lst[i],lst[i+1]=lst[i+1],lst[i]
				last-=1
			else:
				for i in range(last,first,-1):
					if lst[i]<lst[i-1]:
						lst[i],lst[i-1]=lst[i-1],lst[i]
				first+=1
			spin=not spin
		return lst

	@staticmethod
	def comb(lst0):
		lst=lst0.copy()
		h=len(lst)
		isSwap:bool
		while True:
			if 1<h: h=h*10//13
			isSwap=False
			for i in range(len(lst)-h):
				if lst[i+h]<lst[i]:
					lst[i],lst[i+h]=lst[i+h],lst[i]
					isSwap=True
			if 1<h or isSwap: continue
			break
		return lst

	@staticmethod
	def fshell(lst0):
		lst=lst0.copy()
		h=0
		while h<=len(lst)/9: h=3*h+1
		while 0<h:
			for i in range(h,len(lst)):
				for n in range(i%h,i+1,h):
					if lst[i]<lst[n]:
						lsti=lst[i]
						for m in range(i,n,-h):
							lst[m]=lst[m-h]
						lst[n]=lsti
			h=h//3
		return lst

	@staticmethod
	def heap(lst0):
		heap=lst0.copy()
		last=len(heap)-1

		def heapify(i:int):
			left=i*2+1
			right=i*2+2
			min=i
			if left<=last and heap[i]<heap[left]: min=left
			if right<=last and heap[min]<heap[right]: min=right
			if min!=i:
				heap[i],heap[min]=heap[min],heap[i]
				heapify(min)

		for i in range((last+1)//2-1,-1,-1):
			heapify(i)
		while(0<=last):
			heap[0],heap[last]=heap[last],heap[0]
			last-=1
			heapify(0)
		return heap

	@staticmethod
	def merge(lst0):
		def merge(lst):
			if len(lst)<=1: return lst
			sep=len(lst)//2
			lstA=merge(lst[0:sep])
			lstB=merge(lst[sep:])
			a=0;b=0
			for i in range(len(lst)):
				if len(lstB)<=b or a<len(lstA) and lstA[a]<lstB[b]:
					lst[i]=lstA[a];a+=1
				else:
					lst[i]=lstB[b];b+=1
			return lst
		return merge(lst0.copy())

	@staticmethod
	def quick(lst0):
		lst=lst0.copy()
		median:Callable[[int,int,int],int]=(lambda x,y,z:
			x if y<x and x<z or z<x and x<y else
			y if x<y and y<z or z<y and y<x else z)
		def quick(first0:int,last0:int):
			pivot=median(lst[first0],lst[last0],lst[(first0+last0)//2])
			first=first0;last=last0
			while True:
				while lst[first]<pivot: first+=1
				while pivot<lst[last]: last-=1
				if last<=first: break
				lst[first],lst[last]=lst[last],lst[first]
				first+=1;last-=1

			if first0<first-1: quick(first0,first-1)
			if last+1<last0: quick(last+1,last0)

		quick(0,len(lst)-1)
		return lst

	@staticmethod
	def bucket(lst0):
		offset=min(lst0)
		if 0<=offset: offset=0
		bucketMax=max(lst0)+1-offset
		bucket:List[List[int]]=[[] for v in range(bucketMax)];

		for i in lst0:
			bucket[i-offset].append(i)

		lst=[];
		for b in bucket:
			for i in b: lst.append(i)
		return lst

	@staticmethod
	def radix(lst0):
		digit:Callable[[],int]=lambda:32;
		createBucket:Callable[[],List[List[int]]]=lambda:[[] for i in range(2)]
		bucket=createBucket()
		bucket[0]=lst0

		for d in range(digit()):
			tmp=createBucket()
			for b in bucket:
				for  i in b:
					tmp[int(0!=(i&(1<<d)))].append(i)
			bucket=tmp

		bucket.reverse()
		lst=[]
		for b in bucket:
			for i in b: lst.append(i)
		return lst

	@staticmethod
	def pigeon(lst0):
		offset=min(lst0)
		if 0<=offset: offset=0
		pigeonMax=max(lst0)+1-offset
		pigeon:List[List[int]]=[0]*pigeonMax;

		for i in lst0:
			pigeon[i-offset]+=1

		lst=[];
		for i in range(pigeonMax):
			for n in range(pigeon[i]): lst.append(i+offset)
		return lst

	@staticmethod
	def bogo(lst0):
		while True:
			lst=xs.shuffle(lst0)
			for i in range(len(lst0)-1):
				if lst[i+1]<lst[i]: break
			else: return lst

	@staticmethod
	async def sleep(lst0):
		offset=min(lst0)
		if 0<=offset: offset=0
		lst=[]
		async def fn(i):
			await asyncio.sleep((i-offset)*50E-3)
			lst.append(i)
		lstF=[fn(i) for i in lst0]
		await asyncio.gather(*lstF)
		return lst
