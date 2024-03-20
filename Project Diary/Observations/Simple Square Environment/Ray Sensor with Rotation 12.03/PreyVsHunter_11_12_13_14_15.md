## Environment
The environment is a 50x50 Unity3D Plane object surrounded by collidable walls.
## Neural Network:
- 256 neurons per layer
- 3 layers
## Observation Space (Pray: 5, Predator: 6):
- Position (x, y, z)
- Ray Sensor's angle
- Is target spotted (True or False)
#### Stacked Observations:
- PreyVsHunter_13: 3
- PreyVsHunter_14: 10
- Others: 1
### Additional Predator Observation (only for 13-and newer):
- Distance to target - only updated when target hit by ray
## Continuous Actions:
- Move forward and backwards
- Rotate to left and right
- Rotate Ray Sensor up and down
## Hunter Reward System:
- Distance to the last farthest position below 1m:  -0.05
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
In the #15 model, I could finally see the Hunter scanning the area around it successfully. Once the Prey is spotted, Hunter rushes towards it. If the Prey is not spotted though, Hunter will usually just walk into the wall.
Other models display surprisingly little improvement, Hunter started getting more interested in spotting prey once the distance to it started being observed (#13-newer).
### Computational cost
Models with the same number of training steps seemed to have a very similar training time.
The extended (1 million steps) training for #12 model took about 17.5 minutes.
## Post Training Result:
Model #15 seems to be the most successful one, Hunter agents keeps the ray sensor at a sensible level, pursuits the Prey when spotted in a close proximity. Hunter still runs into the walls for an unknown reason.
**Prey** does not show much change in its' behaviour .
### Cumulative Reward Plot:
![Cumulative Reward](Observations/Simple%20Square%20Environment/Ray%20Sensor%20with%20Rotation%2012.03/CumulativeReward.png)
### Entropy Plot:
![Policy Loss](Entropy.png)
### Value Loss Plot
![Value Loss](Observations/Simple%20Square%20Environment/Ray%20Sensor%20with%20Rotation%2012.03/ValueLoss.png)
*How well the model is able to predict the value of each state - this should increase while learning and then decrease once stabilised. [Documentation Reference](<https://unity-technologies.github.io/ml-agents/Using-Tensorboard/#:~:text=Losses/Value%20Loss%20(PPO%3B,decrease%20once%20the%20reward%20stabilizes.>)* 

Models #11 and #12 have a smaller loss compared to models with an additional observation and stacked observations.

## Final Observations:
Stacked observations and increased number of steps is not really helping in training the model. The additional observation was, however, a step in a right direction as the behaviour of the Hunter Agent improved.
