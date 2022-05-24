# PoseAnnotationTool

A simple 2D keypoint annotation tool

![screenshot](img/screenshot.png)

## How to run

1. Build the project. 
2. Place in the `.exe`  file directory a folder named "images". Example:s

```
PoseAnnotationTool.exe
images
├ folderA
   ├ imageA.jpg
   ├ imageB.jpg
   ├ ...
├ folderB
   ├ imageX.jpg
   ├ imageY.jpg
   ├ ...
├ ...
```

## Different folder
If required:
1. Replace "DefaultDataDir" in Datasets.cs with the root image folder path.
2. Then, build and run (We confirmed it works in Visual Studio 2019)
