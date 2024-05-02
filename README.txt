# MajorProject
Major Project - Machine Learning Agents in Unity3D - Research.

## Project Diary
Set of notes divided in two sections

### Weekly
Collection of notes displaying the weekly progress of the development.

### Observations
Collection of notes describing the observed behaviour and efficiency of the trained models. They also contain information on what changes were made to the environment and hyperparameters before conducting the experiment.


## MMP_ML_agents
Unity project containing:
- Code for controlling agents (PreyController.cs, HunterController.cs)
- Code for spawning algorithm (SpawnController.cs)
- Code for managing the scene's cameras (CameraManager.cs)
- Prefabs:
    - Simple and Realistic environments,
    - Models of Hunter and Prey agents.

## Builds
Builds are Unity WebGL applications using the trained model.
Read Builds/README.md for instructions on how to run them.

## Results
Contains all of the trained models for application and view of the training summary in TensorBoard.

## Statistical Tests
Contains a statistical script created for comparison of the results of repeated experiment (e.g., Cumulative Reward) to determine whether the experiment is replicable.
Read Statistical Tests/README.md for instructions.

## Scripts:
- activate_env.bat:
    - Activates Conda virtual environment and runs:
        - TensorBoard in a separate console terminal.
        - run_training.py, which activates the ML Agents and creates a new folder in *results*.
    - After running the script, open and run your model in Unity Engine to commence the training

- activate_tensorboard.bat:
    - Runs TensorBoard, you can access it by opening your browser at localhost:6006

- initialise_conda_environment.bat:
    - If you have conda installed, you can run this script to quickly setup the environment and download all the required python dependencies.
- cuda_test.py:
    - Quick test to check if your system supports CUDA for training.