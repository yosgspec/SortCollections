from typing import *
import asyncio
import time
from XorShift import XorShift
from SortCollection import Sort
from GetPlatform import OS,Runtime
from SortRecorder import SortRecorder

async def main():
	print(
f"""
----------------Sort Collections Benchmark-----------------
**RUNTIMES** {Runtime.fullNameArch()}
**PLATFORM** {OS.fullNameArch()}
-----------------------------------------------------------""")
	msLimit=lambda:5000
	nSwitch:Dict[str,int]={
		"*": 10000,
		"bogo": 10,
		"sleep": 100
	}
	records:List[SortRecorder]=[]

	for name,sort in Sort.collectionAsync().items():
		n=nSwitch[name if name in nSwitch else "*"]
		lst:List[int]=list(range(-n//2,n-n//2))
		lstAns=lst.copy()
		xs=XorShift.defaultSeed()
		lst=xs.shuffle(lst)
		times=0
		ms=0
		lstSorted:List[int]=[]
		sw=time.perf_counter()
		while ms<msLimit():
			lstSorted=await sort(lst)
			ms=int((time.perf_counter()-sw)*1E3)
			times+=1
		complited:bool=lstAns==lstSorted
		record=SortRecorder(
			Runtime.fullNameArch(),OS.fullNameArch(),name,times,ms,n,complited)
		record.display()
		records.append(record)

	SortRecorder.recordsAll(records)
	print(
"""-----------------------------------------------------------
DONE.""")

if __name__=="__main__":
	asyncio.run(main())
