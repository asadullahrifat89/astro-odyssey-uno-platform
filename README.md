# Astro-Odyssey-Uno-Platform
A simple space shooter game built with uno platform and wasm. This game adapts to your screen size, generates random enemies, objects and power ups. Plays cool sound effects and a random music each time you play it. And it has some really annoying boss fights. It gets harder as you level up. But also with that, power ups and health pickups get more frequent and your firepower increases. Also the enemies get surprising and start to follow you, shoot at you, crash at you and will do anything to stop you. It's a game built on the soulsborn philosophy. The key objective is how fast and reactive you can be and how high you can score! The game only ends when you die.

Have a go at it here: https://asadullahrifat89.github.io/astro-odyssey-uno-platform/

![master](https://github.com/asadullahrifat89/Astro-Odyssey-Uno-Platform/actions/workflows/main.yml/badge.svg)

## Game features:
### Game environment:
* Player earns scores for hitting enemies and meteors.
* Player looses health if collides with enemies or meteors.
* Game over if player health reaches zero.
* Game levels increase at certain scores and difficulty scales.
* PhysX effects for projectile impact.
* Explosion effects for enemy or meteor destruction.
* A parallax space background filled with stars.
* Plays at roughly 53 FPS with 19 milliseconds frame time.
* A random background music plays during each game session.
* Sound effects for player health loss.
* Sound effects for player projectile fire.
* Sound effects for player projectile collision with enemy or meteor.
* Sound effects for enemy or meteor destruction.
* Sound effects for power ups and power downs.
* Sound effects for pickups.

### Game objects:
#### Enemies:
* Projectile firing enemies.
* Sideways flying enemies that appear from a corner of the screen.
* Hovering enemies that fly left to right.
* Suicidal enemies that target the player and tries to collide.
* Boss enemies that hover across the screen and shoot rapidly and have giant health bars.
* Enemy sizes vary randomly.

#### Meteors:
* Slowly moving boulders that fly away if hit.
* Meteor sizes vary randomly.

#### Power ups:
* Random power ups that last for a certain period of time. A power up gauge is displayed. 3 types of power ups are implemented:
  * Rapid shot: if picked up increases firing frequency with faster, smaller projectiles.
  * Dead shot: if picked up decreases firing frequency but with deadlier larger projectiles.
  * Doom shot: if picked up decreases firing frequency but with the most devastating, longer, larger and fast projectiles.

#### Pickups:
* Health pickups, if picked up restores 10 health points.

## Screenshots
![asadullahrifat89 github io_astro-odyssey-uno-platform_(iPad Mini)](https://user-images.githubusercontent.com/25480176/201468727-000868b4-2c1c-4015-bdaa-431699d999e5.png)
