# Untitled Tower Defense Game Design Document
I should really come up with a title for this game.



# Goals & Concepts
## Relaxed gameplay with short sessions
This game should, first and foremost, be a game that is chill to play for 5 or
10 minutes.

If there are scenarios that take longer than 10 minutes, I will need to
"quick saves", where a player can halt a battle to quit the game and resume it
next session.


## Do not overwhelm the player
This game should have a colorful amount of variety that unlocks _slowly_. It
should be apparent to the player early on that there is depth to discover in
the game despite the fact that they only have access to a small amount of
content.

Revealing cool art and music early on will be a great way to entice players
without overwhelming them (contrast this with making lots of mechanics
available in the early game.)



# Gameplay Mechanics
## Unit Types
| Name           | Range      | Durability   | Speed     | Damage    | Aesthetic Notes   | Other Notes                                  |
|----------------|------------|--------------|-----------|-----------|-------------------|----------------------------------------------|
| Knight         | Very Short | Very Sturdy  | Fast      | High      | Sword             | -                                            |
| Dragoon        | Short      | Sturdy       | Slow      | Very High | Polearm           | -                                            |
| Fire Mage      | Moderate   | Very Fragile | Very Slow | High      | Open hand casting | Area of Effect                               |
| Thunder Mage   | Moderate   | Very Fragile | Slow      | High      | Open hand casting | -                                            |
| Crossbowman    | Long       | Sturdy-ish   | Slow      | Moderate  | -                 | -                                            |
| Archer         | Very Long  | Fragile      | Slow      | Low       | Wooden bow        | -                                            |
| Javelin Archer | Very Long  | Very Fragile | Moderate  | Low       | AOE2 skirmisher   | Pierce                                       |
| Warrior        | Short      | Sturdy-ish   | Moderate  | Moderate  | Battle axes       | Taunts enemies (prevents other units from being targeted when in an enemy's range) |
| Healer         | Short      | Very Fragile | -         | -         | Looks like mage   | Targets allies with slow, continuous healing |
| Caravan        | -          | Fragile      | -         | -         | Pirate or bandit  | Generates currency                           |



## Unit Behaviors
### General unit behavior
I think units should target the enemy that is farthest along the path.

There are other things to experiment with such as targeting the enemy with the
lowest health, but we'll see how things go with the above logic first.

### Javelin Archer
It would be great to code javelin archers to find an optimal or at least "good"
throw direction. One naive algorithm I can think of is to check sectors (pie
slices) around the unit for clusters of enemies. Once the sector with the most
enemies is found, try like 3 to 5 raycasts and then throw the javelin in the
direction of the one that collides with the most enemies. This already sounds
a bit complicated for the scope of the project, but I think I should also make
this an "online" algorithm so the unit can think over the course of its wait
time between attacks.

### Healer
Healers should probably target the unit with the lowest health or lowest ratio
of health to max health ("percent health").



## Special Personnel
Special personnel are strong units with in-game narratives. They can be placed
free of charge, but they require the player reaching a certain score in a
scenario.



## Enemy Types
It would be cool to unify the enemies into a single aesthetic so they feel like
a "legion". For example, they might all be orcs or zombies, or mostly undead
with some evil wizards.

| Name   | Move Speed | Durability | Attack Speed | Range | Damage   | Other Notes |
|--------|------------|------------|--------------|-------|----------|-------------|
| Knight | Moderate   | Sturdy     | Moderate     | Short | High     | -           |
| Archer | Moderate   | Fragile    | Moderate     | Long  | Moderate | -           |
| Thief  | Fast       | Fragile    | Fast         | Short | Moderate | -           |
| Mage   | Slow       | Fragile    | Slow         | Long  | High     | -           |
| Healer | Slowish    | Fragile    | -            | Long  | -        | Targets other enemies for slow, continuous healing |

## Enemy Behaviors
### Attacking Units
It might be interesting to have enemies do damage to units. That could make
traps more valuable and also make room for a medic or healer unit.

Would also give clear benefits to ranged units over melee units as opposed to
the linear tradeoff of range vs damage output. Could attach a certain level of
risk to placing frail ranged units directly on a path, too!

### Path of Least Resistance
It would be cool to make enemies avoid things like barracades and caltrops, but
giving them that intelligence could have the unintended effect of making
traps a bad investment or a "flat" mechanic (e.g., there's no point in putting
two traps on a single path branch).



## Trap Types
For now, "trap" will be the catch-all term for entities the player can buy and
place on the map to hinder, damage, or destroy enemies.

### Caltrops
Caltrops damage enemies. Slower enemies take more damage from caltrops.

### Glue Trap
Enemies passing over glue traps are slowed down or get temporarily stuck.

### Land Mines
Land mines cause an explosion that can hit multiple enemies when triggered.



## Iron Mints
Iron mints are the currency used during gameplay to place units. Each map has a
set amount of starting currency. Iron mints do not persist after a battle ends.



## Political Power
The score you earn from completing a level feeds proportionally into earning a
currency called "political power". Political power persists outside of battles,
unlike iron mints.

In the narrative of the game, you are a tactician tasked with defending parts
of a kingdom. The more successful encounters you have, the more dangerous the
efforts entrusted to you.



## Unlocking Scenarios
Completing a scenario unlocks another scenario, but some scenarios can only be
unlocked with political power.



## Barracks
Political power can be spent at the barracks to upgrade units and unlock new
ones.



# Core Gameplay Loop
stub



# Art & Assets
stub

## Story
stub


## Dialog/Scriptwriting
stub


## Character Art
stub


## Environment Art
stub


## Cutscene Art
stub


## Music
stub


## Sound Design
stub



# Accessibility
stub



# Scope
stub



# Scratch

- https://en.wikipedia.org/wiki/Area_denial_weapon



# Differentiating this game from __Fantasy Defense__
(From an ideation session on Sunday 2023-03-12.)

This game is based on SN Mobile Technology's __Fantasy Defense__, but I want to
sufficiently differentiate it as a game, primarily for artistic integrity but
also as a matter of avoiding legal complications involving copyright.

## Things I want to keep from __Fantasy Defense__

### The grid
Placing units in some finite number of squares is more chill than placing them
in arbitrary positions (cause otherwise players have to use \[finicky] touch
screens to try to optimize unit layout).

Note that I could also solve this by allowing players to place units right on
top of each other. (I think Bloons TD might do this for certain units, but I
don't like the way that looks, and that also feels like a "more chaotic, higher
energy" game type of thing.)

### The enemy path
Knowing where enemies are going to travel enables strategy by showing the
player e.g., the positions where an archer's range can cover the most tiles.


## Things I could try that are different

### Bigger maps
I implemented camera controls after all lol

### The art in general
I could go for a narrower and hopefully more interesting aesthetic than the
"general fantasy" look __Fantasy Defense__ seems to go for.

Don't get me wrong, I actually love the character designs in that game, but in
my opinion the aesthetic is not as immersive as it could be. It could do more
to set itself apart from other anime fantasy games.

### The character designs
All of the character designs are a big, fit human with lavish armor. There's
lots of opportunity for variety here.

I could incorporate "fantasy races" like moogles and building units like watch
towers.

### Units with non-circular ranges
Could make things interesting imo, to have units that only attack straight
ahead, or units that attack on both sides at once (like Donkey Kong's
down-smash in Super Smash Brothers Ultimate.)

### Units that move
It might be interesting to have units that move within some range? Not sure how
this would work yet.

Maybe a unit that patrols - a guy who does a ton of damage in one slow hit but
the tradeoff is that he isn't always around? (but how do I communicate to the
player that he needs a patrol path of a certain length?)

### Pierce damage
E.g., a javelin thrower that is set up to fire straight down a path, doing
damage to anything walking on said path.

### AOE2 monk
Converts enemies to fight for you! They'd need to be super slow to stay
balanced if converting an enemy is basically an insta-kill. OR maybe enemies
can be converted _temporarily_.

### Blockades
Maybe players can place things directly on the path that block or slow enemies.

Delving deeper into this, maybe if enemy paths were very complex, blockades
could become pretty interesting, where a level starts out with "we cannot reach
every enemy going through" and becomes "we are routing the enemies into our
defenses" with skillful play.

### Other on-path objects
This is a "type of thing" I can think about when imagining new mechanics.

### Fog of war
Enjoying leaning into the __Age of Empires II__ influence here.
