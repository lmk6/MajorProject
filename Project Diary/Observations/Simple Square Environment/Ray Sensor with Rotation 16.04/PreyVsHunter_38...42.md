# Changes in the configuration file (38-42)
## PreyVsHunter_38
### Hyperparameters:
- batch size increased to 2048
- buffer size increased to 20480
## PreyVsHunter_39
### Hyperparameters:
- number of epochs increased to 5
## PreyVsHunter_40
### Hyperparameters:
- number of epochs decreased to 4
#### Neural Network:
- layers increased to 4
- hidden units decreased to 128
## PreyVsHunter_41
### Hyperparameters:
 - number of epochs decreased back to 3
#### Neural Network:
- number of layers decreased to 3
- number of hidden units increased back to 256
## PreyVsHunter_42
- Repeat - gives similar result
## Summary
Increasing number of epochs and doubling the batch and buffer size seems to be the most successful. Other changes only prolonged the training.
## Post Training Results and Comparison
![Cumulative Reward](CumulativeReward.png)
The chart backs the summary comment, scenario #39 achieves the highest cumulative reward at the earliest and finished with the highest success for Hunter Agent.

![Entropy](Entropy.png) Scenario #39 ends with the lowest entropy, which is still relatively high.