import os
import subprocess
import re

PATH_TO_DIR = os.getcwd()
DEFAULT_PATH_TO_RESULTS = PATH_TO_DIR + r"\MajorProject\results"
DEFAULT_PATH_TO_CONFIG = PATH_TO_DIR + r"\MP_ML_agents\Assets\TrainConfigMAgents.yaml"
DEFAULT_LOG_FILENAME = "training_log.txt"

def run_new_training(path_to_config=DEFAULT_PATH_TO_CONFIG):
    new_run_id = get_new_run_id()
    captured_output = ""
    print(f"Running ML-Agents ==== run-id = {new_run_id}")
    process = subprocess.Popen(["mlagents-learn", path_to_config, f"--run-id={new_run_id}"],
                               stdout=subprocess.PIPE,
                               stderr=subprocess.PIPE,
                               universal_newlines=True,
                               shell=True)
    
    while process.poll() is None:
        output = process.stdout.readline()
        captured_output += output
        output = output.strip()
        print(output)

    with open(f"{DEFAULT_PATH_TO_RESULTS}\{new_run_id}\{DEFAULT_LOG_FILENAME}") as file:
        file.write(output)

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
