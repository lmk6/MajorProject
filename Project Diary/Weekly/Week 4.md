Tried alternatives for Unit VCS - Gave up due to the repo size limit (2GB)
Begun working on utilizing RayCasting in training the model to achieve a more realistic result.
Created a working model trained using Ray Sensors and more realistic motorics.
Begun experimenting with the training configuration file (YAML) - it allows to change paramaters such as number of steps, layers (NN) etc.
Spent some time testing the GPU's utilisation in training - went through TensorFlow AMD support documantation - gave up after packages' dependecies mismatch.
Tested the training time in Linux and Windows enivronments for comparison and to check if a single project can be run on a different OS - results are surprising, Win+CPU seems to be faster than GPU on Linux.
Carrying on with the research on Windows using CPU due to the apparent faster training time and ease of use (no unexpected behaviour, better support).
Modified the number of training steps to 750000 and enhanced the reward system to experiment:
	Penalty for every move - Agent became less uncertain, rushes towards the target.
	Reward for rays hitting the target- Agent cheated the reward system, got stuck in a single place looking at the target to receive the reward, reward for looking is higher than the penalty for taking an action.
Outlined the research questions.
Working on automation scripts.