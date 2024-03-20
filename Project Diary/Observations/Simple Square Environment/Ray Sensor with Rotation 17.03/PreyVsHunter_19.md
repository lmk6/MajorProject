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
- Every Action: -0.5
- Target spotted: +0.055
- Distance to Target reduced: +0.055
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
Penalty for every move to encourage policy optimisation.
## Post Training Result:
Hunter seems to spot the Prey, approach it but not catch it which is a sign of a reward system exploitation.
Prey is not doing much.
### Cumulative Reward Plot:
![Cumulative Reward](CumulativeReward.png)
Reward gain is unexpected in this scenario.
### Entropy Plot:
![Policy Loss](Entropy.png)
Hunter's Entropy seems to be on the fall which is a good sign.
### Value Loss Plot
![Value Loss](ValueLoss.png)
*How well the model is able to predict the value of each state - this should increase while learning and then decrease once stabilised. [Documentation Reference](<https://unity-technologies.github.io/ml-agents/Using-Tensorboard/#:~:text=Losses/Value%20Loss%20(PPO%3B,decrease%20once%20the%20reward%20stabilizes.>)* 
It may indicate that the training was too short for Hunter as the Value Loss was on the fall.

## Final Observations:
Agent is clearly exploiting a faulty reward system.
