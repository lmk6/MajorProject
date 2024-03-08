## Environment
The environment is a 50x50 Unity3D Plane object surrounded by collidable walls.
## Neural Network:
- 256 neurons per layer
- 3 layers
## Observation Space (5):
- Position (x, y, z)
- Ray Sensor's angle
- Is target spotted (True or False)
## Continuous Actions:
- Move forward and backwards
- Rotate to left and right
- Rotate Ray Sensor up and down
## Hunter Reward System:
- Distance to the last farthest position below 1m:  -0.05
- Target spotted: +0.055
- Falling off the map (y < 0): -100
- collision with wall: -100
- Collision with Prey: +50
## Prey Reward System:
- Falling off the map (y < 0): -100
- collision with wall: -100
- Collision with Hunter: -50
## Rewards Rationale
Increased the penalty for touching the wall to reduce the occurrence.
Increased the Hunter's reward for spotting the agent.

## Training Observations:
***Run two similarly configured trainings, one with normalised observation vectors.***
### Computational cost
The training lasted about 12 minutes and 38 seconds for non-normalised observations and 12 minutes and 18 seconds for normalised observations.
Normalisation slightly improved the training time.

## Post Training Result:
### Not normalised:
**Hunter** agent remains chaotic in actions it takes, rarely approaches the Prey. I noticed a small improvement in spotting the Prey.
**Prey** shows very small movement.
### Normalised:
**Hunter** agent is still acting chaotically but also a lot more dynamically, it moves around with a higher confidence but usually walks into the wall backwards.
**Prey** displays the exact same behaviour as before.
### Cumulative Reward Plot:
![Cumulative Reward](CumulativeReward.png)
*Pink represents Prey and Cyan represents Hunter*
### Policy Loss Plot:
![Policy Loss](PolicyLoss.png)
*Policy Loss shows how much the process for choosing actions (policy) is changing. [Documentation Reference](<https://unity-technologies.github.io/ml-agents/Using-Tensorboard/#:~:text=Losses%2FPolicy%20Loss%20(PPO%3B,of%20the%20value%20function%20update.>)*

Both agents seem to have a fairly similar policy loss. The values are still low, between ~0.021 and ~0.027, where 1 is the maximum.
### Value Loss Plot
![Value Loss](ValueLoss.png)
*How well the model is able to predict the value of each state - this should increase while learning and then decrease once stabilised. [Documentation Reference](<https://unity-technologies.github.io/ml-agents/Using-Tensorboard/#:~:text=Losses/Value%20Loss%20(PPO%3B,decrease%20once%20the%20reward%20stabilizes.>)* 

Prey Agent is very good at predicting what reward each state will result in. Hunter Agent is bad at predicting the rewards, which is visible during the run of the trained model. Maybe it requires more observations to make some logic of it.

## Final Observations:
Penalising staying within the same area for too long causes the Hunter Agent to be more active, and it is more successful. However, it seems like the agent requires more time (episodes) to be trained properly as the cumulative reward kept raising, though, not as much towards the end which could prognose a failed model. Hunter does not seem to remember where it spotted walls and once the walls are not hit by the agent's rays, it just falls into one.
Prey Agent does exactly what is expected given the above. Despite moving within close proximity to itself, Hunter rarely seem to approach Prey letting it win, so no strategy change is required.
