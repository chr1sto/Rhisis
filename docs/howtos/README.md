# Installing Rhisis on Windows 10

## Table of contents
<!--ts-->
* [Installing Rhisis on CentOS 7](https://github.com/Eastrall/Rhisis/blob/develop/docs/howtos/INSTALL-CENTOS7.md)
* Installing Rhisis on Windows 10
   * [Command Prompt](#Command-prompt)
   * [Installing .net core 3.1](#Installing-.net-core)
   * [Installing Git for Windows](#Installing-Git-for-Windows)
   * [Installing MySQL](#Installing-MySQL)
   * [Creating a user and table in MySQL for Rhisis](#Creating-a-user-and-table-in-MySQL-for-Rhisis)
   * [Installing Rhisis](#Installing-Rhisis)
<!--te-->
___
## Command prompt

1. Some of the actions we will do here require the command prompt, so here I will show you a very easy way to open it.

2. By pressing the <kbd>win</kbd>+<kbd>r</kbd> it will open the execute window

3. Now simple type in cmd.exe and press RUN, that's it :)

## Installing .net core 3.1

1. Download .net core 3.1 SDK from Microsoft at https://dotnet.microsoft.com/download 

2. Now run and install the file you just downloaded

3. Now let's check its properly installed by running the version check command by running it from the [command prompt](#Command-prompt):

```
dotnet --list-sdks
```

4. The above should return something like this based on the version you have installed:

```
3.1.101 [C:\Program Files\dotnet\sdk]
```

## Installing Git for Windows

1. Download git at https://gitforwindows.org

2. Now run the file you have just downloaded git-X.XX.X-64-bit.exe or git-X.XX.X-32-bit.exe, X.XX.X is the version number you have downloaded.

3. Click next to accept the License.

4. Select the folder you wish to install it and click Next.

5. For the components window you can leave it as is and just click Next.

6. Choose whether or not you want to setup a start menu folder and click Next.

7. Select the editor you feel more confortable with for git to open the files when needed, then click Next.

8. For Adjusting your Path environment you can leave the recommended option and just click Next.

9. For HTTPS transport backend, you can just click Next as well.

10. For ending conversions you can leave as is and just click Next.

11. For terminal leave the selected option and click Next.

12. For extra options don't change anything, just click Install.

13. Uncheck both checkboxes and click Next and you're done with git!

14. Now let's check if its working as expected, by opening the [command prompt](#Command-prompt) and running the command:
```
git version
```

15. The above should return something like this based on the version you have installed:
```
git version 2.25.0.windows.1
```

# Installing MySQL

1. Download the MySQL Installer from MySQL website at https://dev.mysql.com/downloads/installer/

2. Now run your MySQL installer and select "Developer Default"

3. For the next windows "Check Requeriments" you can leave as is, and just do next and click yes.

4. It will show you everything it will install, you can go ahead and press Execute.

5. Once its done installing, click Next again.

6. Once again click Next, then select Standalone MySQL Server/Classic MySQL Replication then click Next.

7. You can leave the configurations unchanged and click Next.

8. Again leave the recommended for password and click Next.

9. Now define your root password for your MySQL server and click Next.

10. Again leave the configurations as is for the Windows Service and click Next.

11. Now press Execute and wait.

12. Once its done click Finish.

13. Click Next for product configuration.

14. For MySQL Router Configuration leave everythign as is and click Finish.

15. Now for Connect To Server, type in your password at the bottom, click Check and then the Next button will be enabled, click it.

16. Now press Execute to apply the configurations as needed and once its done click Finish.

17. Click Next again, uncheck both checkboxes and you're done!

## Creating a user and table in MySQL for Rhisis

1. First let's open the [command prompt](#Command-prompt)

2. Now let's move into the MySQL server folder by running the following command:

```
cd "C:\Program Files\MySQL\MySQL Server 8.0\bin"
```

3. Now let's connect to our server with the following command:

```
mysql -u root -p
```

4. You should now see the MySQL command line, `mysql>` so let's create a database by running the command:

```
CREATE DATABASE rhisis DEFAULT CHARACTER SET UTF8 COLLATE utf8_general_ci;
```

5. Now let's create a user for this database(skip to step 5 if you want to use root as your user)

```
CREATE USER 'rhisis'@'localhost' IDENTIFIED BY 'password of your choice';
```

6. Now let's assign that user to our database:

```
GRANT ALL PRIVILEGES ON rhisis.* to 'rhisis'@'localhost';
```

7. Now we update to save the changes and have it working with:

```
FLUSH PRIVILEGES;
```

8. Now to exit MySQL console:

```
quit
```

## Installing Rhisis

1. First let's open the [command prompt](#Command-prompt).

2. Now go to the folder you would like to have Rhisis on, for example, if I wanted it in the Desktop, I would do:

```
cd "C:\Users\admin\Desktop"
```

3. Now let's use git to download Rhisis's latest dev version:
```
git clone --recursive https://github.com/Eastrall/Rhisis.git
```

4. Now let's move into the repository we just cloned

```
cd Rhisis
```

5. Now let's build it:

```
dotnet build
```
   NOTE: Depending on your server spec this might take awhile, on a 1vCPU with 2GB it took roughly 10 seconds.

6. Now let's configure the server, by running the following commands in the [command prompt](#Command-prompt), 1 by 1:

```
rhisis-cli.bat database initialize
rhisis-cli.bat configure login
rhisis-cli.bat configure cluster
rhisis-cli.bat configure world
```

7. Now let's create an account:

```
rhisis-cli user create
```
  * Fill in the username, email, password and confirm your password, when it asks you for the salt, use `kikugalanet`

8. Congratulations, you're ready to run your own FlyFF server!

9. Now use the Windows Explorer and go to your Rhisis\bin folder and start all these 3 files, 1 by 1:

```
1.login.bat
2.cluster.bat
3.world.bat
```

10. Now you can connect to your server and enjoy! Have fun!