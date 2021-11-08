from os import path

recorder="./record.csv"
titles="RUNTIMES, TIMES, MS, TIMES/S, N, COMPLITE,\n"

class SortRecorder:
	def __init__(self,runtimes,platform,sortName,times,ms,n,complited):
		self.runtimes=runtimes
		self.platform=platform
		self.sortName=sortName
		self.times=times
		self.ms=ms
		self.timesps=times/ms*1E3
		self.n=n
		self.complited=complited

	def display(self):
		print(f"""{
			self.sortName.ljust(7)}: {
			str(self.times).rjust(6)}times / {
			str(self.ms).rjust(5)}ms -> {
			self.timesps:7.1f}times/s N={
			str(self.n).ljust(5)} {
			"OK!" if self.complited else "Wrong..."}""")

	@staticmethod
	def recordsAll(records): #records -> List[SortRecorder]
		if not path.isfile(recorder):
			with open(recorder,"w") as f:
				f.write(titles)

		with open(recorder,"a") as f:
			for r in records:
				f.write(f"""{
					r.runtimes}, {
					r.platform}, {
					r.sortName}, {
					r.times}, {
					r.ms}, {
					r.timesps}, {
					r.complited}\n""")
