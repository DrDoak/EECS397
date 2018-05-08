# team8-working
Emails:
luoleizhao2018@u.northwestern.edu

vinaypatel2018@u.northwestern.edu

Following Unity conventions:

Source code can be found under Tsuro/Assets/Scripts/

All test cases can be found under Tsuro/Assets/Editor/

MakeFiles and executables not yet created, although we spent a great deal of time investigating the issue. It may take some time to get make files ready that can work on tlab. We initialy built the project with Unity and used some libraries associated with it. While we can create executables locally, we cannot yet build the program on t-lab computers without the Unity specific dependencies.  We may create a seperate version independent of Unity features for running tests, since we are intending mainly to use Unity for GUI.

This version will essentially just be C-sharp running on .Net 3.5, with the NUnit library for testing. I have contacted root@eecs regarding C-sharp compilers on T-lab, as I was not able to find any myself. 

The project currently can only be built if the user has Unity installed.
