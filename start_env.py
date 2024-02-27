import subprocess

def activate_env():
    # subprocess.run(["conda", "run", "-n", "mlagents", "python", "run_training.py"], check=True)
    subprocess.Popen(["activate_env.bat"], shell=True)

    
if __name__ == "__main__":
    activate_env()
