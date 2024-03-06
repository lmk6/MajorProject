
## Environment
The environment is a 100x100 Unity3D Terrain object modelled by myself.
It contains collidable trees and has an uneven terrain for better simulation.
## Neural Network:
- 128 neurons per layer
- 2 layers
## Observation Space (4):
- Position (x, y, z)
- Ray Sensor's angle
## Continuous Actions:
- Move forward and backwards
- Rotate to left and right
- Rotate Ray Sensor up and down
## Hunter Reward System:
- Every Action: -0.05
- Falling off the map (y < 0): -100
- Collision with Prey: +50
## Prey Reward System:
- Every Action: +0.3
- Falling off the map (y < 0): -100
- Collision with Hunter: -50

## Logic behind rewards
Hunter receives a penalty for every action to encourage a more decisive action.
Prey receives a reward for staying alive by taking any action.
Falling off the map gives twice the penalty to radically discourage such behaviour - better to loose than fall.

## Training Observations:
Both agents act quite confused and do not really try to move, if they move, only in a small proximity from the spawn point.
Eventually, the agents end up rotating or shaking. Hunter agent tries very small movement.
### Computational cost
The training lasted exactly 51 minutes and 31 seconds, so far it is the longest training I conducted.
Visual observations were no good as the Scene in Unity became very laggy.

## Post Training Result:
**Hunter** agent moves back and forth, within a very small proximity, displays a high level of uncertainty. It can never reach the Prey agent unless by a sheer luck - by spawning in a close proximity.

**Prey** agent shakes and does little movement, the reward system encourages it.
### Cumulative Reward Plot:
![Cumulative Reward](CumulativeReward.png)
*Blue represents Prey and pink represents Hunter.*

On the plot above, we can clearly see that Hunter does not do anything to improve its result, ending up with approx. -189.
Prey on the other hand, is rewarded way too much, resulting in not even trying to change its behaviour. It ends up with a huge reward of +911.
### Policy Loss Plot:
![Policy Loss](PolicyLoss.png)
*Policy Loss shows how much the process for choosing actions (policy) is changing. [Documentation Reference](https://unity-technologies.github.io/ml-agents/Using-Tensorboard/#:~:text=Losses%2FPolicy%20Loss%20(PPO%3B,of%20the%20value%20function%20update.)*

The policy loss for both agents is almost the same, standing at 0.023. On this plot, we can see that policy loss has very little variation, approx. between 0.021 and 0.025. The policy loss should be generally less than 1 which means that the loss for this training is very small, resulting in no almost no change in the behaviour for both agents. It actually interesting to see that the Hunter agent, at the end, gives up completely and tries to minimise the penalty it gets.
### Value Loss Plot:
![Policy Loss](ValueLoss.png)
*How well the model is able to predict the value of each state - this should increase while learning and then decrease once stabilised. [Documentation Reference](https://unity-technologies.github.io/ml-agents/Using-Tensorboard/#:~:text=Losses/Value%20Loss%20(PPO%3B,decrease%20once%20the%20reward%20stabilizes.)* 

Prey Agent seems to have no idea what it gets the reward for and Hunter seems to stabilise itself from early on due to constantly failing making any improvement.
## Final Observations:
The reward system is not suitable for the actions available for the agents to choose from.
Prey agent should only be rewarded for surviving and Hunter agent should not be discouraged from making a move to explore the environment and catch the prey. Perhaps a reward for spotting a prey would encourage the behaviour. 