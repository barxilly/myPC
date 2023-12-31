# myPC
aka. pyPC, myPC is a PC specification calculation software, you input the specifications of a PC and it will give the PC a score in a human-readable format.

## Installation
To install simply download the .zip file from releases, extract it and run setup.exe

## CPU and GPU
You can find your CPU and GPU by opening the start menu and typing `dxdiag` and pressing enter. This will open a window with your CPU and GPU listed. The list of [CPUs](https://github.com/Barxells/myPC/blob/master/App%20The%20Second/CPU.list) and [GPUs](https://github.com/Barxells/myPC/blob/master/App%20The%20Second/GPU.list) can be found here.

## Usage
### Spec Number
The spec number tab calculates a spec number when given a PCs specs. This number is useful for rating a PCs performance.
It works through the formula:
> ((RAM * 0.2) + (RAM Type * 0.1) + (Storage Space * 0.2) + (CPU Passmark Score * 0.3) + (GPU Passmark Score * 0.2)) * 10

### Requirements
This tab works out how well a PC can play some games given a PCs specs.

### Presets
This tab shows you specs and spec scores for your presets. It can also show how well those presets play certain games. You can import custom .pcspec files.

### Compare
This tab shows you the info for all of your presets so you can compare them.
