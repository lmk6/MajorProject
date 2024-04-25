# Hyperparameter changes for the experiment
## Hunter:
- Batch size: 4096
- Buffer size: 40960
- Epochs: 10
## Prey:
- Batch size: 2048
- Buffer size: 20480
- Epochs: 5
# Observation Space:
### Hunter Observation Space (10):
- Local Position x, y, z (3)
- Angle the sensor's rays are at (1)
- Prey spotted (1)
- Last observed distance to Prey (1)
- Time on hunger (1)
- Last observed position of Prey x, y, z (3)
### Prey Observation Space (6):
- Local Position x, y, z (3)
- Angle the sensor's rays are at (1)
- Hunter spotted (1)
- Last observed distance to Hunter (1)
# Reward System:
### Hunter:
- Hunger Increased every second: -1
- Target spotted the first time: +15
- Distance to Target reduced since last closest distance: +0.05
- Reaching maximum on-hunger time: -50
- Falling off the map (y < 0): -100
- collision with wall: -100
- Collision with Prey: +50
### Prey:
- Falling off the map (y < 0): -100
- collision with wall: -100
- Collision with Hunter: -50
- Distance to Hunter kept or increased since last measurement: +0.05
- Spotted Hunter the first time: +15
- Surviving: +50
# Action Space:
## Hunter & Prey (2):
- Forward and Backward (1 Continuous Action)
- Rotation to Left and Right (1 Continuous Action)

# PreyVsHunter_55
## Changes:
Agents now have 5 ray sensors, each placed apart by 15 degrees angle difference to extend the field of view. The sensors are immovable.
## Result of the experiment:
Hunter nearly immediately spots Prey, even from distance and begins to pursue.
Prey searches for hunter but tries to maintain distance. Once being chased by hunter, move backwards facing Hunter front.
The interaction is very dynamic and most of the scenario is the actual pursuit.
Unfortunately, there is no deeper strategy that I can observe. Hunter runs towards the Prey in straight line and Prey runs backwards. The only interesting bits I noticed were when the Hunter slowed down before reaching Prey as if expecting the Prey to go to the side, and when Prey tried manoeuvring close to obstacles to make Hunter loose sight of it and get confused.
Both agents scan the environment well.
***Experiment was captured in https://users.aber.ac.uk/lmk6/MP_ML/prey_v_hunter_3/***

Time to train was very long in comparison, the model took over 4 hours.
# PreyVsHunter_56
## Changes:
Agents now have 3 ray sensors, each places apart by 20 degrees angle difference. Decrease in number of sensors is made in hope of reducing training time and achieving the same result.

## Result of the experiment:
The resulting model is a bit less dynamic but still remains very successful in achieving a long pursuit after a short area scan.

Time to train took significantly less than the previous experiment.

# PreyVsHunter_57
A repetition of experiment #57 to see any significant differences.
## Result
Result is very much the same given the randomness factor.
# Cumulative Reward
![Cumulative Reward](CumulativeReward.png)
All the experiments seem to finally achieve what was expected, so an inevitable spike in Hunter's dominance, followed by the development of a defensive strategy by Prey.
We can see it by the crossing of Prey/Hunter lines in two points.
***Theory:*** Experiment #55 is the most dynamic one as it let Hunter achieve the highest cumulative reward, forcing Prey to change a strategy.
Experiments #55-56 show a more balanced result.
# Entropy
![Entropy](Entropy.png)
Entropy results match the observations, Hunter in experiment #55 is more decisive in taking actions.

# Running in Realistic Environment
Unfortunately, the models overfit the simple environment and perform very poorly based on visual observations.

Also, trying to train these configurations in realistic environment took too long to finish.