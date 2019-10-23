# 3mixer
3mixer creates a 3-mixed image can be used in emulation front end menus. 

Usage: 

**1.** Grab a background image of a game, which is typically a screenshot. This file can be any image type but it rename it 'back.png'.

**2.** Rename the left image to 'left.png'. The left image is typically the box art. **This image must be a .png file.**

**3.** Rename the right image to 'right.png'. The right image is typically the game name header art. **This image must be a .png file.**

**4.** When the images are all located in the same directory as the 3mixer.exe file, run the 3mixer application to build the new image. The rendered file will be named 'output.png'.

**5.** The background opacity can be changed by supplying a decimal argument ranging from 0 to 1. **Example:** 3mixer.exe -0.9

![Example image](https://github.com/bgillman22/3mixer/blob/master/output.png?raw=true)


This application will require the **.NET Framework 4.6.1** or greater.
