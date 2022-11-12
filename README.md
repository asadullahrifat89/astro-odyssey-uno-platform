# astro-odyssey-uno-platform
A space shooter game built with Uno Platform and WASM. The goal is to survive and score as far as you can by destroying meteors, enemies, and defeating level bosses.

Have a go at it here: https://asadullahrifat89.github.io/astro-odyssey-uno-platform/

![master](https://github.com/asadullahrifat89/Astro-Odyssey-Uno-Platform/actions/workflows/main.yml/badge.svg)

## Game features:

### Game objects:

#### Players:
* Three ship classes to choose from. Each one have unique abilities and projectiles.

#### Enemies:
* Projectile firing enemies.
* Sideways flying enemies that appear from a corner of the screen.
* Hovering enemies that fly left to right.
* Suicidal enemies that target the player and tries to collide.
* Boss enemies that hover across the screen and shoot rapidly and have giant health bars.
* Enemy sizes vary randomly.

#### Bosses:
* Three unique bosses who have their own projectile firing style and attack patterns. As you level up they get tougher.

#### Meteors:
* Slowly moving boulders that fly away if hit.
* Meteor sizes vary randomly.

#### Power ups:
* Random power ups that last for a certain period of time. A power up gauge is displayed. 3 types of power ups are implemented:
  * Blaze Blitz: if picked up increases firing frequency with faster, smaller projectiles.
  * Plasma Bomb: if picked up decreases firing frequency but with deadlier larger projectiles.
  * Beam Cannon: if picked up decreases firing frequency but with the most devastating, longer, larger and fast projectiles.

#### Pickups:
* Health pickups, if picked up restores health points.

### Game environment:
* Player earns scores for hitting enemies and meteors.
* Player looses health if collides with enemies or meteors.
* Game over if player health reaches zero.
* Game levels increase at certain scores and difficulty scales.
* PhysX effects for projectile impact.
* Explosion effects for enemy or meteor destruction.
* A parallax space background filled with stars.
* Plays at roughly 56 FPS with 18 milliseconds frame time.
* A random background music plays during each game session.
* Sound effects for player health loss.
* Sound effects for player projectile fire.
* Sound effects for player projectile collision with enemy or meteor.
* Sound effects for enemy or meteor destruction.
* Sound effects for power ups and power downs.
* Sound effects for pickups.

## Screenshot
![asadullahrifat89 github io_astro-odyssey-uno-platform_(iPad Mini)](https://user-images.githubusercontent.com/25480176/201468727-000868b4-2c1c-4015-bdaa-431699d999e5.png)
