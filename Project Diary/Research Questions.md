# 1. How a changing environment affects the behaviour of a trained model?
## Metrics:
-  Average duration.
-  Average result.
## Ways of achieving:
- Increasing the size of the environment. – already achieved.
- Changing the shape of the environment (terrain).
- Adding obstacles.
- Choose the spawn points and spawn conditions for agents.
## Steps to answer:
- Prepare various environments for testing.
- Prepare various environments for testing.
- Measure and save the times and results after a run in each changed environment.
- Compare the results and visual observations to determine how impactful the changes were, and which changes impacted the outcome the most.
# 2. How the training changes with the changing complexity of a model?
## Metrics:
- Elapsed time of the training.
- Episode length.
- Mean Reward. 
- Standard deviation of a reward. In question – Minimum number of steps required for the agent to ‘figure out’ what the goal is.

## Ways of achieving:
- Creating more reward / penalty cases – designing new reward systems.
- Creating and adding new sensors to the agents.
- Creating new attributes for the agents to choose from.
- Changing environments.
- Increasing number of neural layers and neurons.

## Steps to answer:
- Run the training for each of the **_selected_** models.
- For each training, take a note of the metrics saved on the TensorBoard.
- Compare the results and write down the observations, choose the most significant factors affecting the training and describe the change in metrics.
# 3.  Do agents appear to develop a strategy in dealing with each other?
## Metrics:
- Set of distinguishable actions taken by agents every run.
## Ways of achieving:
- Establishing repeating chain of actions taken by either of the agents.
- Establishing the significance of the found pattern.
## Steps to answer:
- Run a successful model a few times and take notes of the agents' behaviour