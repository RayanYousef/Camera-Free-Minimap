# Camera Free Minimap (Unity Asset)
![alt text](https://github.com/RayanYousef/Camera-Free-Minimap/blob/main/Misc/Camera%20Free%20Minimap%20.png?raw=true)
<br/>

## Description
"Camera Free Minimap" is a free Unity asset designed to simplify the creation of 
dynamic and camera-independent minimaps. Say goodbye to the complexities 
of setting up a second camera or manually updating your minimap. With 
"Camera Free Minimap," you can effortlessly integrate your minimap images.
**It Supports Procedurally Generated Maps.**

## KEY FEATURE:
### Single Camera Usage: 
The Camera Free Minimap asset allows you to create a 
minimap without the need for a second camera. This feature is essential for 
improving performance in your Unity projects, as it eliminates the overhead 
associated with rendering a secondary camera view.
### Ease of Setup: 
Setting up the Camera Free Minimap is straightforward and user friendly. You can quickly integrate it into your project, making it accessible for 
developers of all skill levels.
In-Depth Script Comments: The included scripts are well-documented with 
detailed comments. This makes it easier for developers to understand and 
customize the minimap behavior according to their project requirements.
### Two Variants: 
This asset provides two distinct minimap variants:
<br/>• Follow Target Minimap:<br/> You can create a dynamic minimap that follows a 
specific object that you specify. This is valuable for games where you want 
the minimap to focus on a moving character or object.
<br/>• Static Minimap:<br/> Alternatively, you can use the static minimap version for 
games that don't require dynamic minimap tracking. This mode offers a 
fixed overview of the game world.

## HOW DOES IT WORK?
Camera Free Minimap simplifies the process of creating a minimap for your Unity 
project. To understand how it operates, let's break it down into key steps:
### World Size Determination: 
Camera Free Minimap requires knowledge of your 
game world's dimensions. This means it needs to know the minimum and 
maximum X and Z coordinates of your world.
### Utilizing the "World Map" Prefab: 
The asset relies on a specific prefab called 
"World Map," which can be found in the prefab folders. This prefab plays a crucial 
role in generating the minimap.
### Establishing a Connection with the Minimap Image: 
The "World Map" prefab 
needs a reference to your minimap image. The minimap image typically resides 
within the Canvas hierarchy. This connection is vital for the minimap generation 
process.
### Mapping the Minimap Image to "World Map": 
Once linked, the "World Map" 
extracts the sprite from your minimap image. It attaches this sprite to itself using 
a Sprite Renderer component.
### Projection of the Minimap in the World: 
As a result of this action, your minimap 
image becomes a visual representation projected into the game world.
### Alignment with the Game World: 
To ensure the minimap matches your game's 
environment, you need to align the "World Map" with your world's structures and 
layout. This alignment ensures that the minimap reflects the in-game world 
correctly.
### Calculating World Dimensions: 
The "World Map" now possesses the ability to 
calculate the size of your world along the X and Z axes. This information becomes 
essential for determining the position of various elements within the minimap.
### Calculating Minimap Dimensions: 
Using the corners of any UI elements you can determine its dimensions, with that we can calculate the dimensions of the minimap in the UI.<br/>
Knowing the world dimensions and the minimap dimensions the minimap now you can determine the position of the UI elements on the minimap!

## Documentation Link:
https://drive.google.com/file/d/1cMswtkUeIzoFR40DU381zZGATX-FvOhJ/view?usp=drive_link

## Feel free to utilize the asset and customize the code to perfectly align with your specific requirements.
