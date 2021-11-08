for i in {0..9};do
	npm run build
	dotnet run
	pypy3 main.py
	py -3 main.py
done
