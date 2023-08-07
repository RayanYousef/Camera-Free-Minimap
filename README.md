# Camera Free Minimap (Unity)
![alt text](https://github.com/RayanYousef/Camera-Free-Minimap/blob/main/Misc/Camera%20Free%20Minimap%20.png?raw=true)
<br/>

"Camera Free Minimap" is a free Unity asset designed to simplify the creation of dynamic
and camera-independent minimaps. Say goodbye to the complexities of setting up a second
camera or manually updating your minimap. With "Camera Free Minimap," you can
effortlessly integrate your minimap images, player icons, and camera FOV, leaving the asset
to handle the rest for you

## How does it work?
Camera Free Minimap utilizes a GameObject with sprite renderer in the world that should perfectly align with your world's environment. This sprite renderer helps capture the X and Z coordinates, representing the minimum and maximum boundaries of your world. By calculating the world's size and tracking the player's position, Camera Free Minimap effectively maps the player's location on the minimap, so make sure to align the sprite renderer with your environment perfectly as it is crucial. 
(The "Minimap Manager" acts as the sprite renderer, although you can choose any other gameObject for this purpose, it's essential to assign its reference to the Minimap Manager.)

## IMPORTANT:
-	Avoid scaling the minimap or its holder as it can interfere with the calculations, leading to incorrect player icon positions on the minimap, to change the size, you can change the width and the height of the Rect Transform.
-	The current version is optimized for Canvas with a Canvas Scaler set to "Scale With Screen Size" and "Constant Pixel Size." It won't function correctly with the "Constant Physical Size" mode.

##Limitations and Future Plans: 
**Current Version Limitations:**<br/>
-	The current version of the minimap is static and best suited for dungeons, mazes, RPG maps, and other static environments. It is not designed for games where the camera moves with the player. To enable dynamic movement, modifications to the code are necessary. 
-	Expanding the elements on the minimap must be done manually, as the present version supports only the player in the "Minimap Manager."
**Future Plans:** <br/>
-	Creating a dynamic version of the minimap that moves along with the player, revealing more of the world map as they explore. 
-	I aim to enhance the minimap's functionality by allowing the addition of more UI elements beyond the player representation. 
-	I am working on implementing a zoom in and out feature for better map navigation and exploration.



## Features:

 Camera Independence: No need for a second camera setup, ensuring a lightweight
and performance-optimized solution.
- Image-Based Rendering: Use custom minimap images and player icons for
creative freedom in your game world.
- Automatic Updates: The minimap dynamically reflects player movements and
camera FOV in real-time gameplay.
- User-Friendly Setup: Seamlessly and intuitively set up with minimal configuration
for developers of all levels.
- Customizable Icons: Tailor player and camera FOV icons to match your game's
style and convey crucial information effectively
<br/>
Video Link: 
https://www.youtube.com/watch?v=6KKvphn_3h8&t

**Feel free to utilize the asset and customize the code to perfectly align with your specific requirements.** 
