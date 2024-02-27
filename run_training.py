import os
import subprocess
import re

DEFAULT_PATH_TO_RESULTS = r"C:\MajorProject\results"
DEFAULT_PATH_TO_CONFIG = r"C:\MajorProject\MP_ML_agents\Assets\TrainConfigMAgents.yaml"

def run_new_training(path_to_config=DEFAULT_PATH_TO_CONFIG):
    new_run_id = get_new_run_id()
    print(f"Running ML-Agents ==== run-id = {new_run_id}")
    process = subprocess.Popen(["mlagents-learn", path_to_config, f"--run-id={new_run_id}"],
                               stdout=subprocess.PIPE,
                               stderr=subprocess.PIPE,
                               universal_newlines=True,
                               shell=True)
    
    while process.poll() is None:
        output = process.stdout.readline().strip()
        print(output)

def get_new_run_id(): 
    lastest_run_id = get_latest_run_name()
    if lastest_run_id is None:
        return "new_test_run"
    
    pattern = r"_(\d+)$"
    match = re.search(pattern, lastest_run_id)
    if not match:
        return f"{lastest_run_id}_1"

    suffix = int(match.group(1)) + 1
    return re.sub(pattern, "_" + f"{suffix}", lastest_run_id)

def get_latest_run_name(results_dir=DEFAULT_PATH_TO_RESULTS):
    
    directories = [d for d in os.listdir(results_dir) if os.path.isdir(os.path.join(results_dir, d))]

    if not directories:
        return None

    dir_creation_times = { d:
        os.path.getctime(
            os.path.join(results_dir, d)
            ) for d in directories
        }

    newest_dir = max(dir_creation_times, key=dir_creation_times.get)

    return newest_dir
    

if __name__ == "__main__":
    run_new_training()
