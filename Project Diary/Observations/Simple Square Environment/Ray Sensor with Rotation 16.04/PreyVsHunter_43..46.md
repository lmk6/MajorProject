# Changes in the Environment / Reward System (43-46)

## Single Reward System Change for experiments #43-46
### Hunter rewards system:
- Hunger Increased every second: -1
- Target spotted the first time: +15
- Distance to Target reduced since last closest distance: +0.05
- Reaching maximum on-hunger time: -50
- Falling off the map (y < 0): -100
- collision with wall: -100
- Collision with Prey: +50
### Hunter Observation Space (7):
- Local Position x, y, z (3)
- Angle the sensor's rays are at (1)
- Prey spotted (1)
- Last observed distance to Prey (1)
- Time on hunger (1)
### Prey Reward System:
- Falling off the map (y < 0): -100
- collision with wall: -100
- Collision with Hunter: -50
- Distance to Hunter kept or increased since last measurement: +0.05
- Spotted Hunter the first time: +15
- Surviving: +50
### Prey Observation Space (6):
- Local Position x, y, z (3)
- Angle the sensor's rays are at (1)
- Hunter spotted (1)
- Last observed distance to Hunter (1)

## Noted issue:
The ray's sphere casts were hitting an unidentified object attached to the Hunter Agent essentially disabling him from seeing anything in front of it. I suspect the Hunter's "stick" object, however despite many tries I could not prove it. 

This true issue was discovered after the last experiment, rendering the results of the experiment irrelevant. The element expected to be causing it at first was the large sphere cast size causing the spheres to overlap and block each other. 

As the result, these experiments were focused on testing different sizes of sphere casts, and their lack in hope to troubleshoot the issue. Any conclusions would be false at this time.

## Cumulative Rewards
![Cumulative Reward Chart](CumulativeReward_1.png)