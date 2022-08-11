# Astro-Odyssey-Uno-Platform
A simple space shooter game built with uno platform and wasm. This game adapts to your screen size, generates random enemies, objects and power ups. Plays cool sound effects and a random music each time you play it. And it has some really annoying boss fights. It gets harder as you level up. But also with that, power ups and health pickups get more frequent and your firepower increases. Also the enemies get surprising and start to follow you, shoot at you, crash at you and will do anything to stop you. It's a game built on the soulsborn philosophy. The key objective is how fast and reactive you can be and how high you can score! The game only ends when you die.

Have a go at it here: https://asadullahrifat89.github.io/Astro-Odyssey-Uno-Platform/

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
![Astro-Odyssey-1](https://user-images.githubusercontent.com/25480176/183248690-056e3a11-1ebf-4539-a4d7-f104d6b9ad5b.png)
![Astro-Odyssey-2](https://user-images.githubusercontent.com/25480176/183248692-8456fc4d-c117-4e8a-b7c3-d9dd5d74d8fe.png)
![Astro-Odyssey-3](https://user-images.githubusercontent.com/25480176/183248694-31c3482c-4282-4912-aa25-7613a108c54d.png)
![Astro-Odyssey-4](https://user-images.githubusercontent.com/25480176/183248695-b2d8c386-ebf8-4878-bbd1-b0432bb29d33.png)
![Astro-Odyssey-5](https://user-images.githubusercontent.com/25480176/183248699-3e948232-f435-4c8d-ad7c-da77837fdd36.png)
![Astro-Odyssey-6](https://user-images.githubusercontent.com/25480176/183248700-fdf0cd9d-e83a-4702-a368-6f57fec95815.png)
![Astro-Odyssey-7](https://user-images.githubusercontent.com/25480176/183248703-5d64079c-f1fe-4016-9fab-40eb946397bd.png)

