Walls are too high, 
Need to collect the number of prey and hunter fails.
Each reward (state) should be collected.
## PreyVsHunter_29
### Hyperparameters (changed):
- Learning Rate = 0.03
### Result
Complete failure, Agents did not learn anything. Entropy is ridiculously high.
## PreyVsHunter_30 - repeats PreyVsHunter_28
## PreyVsHunter_31
### Hyperparameters (changed):
- Learning Rate = 0.003
- Epsilon = 0.02
### Result
Better than model #29 but still did not learn anything.
## PreyVsHunter_32
### Hyperparameters (changed):
- Learning Rate = 0.003
- Epsilon = 0.6
### Result
Similar, maybe slightly worse than #31.
Policy Loss for Prey Agent grows very fast.
## PreyVsHunter_33
### Hyperparameters (changed):
- Learning Rate = 0.003
- Epochs = 6
### Result
Sudden spike in Cumulative Reward at the beginning but it fails shortly after.
Model does not achieve anything and took longer to train.
## PreyVsHunter_34
### Hyperparameters (changed):
- Learning Rate = 0.003
### Result
Model learnt well, however not as completely (speaking mainly of Hunter) as model #28.
It sems like the model achieved a better cumulative reward at the beginning but then slowed down in learning quite a bit.
## Cumulative Reward
![Cumulative Reward](CumulativeReward_29-34.png)
Increasing the Learning Rate, changing epsilon and increasing number of Epochs does not give promising results.
## Entropy
![Entropy](Entropy_29-34.png)
Increasing learning rate to 0.03 caused the entropy to sky-rocket compared to other models.

## PreyVsHunter_35
### Hyperparameters (changed):
- Learning Rate = 0.0001
### Result
Cumulative Reward for Hunter grows faster at start but eventually equates to model #34.
Learning seems a bit more stable.
## PreyVsHunter_36 - repeat
### Hyperparameters (changed):
- Learning Rate = 0.0001
### Result
Nearly the same but shows how randomness can impact the training.
## PreyVsHunter_37 - repeat
### Hyperparameters (changed):
- Learning Rate = 0.0001

## Cumulative Reward 34...37
![Cumulative Reward](CumulativeReward_34-37.png)
## Entropy 34...37
![Epsilon](Entropy_34-37.png)
### Result
*Issue found: experiment 37 conducted on a different device, hence nearly doubled learning time*
Change in Learning Rate shows not much change in the Cumulative Reward but seems to take up less resources given the shorter training time (#35-36).
There is, however, a notable difference in Entropy; the higher learning rate significantly drops the epsilon / randomness.