# Astro-Odyssey-Uno-Platform
A simple space shooter game built with uno platform and wasm. The game is completely developed with C# and XAML.
This is to demostrate that 2D games can be developed using these tech stacks.

The game is live at: https://asadullahrifat89.github.io/Astro-Odyssey-Uno-Platform/

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

<table>
  <tr>
     <td>Login</td>
     <td>Select Game Mode</td>
     <td>Select Spacecraft</td>    
  </tr>
  <tr>
    <td><img src="https://user-images.githubusercontent.com/25480176/183200054-d1c1fef3-f0d5-4ac2-86e1-3c2915bfedb7.PNG" height="500"></td>
    <td><img src="https://user-images.githubusercontent.com/25480176/183200137-b63cd766-35b8-40be-ab65-e34f1bbd6095.PNG" height="500"></td>
    <td><img src="https://user-images.githubusercontent.com/25480176/183200346-7a050263-ef47-4633-94b8-5bf1ddcf7722.PNG" height="500"></td>   
  </tr>
 </table>
<table>
  <tr>
     <td>Play</td>
     <td>Fight Bosses</td>
     <td>Score</td>        
  </tr>
  <tr>
    <td><img src="https://user-images.githubusercontent.com/25480176/183200419-a0f930b7-dfe6-4d24-adc3-d44c2a9bbf3f.PNG" height="500"></td>
    <td><img src="https://user-images.githubusercontent.com/25480176/183200523-f938b922-38db-480e-a0a7-d07813ba141a.PNG" height="500"></td>
    <td><img src="https://user-images.githubusercontent.com/25480176/183200595-4b0fbcf7-a6c0-4f61-9f45-ddae55bac1e7.PNG" height="500"></td>
  </tr>
 </table>
