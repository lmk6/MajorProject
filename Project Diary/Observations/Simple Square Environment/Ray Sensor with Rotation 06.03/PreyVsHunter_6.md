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
- Target spotted: +0.05
- Falling off the map (y < 0): -100
- collision with wall: -50
- Collision with Prey: +50
## Prey Reward System:
- Falling off the map (y < 0): -100
- collision with wall: -50
- Collision with Hunter: -50
## Rewards Rationale
Hunter receives a penalty every time it is less than a meter away from the last farthest point to encourage movement and make it explore more.
Hunter receives a small reward for every time it detects prey to encourage keeping the right ray sensor angle.
Falling off the map gives twice the penalty to radically discourage such behaviour - better to loose than fall.

## Training Observations:
*First half similar to the usual - not much movement or colliding with the walls. The second part was not observed.*

### Computational cost
The training lasted about 12 minutes and 50 seconds.
A small improvement compared to the previous model, mostly due to the more encouraged movement.

## Post Training Result:
**Hunter** agent makes farther movement than previously and seems to be scouting the area around itself. The agent tends to fall into the wall a lot, mostly due to not scanning it first. Because of a quite chaotic movement, the prey tends to be in between the shot ray, causing the Hunter Agent to not see it. However, once spotted, Hunter seems to approach the prey a little but not fast enough to catch it before the timer ends. On occasions, when Prey is in a close proximity, Hunter manages to catch it.
**Prey** agent rotates around itself and usually looks to the ground, a behaviour I had already seen before and it is quite expected as Hunter rarely poses any threat.
### Cumulative Reward Plot:
![Cumulative Reward](CumulativeReward.png)
*Pink represents Prey and Cyan represents Hunter*

Given that Prey Agent receives a reward for basically surviving long enough, it is no surprise that its result is a lot better than the Hunter's. It kept getting punished for colliding with the wall it could not see and for, most likely, spawning too close to the Hunter Agent.

Hunter Agent seems to finally learn at a better rate, however, not fast enough. 
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
