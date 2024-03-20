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
Hunter will now get fully penalised for reaching starvation to respect the hunger increase.
## Training Observations:
*Models #15-17 are used as a point of reference.*
Removing all the by-step penalties and keeping the rewards for spotting Prey was a mistake as the model seems to be slow at learning.
## Post Training Result:
Hunter stopped walking into the walls which is quite an improvement.
Unfortunately, Hunter seems to run out of time before catching the prey.
I suspect the current reward for spotting an agent to be the cause of this.
### Cumulative Reward Plot:
![Cumulative Reward](CumulativeReward_1.png)
Hunter receives too high reward from looking at the Prey.
### Entropy Plot:
![Policy Loss](Entropy_1.png)
Hunter seems to have even lower entropy than recorded before
### Value Loss Plot
![Value Loss](ValueLoss_1.png)
*How well the model is able to predict the value of each state - this should increase while learning and then decrease once stabilised. [Documentation Reference](<https://unity-technologies.github.io/ml-agents/Using-Tensorboard/#:~:text=Losses/Value%20Loss%20(PPO%3B,decrease%20once%20the%20reward%20stabilizes.>)* 
Lesser value loss.

## Final Observations:
Replacing the timer with hunger and adding it as an observation seems to have stopped Hunter from walking into the wall but caused him to stare at Prey instead of catching it.
