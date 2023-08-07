# Camera Free Minimap (Unity)
![alt text](https://github.com/RayanYousef/Camera-Free-Minimap/blob/main/Misc/Camera%20Free%20Minimap%20.png?raw=true)
<br/>

"Camera Free Minimap" is a free Unity asset designed to simplify the creation of dynamic
and camera-independent minimaps. Say goodbye to the complexities of setting up a second
camera or manually updating your minimap. With "Camera Free Minimap," you can
effortlessly integrate your minimap images, player icons, and camera FOV, leaving the asset
to handle the rest for you

**How does it work?**
Camera Free Minimap utilizes a GameObject with sprite renderer in the world that should perfectly align with your world's environment. This sprite renderer helps capture the X and Z coordinates, representing the minimum and maximum boundaries of your world. By calculating the world's size and tracking the player's position, Camera Free Minimap effectively maps the player's location on the minimap, so make sure to align the sprite renderer with your environment perfectly as it is crucial. 
(The "Minimap Manager" acts as the sprite renderer, although you can choose any other gameObject for this purpose, it's essential to assign its reference to the Minimap Manager.)


**Features:**

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
