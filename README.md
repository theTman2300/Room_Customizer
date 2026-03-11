# Room_Customizer
A simple Unity room customizer to learn about saving in unity.
Instructions:
- Download the release and unzup the file
- Launch RoomCustomizer.exe
- Windows will most likely give a warning, I promise it's not a virus XD

How to Use:
Furniture:
- Creat new furniture with the buttons on the left
- Select furniture by clicking on them
- Selected furniture can be moved using wasd and moved faster while holding shift
- Selected furniture can be deleted using delete or backspace

Save menu:
- Press escape to open save menu
- Menu in the top right allows you to input a name and save the current room ina new slot
- Menu below that switches modes. Save mode overwrites a slot with the current room. Load mode loads a room and delete mode deletes a save file.
- Slots will show up in the middle of the screen. Click on them to do the action selected in the previous menu

# Devlog
## Introduction
In this devlog I am going to be talking about the proces of creating a simple room customization game in unity. This project is being made to learn about saving data in unity, instead of having to restart from scratch every time you launch the project. In this project I want to add some very simple room customization tools, because the focus is mostly on the saving. I also want multiple save files and some simple save file management. 

## Code structure planning 
Before implementing into unity, I chose to make a plan for the code structure. I did this to make sure that I wouldn’t run into a problem later that would have to cause me to redo a lot of work. It also gives me a clearer view of the entire project. This plan was made using UML in DrawIO. 

<img width="975" height="565" alt="image" src="https://github.com/user-attachments/assets/5bd3f427-ef51-4863-b983-ddbd536e327f" />
This will most likely not have every possible function and variable that I will eventually need, but it has the main things to have a good overview of what I want to make.

## First implementation
I started with implementing some basic scripts for moving, creating and storing furniture. I also added a function to get a ScriptableObject with all the information on the furniture object. This is what I was later planning on using for the saving. This is mostly the top left part of the previous image. The furniture can be moved using wasd after you click on it to select it. It can be deleted when pressing delete or backspace. You can create more using buttons on the side of the screen. 

When creating the scripts for saving the slots, I ran into a problem. I was using a list of ScriptableObjects and trying to serialize that to JSON, but JSONUtility was not capable of doing this. It was capable of serializing an individual ScriptableObject, which is what confused me. I tried some workarounds that I found on the internet, like making a wrapper class and serializing that, but that also did not work. After some research, I found out that what I am trying to do with JSONUtility is not possible (or at least not easily). Instead, I learned about an alternative called JsonConvert by Newtonsoft. This is capable of serializing an array of scriptableObjects in just a single line, making code more readable than if I did use the workarounds.



https://www.newtonsoft.com/json/help/html/t_newtonsoft_json_jsonconvert.htm 
https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.0/manual/index.html
These are the sources I used to learn about JsonConvert. This has the documentation and the unity package.
I made the slots scriptableObjects, because that would make it easy to add more data to a slot later on. You would be able to just expand the scriptableObject without changing any other code. This is the same reason I made the data of the furniture a scriptableObject.
After I figured this out, finishing the basics of slot saving was finished.

Next, I implemented saving the room itself. This was pretty similar to the saving of slots, but there were some different problems. I wanteed to save the save files in a sub-folder, but to do this I had to first create the folder, which is different than how I expected. I also ran into the problem of the ScriptableObjects not getting serialized, which I found out was because of the inclusion of a vector3 variable. I replaced this with a float[], which fixed my problem. 
While combining the slots and the room saving I changed my approuch from using an integer for slot loading, to each slot storing it’s own path. So instead I now pass around a slot ScriptableObject, which made implementation a lot easier.

When making the slot UI, I needed to be able to make new save files, load save files, overwrite save files and later on I also decided to add the ability to delete save files. I didn’t think deleting was necessary at first, but I changed my mind because the save file list would otherwise very easily get crowded. I added a menu to add a new save file by name in the top right. At first, I only added a not stating you wouldn’t be able to add a slot if it has a different name than all other existing slots, but I added an error message when the player tries to do this to make it more clear. Otherwise the player would get no indication something was wrong. The error also explains to the player what options they have to fix it. 

<img width="973" height="536" alt="image" src="https://github.com/user-attachments/assets/d3bf5886-35a9-492b-8a92-1a6ec00fe0cb" />
You can see the slots in the middle here. The names are just the name I gave them. DeleteTest is, in fact, another slot and not a button that deletes something. The same goes for RandomFurnitureSlot. Instead, deleteTest is called that because I was testing whether deleting an object keeps that object deleted when saving and loading.
In the middle on the right, I have a menu where you can select what action wil happen when you press a slot. You can load, save and delete. At first I wanted to use a radio button, which is built in into a C# form, but I realised that wasn’t a thing in Unity and instead made it a few buttons. There is text showing the currently selected mode. 
While making this UI I also needed to implement the delete button, which didn’t have any code for it before. When I made that I realized that there were a few problems. Firstly, the slot objects weren’t getting removed when the menu closed, causing more and more to build up. Appearantly I was deleting the children of the wrong parent object, so I just changed it back to the correct one. The other issue was that when I deleted a slot, I couldn’t make a new slot with the same name. After looking into it I realized that I never actually deleted the save file itself and only deleted the slot reference to it. Good thing I caught that before it got out of hand, that could cause a giant amount of unmanaged save files. Say goodbye to your hard drive!

## First (kind of) test
This is not yet a proper playtest, this was just a test to see if a build of this project works on another persons computer. This worked exaclty as I expected, which is good and means that I will not have any problems that I have to fix about that. This test was done before I had any assets in the game, so it was just shapes and colors for now. After I have implemented the assets, I will do a proper playtest. I have chosen to do this test before proceeding to make sure that there aren’t any major code problems I would have to fix. This is because what I am doing from here on out is more polishing than the actual focus of the project.

## Assets
Before working on this project I had the idea of making assets myself, but since I am not an artist this would take a lot of time. It is also not the focus of what I want to achieve with this project, so I decided to instead use some simple assets from the unity asset store.
The game now looks like this:

<img width="973" height="544" alt="image" src="https://github.com/user-attachments/assets/a39c4e75-19cc-4de5-b180-153ca5f1274a" />
As you can see, there aren’t any assets in the UI. This is because getting assets for UI is a lot more difficult because of different scaling and sizes. Since the focus of this project was on saving and loading, I decided that the default look is good enough for this project.

## Playtests
Since this is more of a technical project, the playtests weren’t very intensive. Most of it was just checking if everything worked on another machine and that there weren’t any bug that I missed. After 2 playtests, I found that you could make a save file with no name, which didn’t seem like a good idea. I fixed that by changing an empty name to always become “defaultSlot”.
From the playtest I also found that the movement of furniture in my game could be improved by allowing players to drag furniture instead of using wasd, however I decided that since that is not the focus of the project and it would require a lot of work to change that mechanic, the way it is now is fine.

## Reflection
This project went very well and was fun to make. I have learned a lot on how to save. I could have done a different approach to saving where instead of saving slots with a reference to a save file, I instead read what save files are in a location. That would have the upside of having less files, while what I have now has the upside of save files being able to be anywhere. This is something I should consider if I do this again. 
