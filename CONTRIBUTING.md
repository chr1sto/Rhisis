
# Contributing to Rhisis

First of all, thank you for taking the time to contribute to the Rhisis project!

This guide is here to show you how to contribute to the project and make Rhisis the best FlyFF emulator in the world!
Basically, everyone with a minimum of knowledge in computer programming and the C# language can develop a feature or fix an issue. You just need a strong will to learn and improve yourself.

## How to contribute

The Rhisis project has two main branches.
- `master` : containing the stable source code of the emulator
- `develop` : containing the development source code.

All the devevlopments should be based on the `develop` branch. Same goes for the Pull Requests.

Before jumping into the code and develop features or fix bugs, you need to setup your development environment.

We recommend to use [Visual Studio IDE](https://www.visualstudio.com) (For Windows or Mac). Plus, you need to install the [.NET Core 2.0 SDK](https://www.microsoft.com/net/download) and the .NET Framework 4.6.1.

The Rhisis project follows the [GitHub Flow](https://guides.github.com/introduction/flow/index.html) in order to make contribution easier.
The steps to contribute to Rhisis are the following :

1. Create a **fork** of the Rhisis `develop` branch. ([How to fork a GitHub repository](https://help.github.com/articles/fork-a-repo/))
2. Clone your **fork** to your local disk and open the folder.
3. Configure the remote `upstream` of the official Rhisis repository witht the command:
```
$> git remote add upstream https://github.com/Eastrall/Rhisis
```
4. Open `Rhisis.sln` with Visual Studio.
5. Do a right click on the solution name and click `Build Solution`
6. Configure the Rhisis database using the `Rhisis.Configuration` tool found in `bin/tools/Rhisis.Configuration/`
7. You are now ready to start the development of your feature or bug fix.
8. Once you are over with your feature/bug fix, you can open a Pull Request to merge your code into the official repository. [Create a Pull Request](https://help.github.com/articles/about-pull-requests/)

Once you've realised these steps, your Pull Request will be reviewed by the project managers and the contributors. If something's wrong, we will give you some constructive advices to improve your code.

**Warnings**

When your feature / bug fix is ready, you need to rebase your solution on `upstream/develop`. Eventually, between the time you have develop your feature and the time you are submitting it, some commits could have been merged. So you will potentially have conflicts. More informations [here](https://git-scm.com/docs/git-rebase).

Follow this commands:
```
$> git fetch
$> git rebase upstream/develop
```
If there is conflicts, resolve them and then execute the command:
```
$> git push
```

## Coding guidelines

There is some rules you should respect when you contribute to the Rhisis project.

We try to respect the [Microsoft C# Coding Guidelines](https://msdn.microsoft.com/en-us/library/ff926074.aspx) so please, try to do so.

- **Please** try to write simple and readable code.
- **Please** try to write explicit commit messages.

_Reminder:_ Every pull requests not respecting this guidelines will be declined.

## Thanks

Thank you for reading this guide and helping us improving Rhisis!
