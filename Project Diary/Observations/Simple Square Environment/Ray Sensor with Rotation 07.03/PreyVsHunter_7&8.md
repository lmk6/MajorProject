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
![Cumulative Reward](Observations/Simple%20Square%20Environment/Ray%20Sensor%20with%20Rotation%2007.03/CumulativeReward.png)
*Non-normalised: Pink represents Hunter and Orange represents Prey
Normalised: Green represents Hunter and Purple represents Prey*
Normalised Hunter agent seems to learn at a faster rate, achieving a substantially higher reward at at the end of training.
### Policy Loss Plot:
![Policy Loss](PolicyLoss.png)
*Policy Loss shows how much the process for choosing actions (policy) is changing. [Documentation Reference](<https://unity-technologies.github.io/ml-agents/Using-Tensorboard/#:~:text=Losses%2FPolicy%20Loss%20(PPO%3B,of%20the%20value%20function%20update.>)*

Policy Loss seems to be the same for both configurations.
### Value Loss Plot
![Value Loss](Observations/Simple%20Square%20Environment/Ray%20Sensor%20with%20Rotation%2007.03/ValueLoss.png)
*How well the model is able to predict the value of each state - this should increase while learning and then decrease once stabilised. [Documentation Reference](<https://unity-technologies.github.io/ml-agents/Using-Tensorboard/#:~:text=Losses/Value%20Loss%20(PPO%3B,decrease%20once%20the%20reward%20stabilizes.>)* 

Normalised agents end up with a better (lower) value loss.

## Final Observations:
Although the change in reward did a little to the overall result, the normalisation of the observed values seems to be a viable option in the future training, providing a better stability and learning at increased pace.
