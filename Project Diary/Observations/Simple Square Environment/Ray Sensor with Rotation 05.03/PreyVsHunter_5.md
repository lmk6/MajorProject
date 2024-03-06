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
- Every Action: -0.05
- Target spotted: +0.05
- Falling off the map (y < 0): -100
- collision with wall: -50
- Collision with Prey: +50
## Prey Reward System:
- Falling off the map (y < 0): -100
- collision with wall: -50
- Collision with Hunter: -50
## Rewards Rationale
Hunter receives a penalty for every action to encourage a more decisive action.
Hunter receives a small reward for every time it detects prey to encourage keeping the right ray sensor angle.
Falling off the map gives twice the penalty to radically discourage such behaviour - better to loose than fall.

## Training Observations:
Both agents rotate around themselves with little movement. Any contact, if happens, is caused by accident rather than by a developed strategy.

### Computational cost
The training lasted about 13 minutes and 29 seconds, so not even close to how long it took to train the similar model, with actually less observations, in the realistic environment. 
It was possible to take visual observations as the image was smooth enough.

## Post Training Result:
**Hunter** agent rotates around itself and moves its ray sensor up and down. It shows only small interest with spotting the Prey agent. It definitely shows more active and diverse movement than the Prey Agent due to the reward for spotting it. However, it did not develop a strategy of at least keeping the Prey in sight.

**Prey** agent rotates around itself and usually looks to the ground, a behaviour I find hard to explain.
### Cumulative Reward Plot:
![Cumulative Reward](CumulativeReward.png)
*Orange represents Prey and Green represents Hunter*

Given that Prey Agent receives a reward for basically surviving long enough, it is no surprise that its result is a lot better than the Hunter's. It kept getting punished for colliding with the wall it could not see and for, most likely, spawning too close to the Hunter Agent.

Hunter Agent finally shows some kind of progress here, however, it is very unimpressive how long it takes to increase the reward gain. I suspect the reward might be too low for looking at the Prey Agent.
### Policy Loss Plot:
![Policy Loss](PolicyLoss.png)
*Policy Loss shows how much the process for choosing actions (policy) is changing. [Documentation Reference](<https://unity-technologies.github.io/ml-agents/Using-Tensorboard/#:~:text=Losses%2FPolicy%20Loss%20(PPO%3B,of%20the%20value%20function%20update.>)*

The policy loss is quite interesting here. Hunter Agents seems to give up on experimenting closer to the end and Prey Agent is supposedly trying new actions a bit more frequently. It is still worth noting that the policy loss (change in behaviour) here is small in general, only between ~0.021 and ~0.027, where 1 is the maximum.
### Value Loss Plot
![Value Loss](ValueLoss.png)
*How well the model is able to predict the value of each state - this should increase while learning and then decrease once stabilised. [Documentation Reference](<https://unity-technologies.github.io/ml-agents/Using-Tensorboard/#:~:text=Losses/Value%20Loss%20(PPO%3B,decrease%20once%20the%20reward%20stabilizes.>)* 

Prey Agent seems to slowly get better at predicting the value of each state. The opposite can be said about the Hunter Agent which, from around 250000th step, increases the value loss almost linearly.

## Final Observations:
Change in the reward system is possibly too small, but it is still curious to me that the Hunter agent did not learn to at least keep the rays hitting the Prey agent.
Perhaps an increase in the number of neurons is needed to help the agent "connect the dots".
Prey Agent does not need to take any action if Hunter failed to realise it needs to catch it so there is not much to be said here. 
Unless the reward system will push the Hunter towards the Prey, there will be very little change in their behaviour.