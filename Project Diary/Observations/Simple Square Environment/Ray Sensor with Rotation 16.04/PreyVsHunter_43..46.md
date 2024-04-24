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

## PreyVsHunter_43:
