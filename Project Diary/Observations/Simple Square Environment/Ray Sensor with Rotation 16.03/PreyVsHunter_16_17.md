## Environment
The environment is a 50x50 Unity3D Plane object surrounded by collidable walls.
## Hyperparameters:
- Learning Rate: 0.003 (x10)
- Epsilon: 0.02 (x0.1)
- Steps: 
	- #16: 750000
	- #17: 1750000
## Neural Network:
- 256 neurons per layer
- 3 layers
## Observation Space (Pray: 5, Predator: 6):
- Position (x, y, z)
- Ray Sensor's angle
- Is target spotted (True or False)
### Predator-specific Observation:
- Distance to target - only updated when target hit by ray
## Continuous Actions:
- Move forward and backwards
- Rotate to left and right
- Rotate Ray Sensor up and down
## Hunter Reward System:
- Target spotted: +0.055
- Distance to Target reduced: +0.055
- Falling off the map (y < 0): -100
- collision with wall: -100
- Collision with Prey: +50
## Prey Reward System:
- Falling off the map (y < 0): -100
- collision with wall: -100
- Collision with Hunter: -50
## Rewards Rationale
If Hunter starts receiving an additional reward for not only spotting the prey but also for knowing it is getting closer to it, it will likely start approaching.
## Training Observations:
*Model #15 is used as a point of reference (the most successful so far).*
It seems that increasing the learning rate and reducing the epsilon only slowed down the progress.
Agents seem to behave the same as in the previous models.
## Post Training Result:
The behaviour seems the same as before for model #17, model #16 seems to have not enough time to train.
### Cumulative Reward Plot:
![Cumulative Reward](CumulativeReward.png)
Clearly, there is little improvement in the cumulative reward for the models with changed hyperparameters. These models seem to train at a slower pace.
### Entropy Plot:
![Policy Loss](Entropy.png)
Interestingly enough, model #17 seems to have an improved entropy compared to other models.
### Value Loss Plot
![Value Loss](ValueLoss.png)
*How well the model is able to predict the value of each state - this should increase while learning and then decrease once stabilised. [Documentation Reference](<https://unity-technologies.github.io/ml-agents/Using-Tensorboard/#:~:text=Losses/Value%20Loss%20(PPO%3B,decrease%20once%20the%20reward%20stabilizes.>)* 
Eventually, The results get similar.

## Final Observations:
Changing both learning rate and epsilon caused no change in a longer run but a visibly slower training. More focus should be put to the rewards for now, as well as testing any change in parameters individually.
