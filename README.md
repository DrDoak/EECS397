# team8-working
Emails:
luoleizhao2018@u.northwestern.edu

vinaypatel2018@u.northwestern.edu

Following Unity conventions:

Source code can be found under Tsuro/Assets/Scripts/

All test cases can be found under Tsuro/Assets/Editor/

# No Makefiles

MakeFiles and executables not yet created, although we spent a great deal of time investigating the issue. We initialy built the project with Unity and used its libraries and features extensively. While we can create executables locally, we cannot yet build the program on t-lab computers without the Unity specific dependencies.

The project currently can only be built if the user has Unity installed. Instructions are provided on how to use the Unity interface for unit testing purposes.

# Instructions are running tests and creating tests

## Step 1: Downloading Unity 

Unity is required in order to build our project. This includes adding new Unit tests for our project. Fortunately, Unity provides a free version that is 100% sufficient for our needs. Feel free to skip this section if you already have Unity installed.

You can download Unity from [The Unity Store](https://store.unity.com/)

From here, be sure to select the Free version of Unity.
![downloadingfreeversion](https://raw.githubusercontent.com/nwu-software-construction/team8-working/master/ReadmeImages/Step1GetUnity.png)

You will be taken to a screen where you can download an installer. You can click on the text below to switch between the MacOS and Windows versions of Unity. Linux is unfortunately not supported.
![alt text](https://raw.githubusercontent.com/nwu-software-construction/team8-working/master/ReadmeImages/Step2InstallUnity.png)

After downloading the installer, run the executable and following the prompts.

## Step 2: Running Unit Tests

First, clone this repository onto your computer. 

After cloning, you will need to open up our project in Unity. Simply double click on the file located under Assets/Scenes/Main.unity.

![alt text](https://raw.githubusercontent.com/nwu-software-construction/team8-working/master/ReadmeImages/Step3GetUnity.png)

From here, we want to open the Unit Test Runner, which can be found by clicking the "Windows" drop down menu, and going down to "Test Runner"
![alt text](https://raw.githubusercontent.com/nwu-software-construction/team8-working/master/ReadmeImages/Step4OpenTestRunner.png)

This should bring up a new window called "Test Runner". Make sure that the "Edit Mode" button is selected as shown in the screenshot. From here, it will display all Unit Tests currently found in the "Editor" folder of the directory. It may take a minute or two for the Test Runner to find the tests.

When tests are found, press "Run All" to go through all the test cases.

![alt text](https://raw.githubusercontent.com/nwu-software-construction/team8-working/master/ReadmeImages/Step5OpenTestRunner.png)

For additional help with the Test RUnner, refer to the [Unity Test Runner Documentation](https://docs.unity3d.com/Manual/testing-editortestsrunner.html)

## Step 3: Writing your own Unit Tests

All Unit tests should be placed in the "Editor" folder and are C# files. The easiest way to create a new TestCase is to create a template with Unity.

Under the projects tab, go to the Editor folder and write click. 

The navigate to Create->Testing->Editmode C# Scripts, as shown in the image below.

![alt text](https://raw.githubusercontent.com/nwu-software-construction/team8-working/master/ReadmeImages/Step6CreateNewTest.png)

This will create a New C# File holding your test. You should rename it to something describing your test. The Template comes with Two example Test cases: "SimplePasses" and "WithEnumeratorPasses" and Your test Runner should quickly find those two test cases.

For Our tests, just use the SimplePasses as a template for writing all your new test cases.

For additional Resources, refer to the [Unity Test Writing Documentation](https://docs.unity3d.com/Manual/PlaymodeTestFramework.html) 