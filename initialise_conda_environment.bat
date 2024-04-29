set ENV_NAME=mlagents
set REQS=requirements.txt

conda create -n %ENV_NAME% --file %REQS%
