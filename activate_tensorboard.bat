@echo off
call conda activate mlagents
start cmd.exe /k tensorboard --logdir results --port 6006
