# Unity-Basic-Flight-Mechanics
Basic flight mechanics for any rigid body object in Unity 3D. 

# How to use:
First - Import
1) Open a new Unity Project (my case a new URP project).
2) Import the airliner 3D model or any air vehicle model into the unity.
3) Import the any of the available .cs file into the Unity Assest folder.

Second - Initial Setup of 3D Model
1) Add a rigid body component to the 3D model.
2) Optional to add a collider to the 3D model.
3) Drag and drop any of the applicable .cs file to the 3D model.

# Notes:
- Make sure that the project has default input settings
- If in any case that the mouse inputs are not working, it is because the Input Manager is not in the default settings.
- To reset, click 'Edit' -> 'Project Settings' -> 'Input Manager'. Then click on the cog-icon on the top left of the window and click 'Reset'.

# Changes:
- January 30th, 2022: Added Base Class (Aircraft) and Derived Class (Airliner). This allows us to add new aircraft types using a inheritance.
- February 1st, 2022: Stabalized the movements of a game object that is using the Airliner.cs script.
- February 2nd, 2022: Added the Fighter.cs script specifically for Light Aircraft types for Unity. The movements are unstable and the Stall() method is currently not working properly. However, the Manuver() method works fine and it is tuned to fit a small aircraft type. Same to be said for the ForwardSpeed() method.
- February 4th, 2022: Manuverability tuned for Fighter.cs to better control a small aircraft. Serialized the Pitch, Yaw, and Roll strengths for the Fighter.cs script and tuned the Stall() method to detect an aircraft stall better.
