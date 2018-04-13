# The Math Attack

Blast aliens away in this fun math-themed shooter for the Universal Windows Platform!

[![](https://i.imgur.com/9Dai8JV.png)]()

The Math Attack is a take on classic overhead shooters! 

It is a Universal Windows App designed for use with all Windows devices except for the XBox.

---

## Prerequisites

**You must have a computer with a minimum of Visual Studio 2017 installed.**

**If you have a Mac you will need Parallels or a hardware virtualizer of your choice to run Visual Studio 20xx.**

## Installation

Currently the app is not available on the Window's Store but it has been submitted for certification.

In the meantime the only way to play The Math Attack is to clone it to your local machine and run it from Visual Studio.

1. Open your terminal or command line (if you're on a Mac you will need Parallels or some other hardware visualisation tool)

2. Clone the repository with `git clone https://github.com/mattshiel/TheMathAttack`

3. Change directory into the project and open ***MathAttack*** folders until you see ***MathAttack.sln***, open this file.

4. You should now see the solution open in Visual Studio. Run on local machine to play **The Math Attack**.

**Additional Note** 

This game references the NuGet packages Win2D.uwp or Win2D.win81, which will be automatically downloaded when you build the sample, so there is no need to separately install Win2D itself.

---

## Technologies Used

The Math Attack was built using Win2D, a Windows Runtime API for immediate mode 2D graphics and GPU acceleration.

I chose Win2D as I wanted to develop a 2D UWP game and had no experience in using C#. Having previously developed games with HTML5 Canvas and Javascript, Win2D was an attractive choice. It too integrated a canvas-like control that integrated with XAML seamlessly.

## Features

The Math Attack is a **multi-page app** that provides an **intuitive UI** and great gaming experience.
The three main screens are the ***start screen***, ***gameplay screen*** and ***score screen.***

The game was designed with the different users in mind, that is why the app is completely **scalable**. Whether you're on a 40' device or a tablet, the game and its resources will scale accordingly.

The game is **touch screen friendly**.

Users with touchscreens will appreciate the support for the **inclinometre sensor**. Although it is only a 2D game and the ***roll and yaw*** mechanics are unneeded, I thought the ***pitch*** mechanic (swaying left and right) would be a good fit for the game's weapon, which user's can move on the bottom of the screen.

**Projectile speed** is dependent on where the user taps on the screen. Tapping closer to the weapon will cause a slow velocity, whereas tapping further away from the weapon will fire a **quick shot.**

All of these features were considered in advance with the goal of furthering the interactivity of the app.

## Tests

I tested this app with another computer running Windows 10 Anniversary Edition and Visual Studio 2017. The app scaled correctly on a larger 2k monitor and ran as designed.


## The Game in Motion

![Imgur](https://i.imgur.com/he0eA1b.gif)


## References

My guide while starting out it gave great direction on the different technologies:
https://docs.microsoft.com/en-us/windows/uwp/gaming/e2e

A good video series on Win2D that was useful while developing:
https://www.youtube.com/watch?v=XrVvoay7afg

A resource I utilised heavily for my inexperience with C#
https://stackoverflow.com/

---

## Support

Reach out to me at one of the following places!

- Email at `mattshiel@gmail.com`
- Twitter at <a href="https://twitter.com/ShielMatthew?lang=en">`@ShielMatthew`</a>

---

## License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)

- **[MIT license](http://opensource.org/licenses/mit-license.php)**
