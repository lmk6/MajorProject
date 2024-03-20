## Environment
The environment is a 50x50 Unity3D Plane object surrounded by collidable walls.
## Neural Network:
- 256 neurons per layer
- 3 layers
## Observation Space (Pray: 5, Predator: 7):
- Position (x, y, z)
- Ray Sensor's angle
- Is target spotted (True or False)
### Predator-specific Observation:
- Distance to target - only updated when target hit by ray
- Track of the hunger
## Continuous Actions:
- Move forward and backwards
- Rotate to left and right
- Rotate Ray Sensor up and down
## Hunter Reward System:
- Hunger Increased every second: -1
- Target spotted the first time: 15
- Distance to Target reduced since last closest distance: +0.05
- Reaching maximum on-hunger time: -50
- Falling off the map (y < 0): -100
- collision with wall: -100
- Collision with Prey: +50
## Prey Reward System:
- Falling off the map (y < 0): -100
- collision with wall: -100
- Collision with Hunter: -50
- Surviving: +50
## Rewards Rationale
Hunter will receive a single medium reward for spotting the target to encourage the search for it and not cause the overuse. 
Hunger increased penalty is to encourage more decisive action, connect the hunger increase with reward decrease. 
Reduced Distance to Target is being awarded from the achieved minimum to discourage approaching and leaving the target.
### Computational cost
To see the trainings 'tipping point' when the model begins to stagnate, I allowed for 2 million steps of training for model #28. Training time reached around 35 min, which is not bad given the increase.

## Post Training Result (#28):
Hunter searches for the Prey and gets close upon spotting it. States are more distinguishable now, Hunter clearly tries to spot Prey at the very beginning and then narrows the search area. It also moves more dynamically around the environment and very rarely walks into the wall, usually stopping right before.

I noticed Prey sometimes dodging the 'catch' from Hunter which is quite interesting but I am not sure whether intentional
### Cumulative Reward Plot:
![Cumulative Reward](CumulativeReward.png)
There is a visible snap (#28) around the millionth step when the cumulative reward start stabilising but is still on the rise (for Hunter) till the very end.
### Entropy Plot:
![Policy Loss](Entropy.png)
Model #28 seems to be on the fall which is a good sign.
### Value Loss Plot
![Value Loss](ValueLoss.png)
*How well the model is able to predict the value of each state - this should increase while learning and then decrease once stabilised. [Documentation Reference](<https://unity-technologies.github.io/ml-agents/Using-Tensorboard/#:~:text=Losses/Value%20Loss%20(PPO%3B,decrease%20once%20the%20reward%20stabilizes.>)* 
The value loss is on the rise which needs to be investigated upon and tackled.

## Final Observations:
The Rewards System Overhaul changed quite a bit and improved the behaviour of the Hunter Agent quite a lot. Now it might be a good time to tweak and test the hyperparameters.