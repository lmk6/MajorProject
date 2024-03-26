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
## PreyVsHunter_37
### Hyperparameters (changed):
- Learning Rate = 0.0001
### Result
// to be analysed further